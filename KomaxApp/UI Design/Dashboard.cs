﻿using EasyModbus;
using komaxApp.BusinessLayer;
using komaxApp.Utility.ExtensionMethod;
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

        //private Dictionary<string, SerialPort> serialPorts = new Dictionary<string, SerialPort>
        //{
        //      //  { "COM6", new SerialPort("COM6", 9600, Parity.None, 8, StopBits.One) },
        //        { "COM4", new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One) }
        //        //{ "COM5", new SerialPort("COM5", 9600, Parity.None, 8, StopBits.One) },
        //        //{ "COM3", new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One) }
        //};


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
                    pollingTimer.Start();
                }
                else
                {
                    isPollSelected = false;
                    btnStartReadng.BackColor = System.Drawing.Color.Black;
                    pollingTimer.Stop();
                }
            }
            catch (Exception ex)
            {
                JIMessageBox.ErrorMessage(ex.Message);
            }
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
                pollingTimer.Stop();
                JIMessageBox.WarningMessage("COM Ports are not Configure");
                return;
            }
            #region Data Reading
            try
            {
                //List<string> comPorts = new List<string>  //dynamic
                //    {
                //        _powerMeter,_torqueMeter,_rpm,_temperature,
                //    };
                List<string> comPorts = new List<string>
                    {
                        "COM4"
                    };
                List<string> commands = new List<string>
                {
                    //":MEAS?",                          // Command for _powerMeter (COM6)
                    //"0x23, 0x30, 0x30, 0x30, 0x0d",    // Command for _torqueMeter (COM5)
                    "x05 x01 x00 x00 x00 x00 x06 xAA" // Command for _rpm (COM4)
                   // null                               // No command for _temperature (COM7)
                };

                DashboardModel.SerialResponseModel serialResponse = new DashboardModel.SerialResponseModel();

                for (int i = 0; i < comPorts.Count; i++)
                {
                    string comPort = comPorts[i];
                    string command = commands[i]; // Corresponding command for the port
                    switch (comPort)
                    {
                        case "COM4":
                            serialResponse._serialResponseCOM4 = InitializeSerialPort(comPort, command);
                            break;

                        case "COM5":
                            serialResponse._serialResponseCOM5 = InitializeSerialPort(comPort, command);
                            break;
                        case "COM6":
                            serialResponse._serialResponseCOM6 = InitializeSerialPort(comPort, command);
                            break;
                        case "COM7":
                            serialResponse._serialResponseCOM7 = InitializeSerialPort(comPort, command);
                            break;
                        default:
                            JIMessageBox.WarningMessage("No Ports Initlized");
                            return;
                    }
                }


                ParseResponse(serialResponse);  // You can call ParseResponse to handle the data




            }
            catch (Exception ex)
            {
                MessageBox.Show("PollingTimer error: " + ex.Message);
            }
            #endregion
        }

        #region InitilizeSerialPortNew
        private string InitializeSerialPort(string comPort, string command)
        {
            string serialResponse = null;
            try
            {
                // Check if the serial port is already initialized
                if (!serialPorts.ContainsKey(comPort))
                {
                    SerialPort serialPort = new SerialPort
                    {
                        PortName = comPort,
                        BaudRate = 9600,
                        Parity = Parity.None,
                        DataBits = 8,
                        StopBits = StopBits.One,
                        Handshake = Handshake.None,
                        //ReadTimeout = 90000  // Set an optional timeout
                    };
                }
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                    serialPorts[comPort] = serialPort; // Add the port to the dictionary
                }
                else  //already open
                {
                    serialPort = serialPorts[comPort];
                }


                var PortName = serialPort.PortName;
                switch (command)  //COnversion
                {
                    case "COM4":
                        byte[] commandBytes4 = HexStringToByteArray(command);
                        serialPort.Write(commandBytes4, 0, commandBytes4.Length); // dynamic
                        serialResponse = serialPort.ReadExisting();
                        break;

                    case "COM6":
                            byte[] commandBytes6 = HexStringToByteArray(command);
                            serialPort.Write(commandBytes6, 0, commandBytes6.Length); // dynamic
                            serialResponse = serialPort.ReadExisting();
                        break;

                    case "COM5":
                        if (!string.IsNullOrEmpty(serialResponse))
                        {
                            return serialResponse; // Return if there's a response for COM5
                        }
                        break;

                    default:
                        richTextBox1.AppendText("No data received." + Environment.NewLine);
                        return string.Empty; // Return empty string if no data is received
                }

                if (!string.IsNullOrEmpty(command) && command == "COM4")
                {
                    return serialResponse;
                }

                if (!string.IsNullOrEmpty(serialResponse) && command == "COM5")
                {
                    return serialResponse;
                }
                if (!string.IsNullOrEmpty(serialResponse) && command == "COM6")
                {
                    return serialResponse;
                }
                else
                {
                    richTextBox1.AppendText("No data received." + Environment.NewLine);
                    return null;
                }

            }
            catch (Exception ex)
            {
                labelInfo.Text = $"Error initializing serial port {comPort}: {ex.Message}" + Environment.NewLine;
                labelInfo.ForeColor = Color.Red;
                if (serialPorts.ContainsKey(comPort))
                {
                    serialPorts[comPort].Close();
                    serialPorts.Remove(comPort);
                }
            }
            finally
            {
                CloseSerialPort(comPort);
            }
            return serialResponse;
        }
        private byte[] HexStringToByteArray(string hex)
        {
            // Remove spaces and convert to upper case
            hex = hex.Replace(" ", "").Replace("x", "").ToUpper();
            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException("Invalid length of hex string.");
            }

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        #endregion
        private void ParseResponse(DashboardModel.SerialResponseModel data)
        {
            DashboardModel.Manupulation returnModel = new DashboardModel.Manupulation();
            if (!string.IsNullOrEmpty(data._serialResponseCOM4))
            {
                string cleanedData = Regex.Replace(data._serialResponseCOM4, @"\+|E\+\d{2}|E[\+\d]+", "").Trim();  //CleanExtraCharacter
                var dataParts = cleanedData.Split(',');    // Split the string by commas
                returnModel._tbTorqueNm = dataParts.ElementAtOrDefault(4) ?? "N/A";

            }
            if (!string.IsNullOrEmpty(data._serialResponseCOM5))
            {
                string cleanedData = Regex.Replace(data._serialResponseCOM4, @"\+|E\+\d{2}|E[\+\d]+", "").Trim();  //CleanExtraCharacter
                var dataParts = cleanedData.Split(',');    // Split the string by commas
                returnModel._tbSpeedRPM = dataParts.ElementAtOrDefault(4) ?? "N/A";
            }
            if (!string.IsNullOrEmpty(data._serialResponseCOM6))
            {
                string cleanedData = Regex.Replace(data._serialResponseCOM6, @"\+|E\+\d{2}|E[\+\d]+", "").Trim();  //CleanExtraCharacter
                var dataParts = cleanedData.Split(',');    // Split the string by commas

                    returnModel.labelV1 = dataParts.ElementAtOrDefault(4) ?? "N/A";
                    returnModel.labelV2 = dataParts.ElementAtOrDefault(5) ?? "N/A";
                    returnModel.labelV3 = dataParts.ElementAtOrDefault(6) ?? "N/A";
                    returnModel.labelV0 = dataParts.ElementAtOrDefault(7) ?? "N/A";
                returnModel.labelA1 = dataParts.ElementAtOrDefault(8) ?? "N/A";
                    returnModel.labelA2 = dataParts.ElementAtOrDefault(9) ?? "N/A";
                    returnModel.labelA3 = dataParts.ElementAtOrDefault(10) ?? "N/A";
                returnModel.labelA0 = dataParts.ElementAtOrDefault(11) ?? "N/A";
                returnModel.labelPf1 = dataParts.ElementAtOrDefault(15) ?? "N/A";
                    returnModel.labelPf2 = dataParts.ElementAtOrDefault(16) ?? "N/A";
                    returnModel.labelPf3 = dataParts.ElementAtOrDefault(17) ?? "N/A";
                    returnModel.labelPf0 = dataParts.ElementAtOrDefault(18) ?? "N/A";
                returnModel.labelHertz = dataParts.ElementAtOrDefault(19) ?? "N/A";
                    returnModel.labelPower1 = dataParts.ElementAtOrDefault(20) ?? "N/A";
                    returnModel.labelPower2 = dataParts.ElementAtOrDefault(26) ?? "N/A";
                    returnModel.labelPower3 = dataParts.ElementAtOrDefault(27) ?? "N/A";
                returnModel.labelPower0 = dataParts.ElementAtOrDefault(28) ?? "N/A";
            }
            if (!string.IsNullOrEmpty(data._serialResponseCOM7))
            {
                string cleanedData = Regex.Replace(data._serialResponseCOM4, @"\+|E\+\d{2}|E[\+\d]+", "").Trim();  //CleanExtraCharacter
                var dataParts = cleanedData.Split(',');    // Split the string by commas
                returnModel._tbShaftPawerKw = dataParts.ElementAtOrDefault(4) ?? "N/A";
            }


            //UI
            try
            {
                // Update the labels on the UI thread
                this.Invoke((MethodInvoker)delegate
                {
                    labelV1.Text = returnModel.labelV1;
                    labelV2.Text = returnModel.labelV2;
                    labelV3.Text = returnModel.labelV3;
                    labelV0.Text = returnModel.labelV0;
                    labelA1.Text = returnModel.labelA1;
                    labelA2.Text = returnModel.labelA2;
                    labelA3.Text = returnModel.labelA3;
                    labelA0.Text = returnModel.labelA0;
                    labelPf1.Text = returnModel.labelPf1;
                    labelPf2.Text = returnModel.labelPf2;
                    labelPf3.Text = returnModel.labelPf3;
                    labelPf0.Text = returnModel.labelPf0;
                    labelHertz.Text = returnModel.labelHertz;
                    labelPower1.Text = returnModel.labelPower1;
                    labelPower2.Text = returnModel.labelPower2;
                    labelPower3.Text = returnModel.labelPower3;
                    labelPower0.Text = returnModel.labelPower0;
                });
            }
            catch (Exception ex)
            {
            }
            // Convert the class to JSON
            //string jsonResult = JsonConvert.SerializeObject(fromModel, Newtonsoft.Json.Formatting.Indented);
            //var deserializedResponse = JsonConvert.DeserializeObject<DashboardModel.Manupulation>(jsonResult);
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



     

       
       
        #region Screenshot
        private void buttonScreenshot_Click_1(object sender, EventArgs e)
        {
            CaptureMdiChildForm(this);
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            CloseAllSerialPorts();  // Close all ports when the form is closing
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

        #endregion



    }


}


