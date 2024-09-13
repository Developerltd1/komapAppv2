using EasyModbus;
using komaxApp.BusinessLayer;
using komaxApp.Utility.ExtensionMethod;
using KomaxApp.GenericCode;
using KomaxApp.Model;
using KomaxApp.Model.Dashboard;
using KomaxApp.Model.LoadTest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;

namespace KomaxApp.UI_Design
{
    public partial class LoadTest : BaseForm
    {
        private ParentForm parentForm;

        private BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Timer periodicTimer;  // Timer for periodic execution

        private SerialPortManager serialPortManager;
        public string _powerMeter;
        public string _torqueMeter;
        public string _rpm;
        public string _temperature;
        private Dictionary<string, SerialPort> serialPorts = new Dictionary<string, SerialPort>();

        private ModbusClient modbusClient;

        private string ReportNo;
        private void LoadTest_Load(object sender, EventArgs e)
        {
            try
            {
                InitializePollingTimer();
            }
            catch (Exception ex)
            {
                errorMesageEx("LoadTest_Load: ", ex);
            }
        }

        public LoadTest(string ReportNo, string powerMeter, string torqueMeter, string rpm, string temperature , ParentForm _parentForm)
        {
            // Store the configuration values
            _powerMeter = powerMeter;
            _torqueMeter = torqueMeter;
            _rpm = rpm;
            _temperature = temperature;
            parentForm = _parentForm;
            InitializeComponent();
            this.ReportNo = ReportNo;
            LoadData();


            this.Load += LoadTest_Load;

          


            // Initialize BackgroundWorker
            backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true, // Enable progress reporting
                WorkerSupportsCancellation = true // Enable cancellation
            };

            // Handle the DoWork event to perform background operation
            backgroundWorker.DoWork += BackgroundWorker_DoWork;

            // Handle the ProgressChanged event to update progress
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;

            // Handle the RunWorkerCompleted event to handle completion
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

        }

        #region BackgroudWorker
        
