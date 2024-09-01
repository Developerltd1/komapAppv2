using EasyModbus;
using komaxApp.BusinessLayer;
using komaxApp.Utility.ExtensionMethod;
using KomaxApp.GenericCode;
using KomaxApp.Model.Dashboard;
using KomaxApp.Model.Display;
using Microsoft.ReportingServices.RdlExpressions.ExpressionHostObjectModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Utility;
using static KomaxApp.Model.Dashboard.DashboardModel.Manupulation;

namespace KomaxApp.UI_Design
{
    public partial class Dashboard : BaseForm//Form
    {
        private bool isPolling;
        private Timer minuteTimer;
        private Timer pollingTimer;
        private ModbusClient modbusClient;
        int[] registers = { 0 };
        bool[] boolsRegister = { false };
        private const int START_ADDRESS = 0;
        private const int REG_COUNT = 62;

        Int32 modbusStartReg = START_ADDRESS;
        Int32 modbusRegCount = REG_COUNT;
        Int32 modbusTotalReg;

        StringBuilder responseBuilder = new StringBuilder();

        private const int READ_COIL = 0x01;
        private const int READ_HOLDING_REGISTER = 0x03;
        private const int WRITE_REGISTER = 0x06;

        private const int CELL1 = 0;
        private const int CELL2 = 1;
        private const int CELL3 = 2;
        private const int CELL4 = 3;
        private const int CELL5 = 4;
        private const int CELL6 = 5;
        private const int CELL7 = 6;
        private const int CELL8 = 7;
        private const int CELL9 = 8;
        private const int CELL10 = 9;
        private const int CELL11 = 10;
        private const int CELL12 = 11;
        private const int CELL13 = 12;
        private const int CELL14 = 13;
        private const int CELL15 = 14;
        private const int TEMP = 32;
        private const int VOLTAGE = 40;
        private const int CURRENT = 41;
        private const int SOC = 42;
        private const int AH = 48;
        private const int CHARGING_MOS = 53;
        private const int DISCHARGING_MOS = 54;
        private const int POWER = 57;
        private const int FAULT_STATUS_1 = 58;
        private const int FAULT_STATUS_2 = 59;
        private const int FAULT_STATUS_3 = 60;
        private const int FAULT_STATUS_4 = 61;

        private SerialPort serialPort;




        private bool isPollingEnabled = false;
        public static bool isPollSelected = false;

        public string _powerMeter;
        public string _torqueMeter;
        public string _rpm;
        public string _temperature;
        // Dictionary to store SerialPort objects for each COM port
        private Dictionary<string, SerialPort> serialPorts = new Dictionary<string, SerialPort>();


        public Dashboard(string ReportNo, string powerMeter, string torqueMeter, string rpm, string temperature)
        {
            InitializeComponent();
            RefreshComPortList();
            btnStartReadng.Click += buttonReading_Click;

            // Store the configuration values
            _powerMeter = powerMeter;
            _torqueMeter = torqueMeter;
            _rpm = rpm;
            _temperature = temperature;
        }
        private void buttonReading_Click(object sender, EventArgs e)
        {
            try
            {
                isPollingEnabled = !isPollingEnabled;
                if (isPollingEnabled)
                {
                    isPollSelected = true;
                    btnStartReadng.BackColor = System.Drawing.Color.Gray;
                    InitializePollingTimer();
                    StartPolling();
                }
                else
                {
                    isPollSelected = false;
                    btnStartReadng.BackColor = System.Drawing.Color.Black;
                    StopPolling();
                }
            }
            catch (Exception ex)
            {
                JIMessageBox.ErrorMessage(ex.Message);
            }
        }
        private void StopPolling()
        {
            try
            {
                pollingTimer.Stop();
            }
            catch (Exception ex)
            {
                labelInfo.Text = "StopPolling Time: " + ex.Message;
                labelInfo.ForeColor = Color.Red;
            }
        }
        private void StartPolling()
        {
            pollingTimer.Start();
        }
        private void InitializePollingTimer()
        {
            try
            {
                pollingTimer = new Timer();
                pollingTimer.Interval = 1000;
                pollingTimer.Tick += PollingTimer_Tick;
            }
            catch (Exception ex)
            {
                Utility.JIMessageBox.ErrorMessage(ex.Message);
            }
        }
        public void PollingTimer_Tick(object sender, EventArgs e)
        {
            if (_powerMeter == null && _torqueMeter == null && _rpm == null && _temperature == null)
            {
                StopPolling();
                JIMessageBox.WarningMessage("COM Ports are not Configure");
                return;
            }
            #region Data Reading
            try
            {

                // List of COM ports
                List<string> comPorts = new List<string>
                    {
                         _powerMeter,  // COM6
                         _torqueMeter, // COM5
                         _rpm,         // COM4
                         _temperature  // COM7
                    };
                // Corresponding commands for each port
                List<string> commands = new List<string>
                {
                    ":MEAS?",                          // Command for _powerMeter (COM6)
                    "0x23, 0x30, 0x30, 0x30, 0x0d",    // Command for _torqueMeter (COM5)
                    "0x05,0x01,0x00,0x00,0x00,0x00,0x06,0xAA", // Command for _rpm (COM4)
                    null                               // No command for _temperature (COM7)
                };

                // Initialize any ports that haven't been initialized yet
                InitializeMultipleSerialPorts(comPorts);

                // Execute commands on already initialized ports
                ExecuteCommandsOnInitializedPorts(comPorts, commands);

            }
            catch (Exception ex)
            {
                MessageBox.Show("PollingTimer error: " + ex.Message);
            }
            #endregion
        }


