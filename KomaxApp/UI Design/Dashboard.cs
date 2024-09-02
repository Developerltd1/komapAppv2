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
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;
using KomaxApp.GenericCode;
using KomaxApp.Model.Dashboard;



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

        private Dictionary<string, SerialPort> serialPorts;
        private SerialPortCode serialPortCode;

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


            serialPorts = new Dictionary<string, SerialPort>
            {
                    { "COM6", new SerialPort("COM6", 9600, Parity.None, 8, StopBits.One) },
                    { "COM4", new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One) },
                    { "COM5", new SerialPort("COM5", 9600, Parity.None, 8, StopBits.One) },
                    { "COM3", new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One) }
            };

         
        }

        #region Generic
        private void HandleDataReceived(string data)
        {
            // Call ParseResponse to handle the data
            serialPortCode.ParseResponse(data);

            // Update the UI with the new data
            this.Invoke((MethodInvoker)delegate
            {
                // Assuming ParseResponse updates some labels directly
                // You can handle additional UI updates here if needed
            });
        }

        // Initialize the serial ports with their respective commands
        private void InitializePorts()
        {
            var comPorts = new List<string> { "COM6", "COM4", "COM5", "COM3" };
            var commands = new List<string>
        {
            ":MEAS?", // Command for COM6
            "0x23, 0x30, 0x30, 0x30, 0x0d", // Command for COM4
            "0x05,0x01,0x00,0x00,0x00,0x00,0x06,0xAA", // Command for COM5
            null // No command for COM3
        };

            // Initialize serial ports
            serialPortCode.InitializeMultipleSerialPorts(comPorts, commands);
        }

        #endregion
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
                pollingTimer.Interval = 6000;
                pollingTimer.Tick += PollingTimer_Tick;
            }
            catch (Exception ex)
            {
                Utility.JIMessageBox.ErrorMessage(ex.Message);
            }
        }
        public void PollingTimer_Tick(object sender, EventArgs e)
        {
            if (_powerMeter == null && _torqueMeter == null && _rpm == null && _temperature == null) {
                StopPolling();
                JIMessageBox.WarningMessage("COM Ports are not Configure");
                return;
            }

            #region MyRegion
            // Initialize SerialPortCode and subscribe to the event
            serialPortCode = new SerialPortCode();
            serialPortCode.OnDataReceived += HandleDataReceived;

            // Example initialization call (make sure to call this method as needed)
            InitializePorts();
            #endregion
            #region Data Reading
            //try
            //{
            //    //List<string> comPorts = new List<string>  //dynamic
            //    //    {
            //    //        _powerMeter,_torqueMeter,_rpm,_temperature,
            //    //    };
            //    List<string> comPorts = new List<string>
            //        {
            //            "COM6","COM4","COM5","COM3"
            //        };
            //    List<string> commands = new List<string>
            //    {
            //        ":MEAS?",                          // Command for _powerMeter (COM6)
            //        "0x23, 0x30, 0x30, 0x30, 0x0d",    // Command for _torqueMeter (COM5)
            //        "0x05,0x01,0x00,0x00,0x00,0x00,0x06,0xAA", // Command for _rpm (COM4)
            //        null                               // No command for _temperature (COM7)
            //    };
            //    InitializeMultipleSerialPorts(comPorts, commands);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("PollingTimer error: " + ex.Message);
            //}
            #endregion
        }

        // Method to initialize multiple serial ports
        private void InitializeMultipleSerialPorts(List<string> comPorts, List<string> commands)
        {
            //InitializeSerialPort(comPorts[0],);
            //InitializeSerialPort(comPorts[1]);
            //InitializeSerialPort(comPorts[2]);
            //InitializeSerialPort(comPorts[3]);
            for (int i = 0; i < comPorts.Count; i++)
            {
                string comPort = comPorts[i];
                string command = commands[i]; // Corresponding command for the port
                InitializeSerialPort(comPort, command);
            }
        }

        #region InitilizeSerialPortNew
        // Method to initialize a single serial port
        private void InitializeSerialPort(string comPort, string command)
        {
            try
            {
                // Check if the serial port is already initialized
                if (!serialPorts.ContainsKey(comPort))
                {
                    // Initialize the serial port with common settings
                    SerialPort serialPort = new SerialPort
                    {
                        PortName = comPort,
                        BaudRate = 9600,
                        Parity = Parity.None,
                        DataBits = 8,
                        StopBits = StopBits.One,
                        Handshake = Handshake.None,
                        ReadTimeout = 90000  // Set an optional timeout
                    };

                    try
                    {
                        // Open the serial port
                        serialPort.Open();
                        serialPorts[comPort] = serialPort; // Add the port to the dictionary
                    }
                    catch (Exception ex)
                    {
                        // Handle errors during the port opening process
                        labelInfo.Text = $"Error opening serial port {comPort}: {ex.Message}" + Environment.NewLine;
                        labelInfo.ForeColor = Color.Red;
                        return;
                    }

                    // Set up the DataReceived event handler for the serial port
                    serialPort.DataReceived += (sender, e) => DataReceivedHandler(sender, e);

                    // Send the initial command if provided
                    if (!string.IsNullOrEmpty(command))
                    {
                        byte[] commandBytes = Encoding.ASCII.GetBytes(command + "\r\n");
                        serialPort.Write(commandBytes, 0, commandBytes.Length);
                    }
                }
                else
                {
                    // If the port is already initialized, perform the logic that should run every second
                    var serialPort = serialPorts[comPort];
                    if (!serialPort.IsOpen)
                    {
                        try
                        {
                            // Attempt to open the serial port if it is not open
                            serialPort.Open();
                        }
                        catch (Exception ex)
                        {
                            // Handle errors during the port opening process
                            labelInfo.Text = $"Error reopening serial port {comPort}: {ex.Message}" + Environment.NewLine;
                            labelInfo.ForeColor = Color.Red;
                            return;
                        }
                    }
                    try
                    {
                        // Send the command again if provided
                        if (!string.IsNullOrEmpty(command))
                        {
                            byte[] commandBytes = Encoding.ASCII.GetBytes(command + "\r\n");
                            serialPort.Write(commandBytes, 0, commandBytes.Length);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle errors during the command sending process
                        labelInfo.Text = $"Error sending command to serial port {comPort}: {ex.Message}" + Environment.NewLine;
                        labelInfo.ForeColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                // Display an error message and handle cleanup
                labelInfo.Text = $"Error initializing serial port {comPort}: {ex.Message}" + Environment.NewLine;
                labelInfo.ForeColor = Color.Red;

                // Clean up resources if initialization fails
                if (serialPorts.ContainsKey(comPort))
                {
                    serialPorts[comPort].Close();
                    serialPorts.Remove(comPort);
                }
            }
        }


        #endregion
         
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                var PortName = sp.PortName;
                string inData = sp.ReadLine();
                if (!string.IsNullOrEmpty(inData))
                {
                    // Check if we need to use Invoke
                    if (richTextBox1.InvokeRequired)
                    {
                        // Create a delegate to handle the invocation
                        richTextBox1.Invoke(new Action(() =>
                        {
                            richTextBox1.AppendText(inData);
                        }));
                    }
                    else
                    {
                        // Directly update the control if on the UI thread
                        richTextBox1.AppendText(inData);
                    }
                    ParseResponse(inData);  // You can call ParseResponse to handle the data
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
                string comPort = ((SerialPort)sender).PortName;
                CloseSerialPort(comPort);
            }
        }

        // Method to close and remove a serial port
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
        private void InitializeModbusClient()
        {
            try
            {
                //Logger.Info("MainParamaterForm/InitializeModbusClient| comPorts: " + comPorts.Text);
                modbusClient = new ModbusClient("COM3")
                {
                    Baudrate = 9600,//Convert.ToInt32(cbBaudRate1.SelectedItem),
                    Parity = System.IO.Ports.Parity.None,
                    StopBits = System.IO.Ports.StopBits.One,
                    UnitIdentifier = 0x01,//Convert.ToByte(slaveID), // Slave ID
                    ConnectionTimeout = 500//Convert.ToInt32(readingTimeOut.Value) // Timeout in milliseconds
                };

                //statusConnection.Text = ("Connected");

                modbusClient.Connect();
                //Logger.Info("MainParamaterForm/InitializeModbusClient| modbusClient.Connect()");
            }
            catch (Exception ex)
            {
                //Logger.Error("MainParamaterForm/InitializeModbusClient| Exception: " + ex.Message);
                string ss = ex.Message;
                string[] parts = ss.Split('\''); // Split by single quote
                string port = parts.Length > 1 ? parts[1] : string.Empty; // Get the second part (COM4)

                richTextBox1.Text += ("\r\n" + port); // Outputs: COM4
                //statusConnection.Text = ("Disconnected" + port + "is denied");
                //infoMessages.Text = ("Error reading: " + "+ port: " + port + ex.Message);
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
       

        #region MyRegion
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
        #endregion
        #region Old
        //private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        //{
        //    SerialPort serialPort = (SerialPort)sender;

        //    try
        //    {
        //        string inData = serialPort.ReadExisting();
        //        if (!string.IsNullOrEmpty(inData))
        //        {
        //            //richTextBox1.Text += inData;
        //            responseBuilder.Append(inData);
        //            //richTextBox1.Text += inData;

        //        }
        //        /*   string[] parsedData = inData.Split(',');
        //           */
        //    }
        //    catch (TimeoutException)
        //    {
        //        // Handle timeout if necessary, e.g., break the loop
        //    }
        //    //Logger.Info("MainParamaterForm/DataReceivedHandler|  canPacket Ack:" + inData);


        //}

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            // InitializeSerialPort();
            richTextBox1.Text = null;
        }

        private void labelA0_Click(object sender, EventArgs e)
        {

        }

        #region try for PM data reading
        public void SendCommandAndDisplayResponse(string command)
        {
            try
            {
                // Open the serial port if it's not already open
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }

                // Send the command
                byte[] commandBytes = Encoding.ASCII.GetBytes(command + "\r\n"); // Adding CRLF
                serialPort.Write(commandBytes, 0, commandBytes.Length);
                //serialPort.Write(":MEAS?");

                richTextBox1.AppendText("Command sent: " + command + Environment.NewLine);

                // Call the ReadCompleteResponse method to get the JSON string
                string response = ReadCompleteResponse();

                if (!string.IsNullOrEmpty(response))
                {
                    richTextBox1.AppendText(response + Environment.NewLine);
                }
                else
                {
                    JIMessageBox.WarningMessage("Response is Empty");
                    return;
                }

                ParseResponse(response);
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText("Error: " + ex.Message + Environment.NewLine);
            }
            finally
            {
                // Close the serial port if it's open
 
                richTextBox1.AppendText("Serial port closed." + Environment.NewLine);
            }
        }

        private string ReadCompleteResponse()
        {
            string jsonResult = null;
            try
            {

                string data = serialPort.ReadExisting();

                if (!string.IsNullOrEmpty(data))
                {
                    // Split the string by commas
                    var dataParts = data.Split(',');

                    // Define a dictionary to hold key-value pairs
                    var dataDictionary = new Dictionary<string, string>();

                    try
                    {
                        dataDictionary["labelV1"] = Conversion.GetValue(dataParts, 4);
                        dataDictionary["labelV2"] = Conversion.GetValue(dataParts, 5);
                        dataDictionary["labelV3"] = Conversion.GetValue(dataParts, 6);
                        dataDictionary["labelV0"] = Conversion.GetValue(dataParts, 7);
                        dataDictionary["labelA1"] = Conversion.GetValue(dataParts, 8);
                        dataDictionary["labelA2"] = Conversion.GetValue(dataParts, 9);
                        dataDictionary["labelA3"] = Conversion.GetValue(dataParts, 10);
                        dataDictionary["labelA0"] = Conversion.GetValue(dataParts, 11);
                        dataDictionary["labelPf1"] = Conversion.GetValue(dataParts, 15);
                        dataDictionary["labelPf2"] = Conversion.GetValue(dataParts, 16);
                        dataDictionary["labelPf3"] = Conversion.GetValue(dataParts, 17);
                        dataDictionary["labelPf0"] = Conversion.GetValue(dataParts, 18);
                        dataDictionary["labelHertz"] = Conversion.GetValue(dataParts, 19);
                        dataDictionary["labelPower1"] = Conversion.GetValue(dataParts, 20);
                        dataDictionary["labelPower2"] = Conversion.GetValue(dataParts, 26);
                        dataDictionary["labelPower3"] = Conversion.GetValue(dataParts, 27);
                        dataDictionary["labelPower0"] = Conversion.GetValue(dataParts, 28);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred while processing data: " + ex.Message);
                    }

                    // Convert the dictionary to JSON
                    jsonResult = JsonConvert.SerializeObject(dataDictionary, Newtonsoft.Json.Formatting.Indented);

                    // Output the JSON
                    Console.WriteLine(jsonResult);
                }
            }
            catch (TimeoutException)
            {
                // Handle timeout
                richTextBox1.AppendText("Timeout occurred while reading response." + Environment.NewLine);
            }
            return jsonResult;
        }

        private void ParseResponse(string data)
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
                labelPf1 = dataParts.ElementAtOrDefault(15) ?? "N/A",
                labelPf2 = dataParts.ElementAtOrDefault(16) ?? "N/A",
                labelPf3 = dataParts.ElementAtOrDefault(17) ?? "N/A",
                labelPf0 = dataParts.ElementAtOrDefault(18) ?? "N/A",
                labelHertz = dataParts.ElementAtOrDefault(19) ?? "N/A",
                labelPower1 = dataParts.ElementAtOrDefault(20) ?? "N/A",
                labelPower2 = dataParts.ElementAtOrDefault(26) ?? "N/A",
                labelPower3 = dataParts.ElementAtOrDefault(27) ?? "N/A",
                labelPower0 = dataParts.ElementAtOrDefault(28) ?? "N/A",


            };
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

            // Close the serial port if it is open

            //if (serialPort.IsOpen)
            //{
            //    serialPort.Close();
            //    richTextBox1.Invoke((MethodInvoker)delegate
            //    {
            //        richTextBox1.AppendText("Serial port closed." + Environment.NewLine);
            //    });
            //}
        }

        // Method to update UI labels
        private void UpdateUI(DashboardModel.Manupulation response)
        {
            labelV1.Text = response.labelV1;
            labelV2.Text = response.labelV2;
            labelV3.Text = response.labelV3;
            labelV0.Text = response.labelV0;
            labelA1.Text = response.labelA1;
            labelA2.Text = response.labelA2;
            labelA3.Text = response.labelA3;
            labelA0.Text = response.labelA0;
            labelPf1.Text = response.labelPf1;
            labelPf2.Text = response.labelPf2;
            labelPf3.Text = response.labelPf3;
            labelPf0.Text = response.labelPf0;
            labelHertz.Text = response.labelHertz;
            labelPower1.Text = response.labelPower1;
            labelPower2.Text = response.labelPower2;
            labelPower3.Text = response.labelPower3;
            labelPower0.Text = response.labelPower0;
        }

        private void AppendTextToRichTextBox(string text)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    richTextBox1.AppendText(text + Environment.NewLine);
                });
            }
            else
            {
                richTextBox1.AppendText(text + Environment.NewLine);
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

        private void btnStartReadng_Click(object sender, EventArgs e)
        {

        }
    }


}