        private void PeriodicTimer_Tick(object sender, EventArgs e)
        {
            // Check if the background worker is not busy before starting a new task
            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            #region Data Reading
            try
            {

                // Get all open serial ports
                Dictionary<string, SerialPort> openPorts = parentForm.GetAllOpenSerialPorts();

                DashboardModel.SerialResponseModel serialResponse = new DashboardModel.SerialResponseModel();
                bool portInitialized = false;

                foreach (var portEntry in openPorts)
                {
                    string comPort = portEntry.Key;
                    //backgroundWorker.ReportProgress(0, new { Port = comPort});

                    switch (comPort)
                    {
                        case "COM4":
                            serialResponse._serialResponseCOM4 = InitializeSerialPort(openPorts, comPort, Model.PortsAndCommands.COM4_x05x01x00x00x00x00x06xAA);
                            portInitialized = true;
                            break;
                        case "COM5":
                            serialResponse._serialResponseCOM5 = InitializeSerialPort(openPorts, comPort, Model.PortsAndCommands.COM5_x23x30x30x30x0d);
                            portInitialized = true;

                            break;
                        case "COM6":
                            serialResponse._serialResponseCOM6 = InitializeSerialPort(openPorts, comPort, Model.PortsAndCommands.COM6_MEAS);
                            portInitialized = true;
                            break;
                        case "COM7":
                            //double temp1 = LoadModbusData(comPort, 1);
                            //serialResponse._serialResponseCOM7Temp1 = temp1.ToString();
                            //double temp2 = LoadModbusData(comPort, 2);
                            //serialResponse._serialResponseCOM7Temp2 = temp2.ToString();
                            //portInitialized = true;
                            break;
                        default:
                            JIMessageBox.WarningMessage("No Ports Initialized");
                            return;
                    }
                }
                if (portInitialized)
                {
                    e.Result = serialResponse; // Store the result in the Result property
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("PollingTimer error: " + ex.Message);
            }
            #endregion


            // Perform the long-running operation here
            if (backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }


            // Simulate a long-running task
            Thread.Sleep(50);
        }

        private void errorMesageEx(string _msg, Exception ex)
        {
            if (erroMessage.InvokeRequired)
            {
                erroMessage.Invoke((MethodInvoker)delegate {
                    erroMessage.Text = _msg + ex.Message;  // Safely update the control
                });
            }
            else
            {
                erroMessage.Text = _msg + ex.Message; ;  // Update directly if already on the UI thread
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {


                // Use Invoke to ensure that the code runs on the UI thread
                this.Invoke((MethodInvoker)delegate
                {
                    // Extract information from the user state
                    var progressInfo = e.UserState as dynamic;
                    if (progressInfo != null)
                    {
                        string comPort = progressInfo.Port;
                        string command = progressInfo.Command;

                        // Update the UI with progress information
                        infoMessages.Text = $"Processing {comPort} with command: {command}";
                        // Or update other UI elements if needed
                    }
                });
            }
            catch (Exception ex)
            {
                errorMesageEx("Progress: " , ex);
            }

        }
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {


                if (e.Cancelled)
                {
                    //MessageBox.Show("Operation was cancelled.");
                    errorMesageEx("Operation was cancelled.: ", null);
                }
                else if (e.Error != null)
                {
                   // MessageBox.Show("An error occurred: " + e.Error.Message);
                    errorMesageEx("An error occurred: ", null);
                }
                else
                {
                    if (e.Result is DashboardModel.SerialResponseModel serialResponse)
                    {
                        // Handle the data after initialization
                        ParseResponse(serialResponse);
                    }
                    else if (e.Result is Exception ex)
                    {
                        errorMesageEx("An error occurred during background work:: ", ex);
                    }
                    else
                    {
                        errorMesageEx("Operation completed with unexpected result. ", null);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMesageEx("DoWork: ", ex);
            }
        }

        #endregion



        #region InitilizeSerialPortNew
        // Method to initialize a single serial port
        private void InitializeSerialPort(string comPort)
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
                    }
                    catch (Exception ex)
                    {
                        errorMesageEx("InitializeSerialPort: ", ex);
                        return;  // Exit if the port couldn't be opened
                    }

                    // Set up the DataReceived event handler and send an initial command
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    byte[] commandBytes = Encoding.ASCII.GetBytes(":MEAS?" + "\r\n");  // Add CRLF
                    serialPort.Write(commandBytes, 0, commandBytes.Length);

                    // Add the initialized port to the dictionary
                    serialPorts[comPort] = serialPort;
                }
            }
            catch (Exception ex)
            {
                errorMesageEx("InitializeSerialPort-: ", ex);
                // Close and remove the port from the dictionary if initialization fails
                if (serialPorts.ContainsKey(comPort))
                {
                    serialPorts[comPort].Close();
                    //serialPorts.Remove(comPort);
                }
            }
        }


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
                    if (infoMessages.InvokeRequired)
                    {
                        // Create a delegate to handle the invocation
                        infoMessages.Invoke(new Action(() =>
                        {
                            infoMessages.AppendText(inData);
                        }));
                    }
                    else
                    {
                        // Directly update the control if on the UI thread
                        infoMessages.AppendText(inData);
                    }
                    ParseResponse(inData);  // You can call ParseResponse to handle the data
                }
            }
            catch (Exception ex)
            {
                errorMesageEx("InitializeSerialPort-: ", ex);
            }
            finally
            {
                string comPort = ((SerialPort)sender).PortName;
                CloseSerialPort1(comPort);
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

        #endregion





        #region PoolingCode
        public void PollingTimer_Tick(object sender, EventArgs e)
        {

            #region Data Reading
            try
            {

                // Get all open serial ports
                Dictionary<string, SerialPort> openPorts = parentForm.GetAllOpenSerialPorts();

                DashboardModel.SerialResponseModel serialResponse = new DashboardModel.SerialResponseModel();
                bool portInitialized = false;

                foreach (var portEntry in openPorts)
                {
                    string comPort = portEntry.Key;
                    //backgroundWorker.ReportProgress(0, new { Port = comPort});

                    switch (comPort)
                    {
                        case "COM4":
                            serialResponse._serialResponseCOM4 = InitializeSerialPort(openPorts, comPort, Model.PortsAndCommands.COM4_x05x01x00x00x00x00x06xAA);
                            portInitialized = true;
                            break;
                        case "COM5":
                            serialResponse._serialResponseCOM5 = InitializeSerialPort(openPorts, comPort, Model.PortsAndCommands.COM5_x23x30x30x30x0d);
                            portInitialized = true;

                            break;
                        case "COM6":
                            serialResponse._serialResponseCOM6 = InitializeSerialPort(openPorts, comPort, Model.PortsAndCommands.COM6_MEAS);
                            portInitialized = true;
                            break;
                        case "COM7":
                            //double temp1 = LoadModbusData(comPort, 1);
                            //serialResponse._serialResponseCOM7Temp1 = temp1.ToString();
                            //double temp2 = LoadModbusData(comPort, 2);
                            //serialResponse._serialResponseCOM7Temp2 = temp2.ToString();
                            //portInitialized = true;
                            break;
                        default:
                            JIMessageBox.WarningMessage("No Ports Initialized");
                            return;
                    }
                }
                if (portInitialized)
                {
                    //e.Result = serialResponse; // Store the result in the Result property
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("PollingTimer error: " + ex.Message);
            }
            #endregion

        }


        private void InitializePollingTimer()
        {
            try
            {
                if (_powerMeter == null && _torqueMeter == null && _rpm == null && _temperature == null)
                {
                    periodicTimer.Stop();
                    erroMessage.Text = "COM Ports are not Configure";
                    return;
                }

                // Initialize and start the periodic timer
                if (periodicTimer == null)
                {
                    periodicTimer = new System.Windows.Forms.Timer();
                    periodicTimer.Interval = 1000; // 1 second interval
                    periodicTimer.Tick += PeriodicTimer_Tick;
                    periodicTimer.Start();
                }
                else
                {
                    periodicTimer.Start();  // Ensure the timer is started
                }

                // Start the background operation if not already running
                if (!backgroundWorker.IsBusy)
                {
                    backgroundWorker.RunWorkerAsync();
                }

                // Start the background operation
                if (!backgroundWorker.IsBusy)
                {
                    backgroundWorker.RunWorkerAsync();
                }

            }
            catch (Exception ex)
            {
                errorMesageEx("InitializePollingTimer: ", ex);
            }
        }
     
      

        #region InitilizeSerialPortNew

        #region ForCOM7
        private bool isModbusClientConnected = false;
        private void InitializeModbusClient(string PortName, Int32 slaveId)
        {
            try
            {
                modbusClient = new ModbusClient(PortName)
                {
                    Baudrate = 9600,//Convert.ToInt32(cbBaudRate1.SelectedItem),
                    Parity = System.IO.Ports.Parity.None,
                    StopBits = System.IO.Ports.StopBits.One,
                    UnitIdentifier = Convert.ToByte(slaveId), // Slave ID
                    ConnectionTimeout = 300//Convert.ToInt32(readingTimeOut.Value) // Timeout in milliseconds
                };


                modbusClient.Connect();
                isModbusClientConnected = true;
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
                string[] parts = ss.Split('\''); // Split by single quote
                string port = parts.Length > 1 ? parts[1] : string.Empty; // Get the second part (COM4)

                Console.WriteLine(port); // Outputs: COM4
                errorMesageEx("InitializeModbusClient: ", ex);

            }
        }

        public double LoadModbusData(string PortName, Int32 slaveId)
        {
            double temp = 0;
            try
            {

                InitializeModbusClient(PortName, slaveId);

                int[] registerValuefrmSensor = modbusClient.ReadInputRegisters(1000, 1);    //uncomment
                temp = registerValuefrmSensor[0];
                //infoMessages.Text = ("Reading Successful");


            }
            catch (Exception ex)
            {
                modbusClient.Disconnect();
                isModbusClientConnected = false;
                errorMesageEx("LoadModbusData: ", ex);
            }

            finally
            {
                modbusClient.Disconnect();
                isModbusClientConnected = false;
            }
            return temp;
        }

        #endregion


        private string InitializeSerialPort(Dictionary<string, SerialPort> openPorts, string comPort, string command)
        {
            string serialResponse = null;
            try
            {
                foreach (var portEntry in openPorts)
                {
                    var PortName = portEntry.Key;
                    switch (PortName)//command)  //COnversion
                    {
                        case "COM4":
                            byte[] commandBytes4 = new GenericCode.SerialPortManager().HexStringToByteArray(command);
                            portEntry.Value.Write(commandBytes4, 0, commandBytes4.Length); // dynamic
                            System.Threading.Thread.Sleep(100);
                            serialResponse = portEntry.Value.ReadExisting();  //"\u0005\u0001-2059.50.0000~f2?";//
                            if (!string.IsNullOrEmpty(serialResponse))
                                return serialResponse;
                            else
                                serialResponse = null;
                            break;

                        case "COM6":
                            byte[] commandBytes6 = Encoding.ASCII.GetBytes(command + "\r\n");  // Add CRLF
                            portEntry.Value.Write(commandBytes6, 0, commandBytes6.Length); // dynamic
                            System.Threading.Thread.Sleep(300);
                            serialResponse = portEntry.Value.ReadExisting();//"2024/09/12,20:06:17,00000:00:00,0000000000,+417.21E+00,+419.21E+00,+419.16E+00,+418.53E+00,+000.00E+00,+000.00E+00,+000.00E+00,+000.00E+00,+000.00E+03,+000.00E+03,+000.00E+03,+000000E+99,+50.241E+00,+000.00E+03,+000.00E+03,+000.00E+03,+000.00E+03,+000.00E+03,+000.00E+03,+000.00E+03,+000.00E+03,+000.00E+03,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000.000E+03,-000.000E+03,+000.000E+03,-000.000E+03,+000.000E+03,-000.000E+03,+000.000E+03,-000.000E+03,+000000E+99,+000000E+99,+000000E+99,+000000E+99,----/--/--,--:--:--,+417.11E+00,+000.32E+00,+002.18E+00,+003.75E+00,+000.21E+00,+001.54E+00,+000.66E+00,+001.13E+00,+000.00E+00,+000.00E+00,+000.00E+00,+000.01E+00,+000.00E+00,+000.00E+00,+000.00E+00,+098.42E+00,+000.00E+03,+000.00E+03,-000.00E+03,-000.00E+03,-000.00E+03,-000.00E+03,+000.00E+03\r\n2024/09/12,20:06:17,00000:00:00,0000000000,+417.21E+00,+419.21E+00,+419.16E+00,+418.53E+00,+000.00E+00,+000.00E+00,+000.00E+00,+000.00E+00,+000.00E+03,+000.00E+03,+000.00E+03,+000000E+99,+50.241E+00,+000.00E+03,+000.00E+03,+000.00E+03,+000.00E+03,+000.00E+03,+000.00E+03,+000.00E+03,+000.00E+03,+000.00E+03,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000000E+99,+000.000E+03,-000.000E+03,+000.000E+03,-000.000E+03,+000.000E+03,-000.000E+03,+000.000E+03,-000.000E+03,+000000E+99,+000000E+99,+000000E+99,+000000E+99,----/--/--,--:--:--,+417.11E+00,+000.32E+00,+002.18E+00,+003.75E+00,+000.21E+00,+001.54E+00,+000.66E+00,+001.13E+00,+000.00E+00,+000.00E+00,+000.00E+00,+000.01E+00,+000.00E+00,+000.00E+00,+000.00E+00,+098.42E+00,+000.00E+03,+000.00E+03,-000.00E+03,-000.00E+03,-000.00E+03,-000.00E+03,+000.00E+03\r\n";
                            if (!string.IsNullOrEmpty(serialResponse))
                                return serialResponse;
                            else
                                serialResponse = null;
                            break;

                        case "COM5":

                            byte[] commandBytes5 = new GenericCode.SerialPortManager().HexStringToByteArray(command);
                            portEntry.Value.Write(commandBytes5, 0, commandBytes5.Length); // dynamic
                            System.Threading.Thread.Sleep(100);
                            serialResponse = portEntry.Value.ReadExisting(); // "\">+06.361\\r\"";
                            if (!string.IsNullOrEmpty(serialResponse))
                                return serialResponse;
                            else
                                serialResponse = null;
                            break;

                        //case "COM7":
                        //    //byte[] commandBytes7 = Encoding.ASCII.GetBytes(command + "\r\n");  // Add CRLF
                        //    //serialPort.Write(commandBytes7, 0, commandBytes7.Length); // dynamic
                        //    //System.Threading.Thread.Sleep(100);
                        //    //serialResponse = serialPort.ReadExisting();
                        //    //if (!string.IsNullOrEmpty(serialResponse))
                        //    //    return serialResponse;
                        //    //else
                        //    //    serialResponse = null;
                        //    break;

                        default:
                            return string.Empty; // Return empty string if no data is received
                    }

                }
            }
            catch (Exception ex)
            {
                // Use Invoke to update the UI from the main thread
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        labelInfo.Text = $"Error initializing serial port {comPort}: {ex.Message}" + Environment.NewLine;
                        labelInfo.ForeColor = Color.Red;
                    });
                }
                else
                {
                    labelInfo.Text = $"Error initializing serial port {comPort}: {ex.Message}" + Environment.NewLine;
                    labelInfo.ForeColor = Color.Red;
                }

                if (serialPorts.ContainsKey(comPort))
                {
                    serialPorts[comPort].Close();
                    //serialPorts.Remove(comPort);
                }
            }
            //finally
            //{
            //    CloseSerialPort(comPort);
            //}
            return serialResponse;
        }

        private void ParseResponse(DashboardModel.SerialResponseModel data)
        {
            DashboardModel.Manupulation returnModel = new DashboardModel.Manupulation();
            if (!string.IsNullOrEmpty(data._serialResponseCOM4))
            {
                string cleanedData = Regex.Replace(data._serialResponseCOM4, @".*?(-\d+\.\d+\.\d+).*", "$1").Trim();  //CleanExtraCharacter
                var dataParts = cleanedData.Split(',');    // Split the string by commas
                returnModel._tbSpeedRPM  = dataParts.ElementAtOrDefault(0) ?? "N/A";

            }
            if (!string.IsNullOrEmpty(data._serialResponseCOM5))
            {
                string cleanedData = Regex.Replace(data._serialResponseCOM5, @".*\+(\d+\.\d+)", "$1").Trim();  //CleanExtraCharacter
                var dataParts = cleanedData.Split(',');    // Split the string by commas
                returnModel._tbTorqueNm = dataParts.ElementAtOrDefault(0) ?? "N/A";
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
                returnModel.labelPf1 = dataParts.ElementAtOrDefault(26) ?? "N/A";
                returnModel.labelPf2 = dataParts.ElementAtOrDefault(27) ?? "N/A";
                returnModel.labelPf3 = dataParts.ElementAtOrDefault(28) ?? "N/A";
                returnModel.labelPf0 = dataParts.ElementAtOrDefault(15) ?? "N/A";
                returnModel.labelHertz = dataParts.ElementAtOrDefault(16) ?? "N/A";
                returnModel.labelPower1 = dataParts.ElementAtOrDefault(17) ?? "N/A";
                returnModel.labelPower2 = dataParts.ElementAtOrDefault(18) ?? "N/A";
                returnModel.labelPower3 = dataParts.ElementAtOrDefault(19) ?? "N/A";
                returnModel.labelPower0 = dataParts.ElementAtOrDefault(20) ?? "N/A";
            }
            if (!string.IsNullOrEmpty(data._serialResponseCOM7Temp1))
            {
                string cleanedData = Regex.Replace(data._serialResponseCOM7Temp1, @"\+|E\+\d{2}|E[\+\d]+", "").Trim();  //CleanExtraCharacter
                var dataParts = cleanedData.Split(',');    // Split the string by commas
                returnModel._tbserialResponseCOM7Temp1 = dataParts.ElementAtOrDefault(0) ?? "N/A";
            }
            if (!string.IsNullOrEmpty(data._serialResponseCOM7Temp2))
            {
                string cleanedData = Regex.Replace(data._serialResponseCOM7Temp2, @"\+|E\+\d{2}|E[\+\d]+", "").Trim();  //CleanExtraCharacter
                var dataParts = cleanedData.Split(',');    // Split the string by commas
                returnModel.__tbserialResponseCOM7Temp2 = dataParts.ElementAtOrDefault(0) ?? "N/A";
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
                    textBoxTorqueNm.Text = returnModel._tbTorqueNm;
                    textBoxSpeedRPM.Text = returnModel._tbSpeedRPM;
                    textBoxAmbientTempC.Text = returnModel._tbserialResponseCOM7Temp1;
                    textBoxmotorTempC.Text = returnModel.__tbserialResponseCOM7Temp2;

                    textBoxShaftPawerKw.Text = (textBoxTorqueNm.Text.ToDoble() * 0.00010472).ToString();
                    textBoxLoadingFactor.Text = (textBoxShaftPawerKw.Text.ToDoble() * 100 / (textBoxMotorSizeHP.Text.ToDoble() * 0.746)).ToString();
                    textBoxEstimitedEfficency.Text = "0";//(textBoxShaftPawerKw.Text.ToDoble() * 100 / labelPower0.Text.ToDoble()+).ToString();
                });
            }
            catch (Exception ex)
            {
                errorMesageEx(" ", ex);
            }
            // Convert the class to JSON
            //string jsonResult = JsonConvert.SerializeObject(fromModel, Newtonsoft.Json.Formatting.Indented);
            //var deserializedResponse = JsonConvert.DeserializeObject<DashboardModel.Manupulation>(jsonResult);
        }

        #endregion


        private void CloseSerialPort1(string comPort)
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
        #endregion
        #region SerialPortManagerClass

        private void SerialPortManager_DataReceived(object sender, string data)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ParseResponse(data)));
            }
            else
            {
                ParseResponse(data);
            }
        }

        private void SerialPortManager_ErrorOccurred(object sender, string errorMessage)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => infoMessages.AppendText(errorMessage + Environment.NewLine)));
            }
            else
            {
                infoMessages.AppendText(errorMessage + Environment.NewLine);
            }
        }


        private void ParseResponse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                infoMessages.AppendText("No data received." + Environment.NewLine);
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

              
            }
            catch (Exception ex)
            {
                errorMesageEx(": ", ex);
                
            }

        }




        private void InitializeMultipleSerialPorts(List<string> comPorts)
        {
            InitializeSerialPort(comPorts[0]);
            InitializeSerialPort(comPorts[1]);
            InitializeSerialPort(comPorts[2]);
            InitializeSerialPort(comPorts[3]);
            //foreach (var comPort in comPorts)
            //{
            //    serialPortManager.InitializeSerialPort(comPort);
            //}
        }

        private void CloseAllSerialPorts()
        {
            serialPortManager.CloseSerialPort();
        }

        #endregion
        private void LoadData()
        {
            RequestLoadTestModel.LabelCountModel mdoelCheckLbl = new GetListBL().CheckLoadTestRecordExistBL(ReportNo);
            if (mdoelCheckLbl != null)
            {
                label1Count.Text = mdoelCheckLbl.LabelCount.ToString() + "%";
                label1Count.Visible = true;
            }
        }

        private void textBoxSpeedRPM_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void btnRecordNoLoadPoint_Click(object sender, EventArgs e)
        {
            RequestLoadTestModel.Request testModel = new RequestLoadTestModel.Request();

            #region Actual Value From Sensor
            testModel.ReportNo = Convert.ToInt32(ReportNo);
            testModel.TorqueNm = textBoxTorqueNm.Text.ToDouble();
            testModel.SpeedRPM = textBoxSpeedRPM.Text.ToDouble();
            testModel.ShaftPowerkW = textBoxShaftPawerKw.Text.ToDouble();
            testModel.LoadingFactor = textBoxLoadingFactor.Text.ToDouble();
            testModel.MotorSize = textBoxMotorSizeHP.Text.ToDouble();

            testModel.VoltageV_Value1 = labelV0.Text.ToDouble();
            testModel.VoltageV_Value2 = labelV1.Text.ToDouble();
            testModel.VoltageV_Value3 = labelV2.Text.ToDouble();
            testModel.VoltageV_Value4 = labelV3.Text.ToDouble();
            testModel.CurrentA_Value1 = labelA0.Text.ToDouble();
            testModel.CurrentA_Value2 = labelA1.Text.ToDouble();
            testModel.CurrentA_Value3 = labelA2.Text.ToDouble();
            testModel.CurrentA_Value4 = labelA3.Text.ToDouble();
            testModel.Frequency_Value1 = labelPf0.Text.ToDouble();
            testModel.Frequency_Value2 = labelPf1.Text.ToDouble();
            testModel.Frequency_Value3 = labelPf2.Text.ToDouble();
            testModel.Frequency_Value4 = labelPf3.Text.ToDouble();
            testModel.ActivePower_Value1 = labelPower0.Text.ToDouble();
            testModel.ActivePower_Value2 = labelPower1.Text.ToDouble();
            testModel.ActivePower_Value3 = labelPower2.Text.ToDouble();
            testModel.ActivePower_Value4 = labelPower3.Text.ToDouble();
            testModel.FrequencyHZ = labelHertz.Text.ToDouble();

            testModel.AmbientTemperature = textBoxAmbientTempC.Text.ToDouble();
            testModel.MotorTemperature = textBoxmotorTempC.Text.ToDouble();
            testModel.EstimitedEfficiency = textBoxEstimitedEfficency.Text.ToDouble();

            testModel.Pt100_Temp1 = textBoxAmbientTempC.Text.ToDouble();
            testModel.Pt100_Temp2 = textBoxmotorTempC.Text.ToDouble();
            #endregion
            #region Value From DummyData
            //testModel.ReportNo = Convert.ToInt32(ReportNo);//DummyData.btnRecordNoLoadPoint_Click.GetReportNo();
            //testModel.TorqueNm = DummyData.btnRecordNoLoadPoint_Click.GetTorqueNm();
            //testModel.SpeedRPM = DummyData.btnRecordNoLoadPoint_Click.GetSpeedRPM();
            //testModel.ShaftPowerkW = DummyData.btnRecordNoLoadPoint_Click.GetShaftPowerKw();
            //testModel.LoadingFactor = DummyData.btnRecordNoLoadPoint_Click.GetLoadingFactor();
            //testModel.MotorSize = DummyData.btnRecordNoLoadPoint_Click.GetMotorSize();

            //testModel.VoltageV_Value1 = DummyData.btnRecordNoLoadPoint_Click.GetVoltageV1();
            //testModel.VoltageV_Value2 = DummyData.btnRecordNoLoadPoint_Click.GetVoltageV2();
            //testModel.VoltageV_Value3 = DummyData.btnRecordNoLoadPoint_Click.GetVoltageV3();
            //testModel.VoltageV_Value4 = DummyData.btnRecordNoLoadPoint_Click.GetVoltageV4();
            //testModel.CurrentA_Value1 = DummyData.btnRecordNoLoadPoint_Click.GetCurrentA1();
            //testModel.CurrentA_Value2 = DummyData.btnRecordNoLoadPoint_Click.GetCurrentA2();
            //testModel.CurrentA_Value3 = DummyData.btnRecordNoLoadPoint_Click.GetCurrentA3();
            //testModel.CurrentA_Value4 = DummyData.btnRecordNoLoadPoint_Click.GetCurrentA4();
            //testModel.FrequencyHZ = DummyData.btnRecordNoLoadPoint_Click.GetFrequencyHZ();
            //testModel.Frequency_Value1 = DummyData.btnRecordNoLoadPoint_Click.GetFrequency_Value1();
            //testModel.Frequency_Value2 = DummyData.btnRecordNoLoadPoint_Click.GetFrequency_Value2();
            //testModel.Frequency_Value3 = DummyData.btnRecordNoLoadPoint_Click.GetFrequency_Value3();
            //testModel.Frequency_Value4 = DummyData.btnRecordNoLoadPoint_Click.GetFrequency_Value4();
            //testModel.ActivePower_Value1 = DummyData.btnRecordNoLoadPoint_Click.GetActivePower1();
            //testModel.ActivePower_Value2 = DummyData.btnRecordNoLoadPoint_Click.GetActivePower2();
            //testModel.ActivePower_Value3 = DummyData.btnRecordNoLoadPoint_Click.GetActivePower3();
            //testModel.ActivePower_Value4 = DummyData.btnRecordNoLoadPoint_Click.GetActivePower4();

            //testModel.AmbientTemperature = DummyData.btnRecordNoLoadPoint_Click.GetAmbientTemperature();
            //testModel.MotorTemperature = DummyData.btnRecordNoLoadPoint_Click.GetMotorTemperature();
            //testModel.EstimitedEfficiency = DummyData.btnRecordNoLoadPoint_Click.GetEstimitedEfficiency();
            #endregion
            #region PopUpWindow


            using (var popup = new PopUp(testModel))
            {
                var result = popup.ShowDialog();

                if (result == DialogResult.Yes)
                {

                    RequestLoadTestModel.Response response = new InsertBL().InsertRecordNoLoadPointBL(testModel);
                    if (response.LabelStatus == "Completed")
                    {
                        Display display = new Display(null, null, null, null,null);
                        display.MdiParent = this.MdiParent;
                        display.Dock = DockStyle.Fill;
                        display.Show();
                    }
                    else
                    {

                        label1Count.Visible = true;
                        label1Count.Text = response.LabelCount.ToString() + "%";
                    }
                }
                else if (result == DialogResult.No)
                {

                }
            }
            #endregion
        }


        private void Display_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Disconnect Modbus client if connected
            if (modbusClient != null && isModbusClientConnected)
            {
                modbusClient.Disconnect();
                isModbusClientConnected = false;
            }

            // Stop other ongoing operations (like timers, background workers, etc.)
            periodicTimer?.Stop();

            // Close all serial ports
            foreach (var port in serialPorts.Keys.ToList()) // Use ToList() to avoid modification issues during iteration
            {
                CloseSerialPort(port);
            }
        }

        public byte[] HexStringToByteArray(string hex)
        {
            // Remove spaces and convert to upper case
            hex = hex.Replace(" ", "").Replace("x", "").ToUpper();
            if (hex.Length % 2 != 0)
            {
                errorMesageEx("Invalid length of hex string.", null);
            }

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

    }
}