        #region InitilizeSerialPortNew
        
        private void InitializeMultipleSerialPorts(List<string> comPorts)
        {
            foreach (string comPort in comPorts)
            {
                if (!serialPorts.ContainsKey(comPort))
                {
                    InitializeSerialPort(comPort, GetCommandForPort(comPort));
                }
            }
        }
        private string GetCommandForPort(string comPort)
        {
            // Define the command based on the port
            switch (comPort)
            {
                case var port when port == _powerMeter: return ":MEAS?";
                case var port when port == _torqueMeter: return "0x23,0x30,0x30,0x30,0x0d";
                case var port when port == _rpm: return "0x05,0x01,0x00,0x00,0x00,0x00,0x06,0xAA";
                case var port when port == _temperature: return null;
                default: return null;
            }
        }
        private void ExecuteCommandsOnInitializedPorts(List<string> comPorts, List<string> commands)
        {
            for (int i = 0; i < comPorts.Count; i++)
            {
                if (serialPorts.ContainsKey(comPorts[i]))
                {
                    SendCommandToPort(serialPorts[comPorts[i]], commands[i]);
                }
            }
        }
        private void SendCommandToPort(SerialPort serialPort, string command)
        {
            try
            {
                if (!string.IsNullOrEmpty(command))
                {
                    byte[] commandBytes = Encoding.ASCII.GetBytes(command + "\r\n");
                    serialPort.Write(commandBytes, 0, commandBytes.Length);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending command to port {serialPort.PortName}: {ex.Message}");
            }
        }
        


        private void InitializeSerialPort(string comPort, string CommandName)
        {
            try
            {
                if (!serialPorts.ContainsKey(comPort))
                {
                    SerialPort serialPort = new SerialPort
                    {
                        PortName = comPort,  // Set the COM port name
                        BaudRate = 9600,
                        Parity = Parity.None,
                        DataBits = 8,
                        StopBits = StopBits.One,
                        Handshake = Handshake.None,
                        ReadTimeout = 90000  // Optional timeout
                    };

                    try
                    {
                        serialPort.Open();  // Attempt to open the serial port
                        serialPorts[comPort] = serialPort; // Add to dictionary after successful open
                    }
                    catch (Exception ex)
                    {
                        labelInfo.Text = $"Error opening serial port {comPort}: {ex.Message}" + Environment.NewLine;
                        labelInfo.ForeColor = System.Drawing.Color.Red;
                        return;  // Exit if the port couldn't be opened
                    }

                    var model = new SerialPortCommandModel(serialPort, CommandName);
                    serialPort.DataReceived += (sender, e) => DataReceivedHandler(model, e);
                    if (CommandName != null)
                    {
                        SendCommandToPort(serialPort, CommandName);
                    }
                }
            }
            catch (Exception ex)
            {
                labelInfo.Text = $"Error initializing serial port {comPort}: {ex.Message}" + Environment.NewLine;
                labelInfo.ForeColor = System.Drawing.Color.Red;

                CloseSerialPort(comPort);
            }
        }
        #endregion
        
        private void AppendTextToRichTextBox(string text)
        {
            // Assuming you have a RichTextBox control named richTextBox1
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(text)));
            }
            else
            {
                richTextBox1.AppendText(text);
            }
        }



        private void DataReceivedHandler(SerialPortCommandModel model, SerialDataReceivedEventArgs e)
        {
            try
            {
                //SerialPort sp = (SerialPort)sender;
                SerialPort sp = model.SerialPort;
                var PortName = sp.PortName;
                string commandName = model.CommandName;
                string inData = sp.ReadLine();
                if (!string.IsNullOrEmpty(inData))
                {
                    AppendTextToRichTextBox(inData);
                    ParseResponse(inData, commandName, PortName);  // You can call ParseResponse to handle the data
                }
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    richTextBox1.AppendText("An error occurred while processing data: " + ex.Message + Environment.NewLine);
                });
            }
            finally
            {
                CloseSerialPort(model.SerialPort.PortName);
            }
        }


        private void RefreshComPortList()
        {
            try
            {
                comPorts.Items.Clear();

                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    comPorts.Items.Add(port);
                }
                // Select the first item if the list is not empty
                if (comPorts.Items.Count > 0)
                {
                    comPorts.SelectedIndex = comPorts.Items.Count - 1;
                }
                else
                {
                    comPorts.Text = "No COM";//, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                JIMessageBox.ErrorMessage(ex.Message);
            }
        }
        private void CloseSerialPort(string comPort)
        {
            if (serialPorts.ContainsKey(comPort))
            {
                SerialPort port = serialPorts[comPort];
                if (port.IsOpen)
                {
                    port.Close(); // Close the port if it's open
                }
                serialPorts.Remove(comPort); // Remove it from the dictionary
            }
        }
        private void CloseAllSerialPorts()
        {
            foreach (var serialPort in serialPorts.Values)
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();  // Close each open serial port
                }
            }
            serialPorts.Clear();  // Clear the dictionary after closing all ports
        }
        

        
        

        #region try for PM data reading
       
        
        private void ParseResponse(string data, string commandName, string PortNo)
        {
            if (string.IsNullOrEmpty(data))
            {
                richTextBox1.AppendText("No data received." + Environment.NewLine);
                return;
            }
            string cleanedData = Regex.Replace(data, @"\+|E\+\d{2}|E[\+\d]+", "").Trim();
            // Split the string by commas
            var dataParts = cleanedData.Split(',');
            var dataResponse = new DashboardModel.Manupulation
            {
                labelV1 = dataParts.ElementAtOrDefault(4) ?? "N/A",
                labelV2 = dataParts.ElementAtOrDefault(5) ?? "N/A",
                labelV3 = dataParts.ElementAtOrDefault(6) ?? "N/A",
                labelV0 = dataParts.ElementAtOrDefault(7) ?? "N/A",
                labelA1 = dataParts.ElementAtOrDefault(8) ?? "N/A",
                labelA2 = dataParts.ElementAtOrDefault(9) ?? "N/A",
                labelA3 = dataParts.ElementAtOrDefault(10) ?? "N/A",
                labelA0 = dataParts.ElementAtOrDefault(11) ?? "N/A",
                labelPf1 = dataParts.ElementAtOrDefault(26) ?? "N/A",
                labelPf2 = dataParts.ElementAtOrDefault(27) ?? "N/A",
                labelPf3 = dataParts.ElementAtOrDefault(28) ?? "N/A",
                labelPf0 = dataParts.ElementAtOrDefault(15) ?? "N/A",
                labelHertz = dataParts.ElementAtOrDefault(16) ?? "N/A",
                labelPower1 = dataParts.ElementAtOrDefault(17) ?? "N/A",
                labelPower2 = dataParts.ElementAtOrDefault(18) ?? "N/A",
                labelPower3 = dataParts.ElementAtOrDefault(19) ?? "N/A",
                labelPower0 = dataParts.ElementAtOrDefault(20) ?? "N/A",
            };
            #region Formula
            dataResponse.labelPower1 = (Convert.ToDouble(dataResponse.labelPower1) / 1000).ToString();
            dataResponse.labelPower2 = (Convert.ToDouble(dataResponse.labelPower2) / 1000).ToString();
            dataResponse.labelPower3 = (Convert.ToDouble(dataResponse.labelPower3) / 1000).ToString();
            dataResponse.labelPower0 = (Convert.ToDouble(dataResponse.labelPower0) / 1000).ToString();
            #endregion
            // Convert the class to JSON
            string jsonResult = JsonConvert.SerializeObject(dataResponse, Newtonsoft.Json.Formatting.Indented);
            // Deserialize the JSON string into a class instance
            var deserializedResponse = JsonConvert.DeserializeObject<DashboardModel.Manupulation>(jsonResult);


            try
            {
                // Update the labels on the UI thread
                this.Invoke((MethodInvoker)delegate
                {
                    labelV1.Text = deserializedResponse.labelV1;
                    labelV2.Text = deserializedResponse.labelV2;
                    labelV3.Text = deserializedResponse.labelV3;
                    labelV0.Text = deserializedResponse.labelV0;
                    labelA1.Text = deserializedResponse.labelA1;
                    labelA2.Text = deserializedResponse.labelA2;
                    labelA3.Text = deserializedResponse.labelA3;
                    labelA0.Text = deserializedResponse.labelA0;
                    labelPf1.Text = deserializedResponse.labelPf1;
                    labelPf2.Text = deserializedResponse.labelPf2;
                    labelPf3.Text = deserializedResponse.labelPf3;
                    labelPf0.Text = deserializedResponse.labelPf0;
                    labelHertz.Text = deserializedResponse.labelHertz;
                    labelPower1.Text = deserializedResponse.labelPower1;
                    labelPower2.Text = deserializedResponse.labelPower2;
                    labelPower3.Text = deserializedResponse.labelPower3;
                    labelPower0.Text = deserializedResponse.labelPower0;
                    labelPortNo.Text = PortNo;
                    labelPortNo.ForeColor = Color.DarkGoldenrod;
                });

                // Optionally append the raw response to the RichTextBox
                richTextBox1.Invoke((MethodInvoker)delegate
                {
                    richTextBox1.AppendText(data + Environment.NewLine);
                });
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    richTextBox1.AppendText("An error occurred while processing data: " + ex.Message + Environment.NewLine);
                });
            }
        }

         
        #endregion

        private void buttonScreenshot_Click_1(object sender, EventArgs e)
        {
            CaptureMdiChildForm(this);
        }

        // Example method to handle form closing
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            CloseAllSerialPorts();  // Close all ports when the form is closing
        }


        #region ScreenShot_Code
        public void CaptureWindow(Form form)
        {
            // Get the form's bounds on the screen (including borders and title bar)
            Rectangle formBounds = form.Bounds;

            // Create a bitmap with the size of the form
            using (Bitmap bitmap = new Bitmap(formBounds.Width, formBounds.Height))
            {
                // Create a graphics object from the bitmap
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    // Copy the form's content into the bitmap
                    g.CopyFromScreen(formBounds.Location, Point.Empty, formBounds.Size);
                }

                // Create and configure the SaveFileDialog
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    // Set default file name with date and time
                    saveFileDialog.FileName = $"screenshot_{DateTime.Now:yyyy_MM_dd_HH_mm_ss_tt}.jpg";
                    saveFileDialog.Filter = "JPEG Image|*.jpg|PNG Image|*.png";
                    saveFileDialog.Title = "Save Screenshot";

                    // Show the dialog and check if the user clicked "Save"
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Save the bitmap to the selected file path
                        string filePath = saveFileDialog.FileName;
                        bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }
            }
        }
        public void CaptureMdiChildForm(Form mdiChildForm)
        {
            if (mdiChildForm != null && mdiChildForm.Visible)
            {
                // Get the bounds of the MDI child form
                Rectangle formBounds = mdiChildForm.Bounds;

                // Create a bitmap with the size of the form's client area
                using (Bitmap bitmap = new Bitmap(formBounds.Width, formBounds.Height))
                {
                    // Create a graphics object from the bitmap
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        // Capture only the form's content (not including borders)
                        Point formLocation = mdiChildForm.PointToScreen(Point.Empty);
                        g.CopyFromScreen(formLocation, Point.Empty, formBounds.Size);
                    }

                    // Create and configure the SaveFileDialog
                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        // Set default file name with date and time
                        saveFileDialog.FileName = $"screenshot_{DateTime.Now:yyyy_MM_dd_HH_mm_ss_tt}.jpg";
                        saveFileDialog.Filter = "JPEG Image|*.jpg|PNG Image|*.png";
                        saveFileDialog.Title = "Save Screenshot";

                        // Show the dialog and check if the user clicked "Save"
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            // Save the bitmap to the selected file path
                            string filePath = saveFileDialog.FileName;
                            bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("The MDI child form is not visible.", "Capture Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

    }
}


