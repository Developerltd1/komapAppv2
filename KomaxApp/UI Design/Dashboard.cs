using EasyModbus;
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
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Utility;
using System.Threading;
using static KomaxApp.Model.Dashboard.DashboardModel.Manupulation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace KomaxApp.UI_Design
{
    public partial class Dashboard : BaseForm//Form
    {
        private ParentForm parentForm;



        private BackgroundWorker backgroundWorker;

        private bool isPolling;
        private System.Windows.Forms.Timer minuteTimer;
        private System.Windows.Forms.Timer pollingTimer;
        private ModbusClient modbusClient;
        int[] registers = { 0 };
        private SerialPort serialPort;
        private bool isPollingEnabled = false;
        public static bool isPollSelected = false;

        public string _powerMeter;
        public string _torqueMeter;
        public string _rpm;
        public string _temperature;
        // Dictionary to store SerialPort objects for each COM port
        private Dictionary<string, SerialPort> serialPorts = new Dictionary<string, SerialPort>();

        public Dashboard(string ReportNo, string powerMeter, string torqueMeter, string rpm, string temperature, ParentForm parent)
        {
            InitializeComponent();

            // Store the configuration values
            _powerMeter = powerMeter;
            _torqueMeter = torqueMeter;
            _rpm = rpm;
            _temperature = temperature;

            parentForm = parent;

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
        private System.Windows.Forms.Timer periodicTimer;  // Timer for periodic execution
        private void StartBackgroundWorkerButton_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, SerialPort> openPorts = parentForm.GetAllOpenSerialPorts();
                if (openPorts.Count() <= 0)
                {
                    errorMesageEx("Ports are not Open", null);
                    return;
                }
                btnStartReadng.BackColor = System.Drawing.Color.FromArgb(38, 166, 66);
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
            }
            catch (Exception ex)
            {
                infoMessages.Text = ex.Message;
            }


        }
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
                                double temp1 = LoadModbusData(comPort, 1);
                                serialResponse._serialResponseCOM7Temp1 = temp1.ToString();
                                double temp2 = LoadModbusData(comPort, 2);
                                serialResponse._serialResponseCOM7Temp2 = temp2.ToString();
                                portInitialized = true;
                                break;
                            default:
                                errorMesageEx("No Ports Initialized", null);
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
                errorMesageEx("PollingTimer error: ", ex);
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
                errorMesageEx("Progress: ", ex);
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
        private string InitializeSerialPort(Dictionary<string, SerialPort> openPorts,string comPort, string command)
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
                            System.Threading.Thread.Sleep(400);
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
        private async Task<string> InitializeSerialPortAsync(string comPort, string command)
        {
            string serialResponse = null;
            SerialPort serialPort;

            try
            {
                // Check if the serial port is already initialized
                if (!serialPorts.ContainsKey(comPort))
                {
                    serialPort = new SerialPort()
                    {
                        PortName = comPort,
                        BaudRate = 9600,
                        Parity = Parity.None,
                        DataBits = 8,
                        StopBits = StopBits.One,
                        Handshake = Handshake.None,
                        ReadTimeout = 5000  // Set an optional timeout
                    };

                    serialPorts[comPort] = serialPort;
                }
                else
                {
                    serialPort = serialPorts[comPort];
                }

                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }

                var PortName = serialPort.PortName;

                // Use Task.Run to offload the synchronous serial port operations
                switch (PortName)
                {
                    case "COM4":
                    case "COM5":
                        {
                            byte[] commandBytes = new GenericCode.SerialPortManager().HexStringToByteArray(command);
                            serialPort.Write(commandBytes, 0, commandBytes.Length); // dynamic
                            await Task.Delay(100); // Non-blocking delay
                            serialResponse = serialPort.ReadExisting();
                            break;
                        }
                    case "COM6":
                        {
                            byte[] commandBytes = Encoding.ASCII.GetBytes(command + "\r\n");  // Add CRLF
                            await Task.Delay(100); // Non-blocking delay
                            serialPort.Write(commandBytes, 0, commandBytes.Length); // dynamic
                            serialResponse = serialPort.ReadLine();
                            break;
                        }
                    case "COM7":
                        {
                            byte[] commandBytes = Encoding.ASCII.GetBytes(command + "\r\n");  // Add CRLF
                            await Task.Delay(100); // Non-blocking delay
                            serialPort.Write(commandBytes, 0, commandBytes.Length); // dynamic
                            serialResponse = serialPort.ReadExisting();
                            break;
                        }
                    default:
                        return string.Empty; // Return empty string if no data is received
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







        private void ParseResponse(DashboardModel.SerialResponseModel data)
        {
            DashboardModel.Manupulation returnModel = new DashboardModel.Manupulation();
            if (!string.IsNullOrEmpty(data._serialResponseCOM4))
            {
                string cleanedData = Regex.Replace(data._serialResponseCOM4, @".*?(-\d+\.\d+\.\d+).*", "$1").Trim();  //CleanExtraCharacter
                var dataParts = cleanedData.Split(',');    // Split the string by commas
                returnModel._tbSpeedRPM = dataParts.ElementAtOrDefault(0) ?? "N/A";

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
                #region Default
                //this.Invoke((MethodInvoker)delegate
                //      {
                //          labelV1.Text = returnModel.labelV1;
                //          labelV2.Text = returnModel.labelV2;
                //          labelV3.Text = returnModel.labelV3;
                //          labelV0.Text = returnModel.labelV0;
                //          labelA1.Text = returnModel.labelA1;
                //          labelA2.Text = returnModel.labelA2;
                //          labelA3.Text = returnModel.labelA3;
                //          labelA0.Text = returnModel.labelA0;
                //          labelPf1.Text = returnModel.labelPf1;
                //          labelPf2.Text = returnModel.labelPf2;
                //          labelPf3.Text = returnModel.labelPf3;
                //          labelPf0.Text = returnModel.labelPf0;
                //          labelHertz.Text = returnModel.labelHertz;
                //          labelPower1.Text = returnModel.labelPower1;
                //          labelPower2.Text = returnModel.labelPower2;
                //          labelPower3.Text = returnModel.labelPower3;
                //          labelPower0.Text = returnModel.labelPower0;
                //          tbTorqueNm.Text = returnModel._tbTorqueNm;
                //          tbSpeedRPM.Text = returnModel._tbSpeedRPM;
                //          tbTemp1.Text = returnModel._tbserialResponseCOM7Temp1;
                //          tbTemp2.Text = returnModel.__tbserialResponseCOM7Temp2;
                //      }); 
                #endregion
                #region Default
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
                          tbTorqueNm.Text = returnModel._tbTorqueNm;
                          tbSpeedRPM.Text = returnModel._tbSpeedRPM;
                          tbTemp1.Text = returnModel._tbserialResponseCOM7Temp1;
                          tbTemp2.Text = returnModel.__tbserialResponseCOM7Temp2;
                          tbShaftPawerKw.Text = (tbTorqueNm.Text.ToDoble() * 0.00010472).ToString();
                          tbLoadingFactorPercentage.Text = (tbShaftPawerKw.Text.ToDoble() * 100 / (textBoxMotorSizeHP.Text.ToDoble() * 0.746)).ToString();
                          textBoxEstimitedEfficency.Text = "0";//(textBoxShaftPawerKw.Text.ToDoble() * 100 / labelPower0.Text.ToDoble()+).ToString();
                      });
                #endregion
            }
            catch (Exception ex)
            {
            }
            // Convert the class to JSON
            //string jsonResult = JsonConvert.SerializeObject(fromModel, Newtonsoft.Json.Formatting.Indented);
            //var deserializedResponse = JsonConvert.DeserializeObject<DashboardModel.Manupulation>(jsonResult);
        }

        #endregion

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
        private async void buttonScreenshot_Click_1(object sender, EventArgs e)
        {
            await CaptureMdiChildForm(this);
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
        public async Task CaptureWindow(Form form)
        {
            await Task.Delay(100);

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
        public async Task CaptureMdiChildForm(Form mdiChildForm)
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
                errorMesageEx("Error reading: " + "+ port: " + port, ex);
            }
        }

        public double LoadModbusData(string PortName, Int32 slaveId)
        {
            double temp = 0;
            try
            {
                InitializeModbusClient(PortName, slaveId);
                System.Threading.Thread.Sleep(100);
                int[] registerValuefrmSensor = modbusClient.ReadInputRegisters(1000, slaveId);    //uncomment
                temp = registerValuefrmSensor[0]; //12;
            }
            catch (Exception ex)
            {
                modbusClient.Disconnect();
                isModbusClientConnected = false;
                errorMesageEx("LoadModbusData: ",ex);
            }
            finally
            {
                modbusClient.Disconnect();
                isModbusClientConnected = false;
            }
            return temp;
        }
        public async Task<double> LoadModbusDataAsync(string PortName, Int32 slaveId)
        {
            double temp = 0;
            try
            {
                await Task.Delay(100); // Non-blocking delay
                InitializeModbusClient(PortName, slaveId);
                await Task.Delay(100); // Non-blocking delay
                int[] registerValuefrmSensor = modbusClient.ReadInputRegisters(1000, 1);    //uncomment
                temp = registerValuefrmSensor[0];
                //infoMessages.Text = ("Reading Successful");


            }
            catch (Exception ex)
            {
                modbusClient.Disconnect();
                isModbusClientConnected = false;
                JIMessageBox.ErrorMessage(ex.Message);
            }

            finally
            {
                modbusClient.Disconnect();
                isModbusClientConnected = false;
            }
            return temp;
        }

        #endregion

        private void btnStopReading_Click(object sender, EventArgs e)
        {
            try
            {


                isPollSelected = false;
                btnStartReadng.BackColor = System.Drawing.Color.Black;
                btnStartReadng.BackColor = System.Drawing.Color.FromArgb(38, 166, 99);
                if (periodicTimer != null)
                    periodicTimer.Stop();
                
                if (modbusClient != null)
                {
                    if (modbusClient.Connected)
                    {
                        modbusClient.Disconnect();
                    }
                }
                if (serialPort != null)
                {
                    if (serialPort.IsOpen)
                    {
                        serialPort.Close();
                        serialPort.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    erroMessage.Text = "DoWork: " + ex.Message;
                });
            }
        }


        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }

        private void Dashboard_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Dashboard_FormClosing_1(object sender, FormClosingEventArgs e)
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
        private void errorMesageEx(string _msg, Exception ex)
        {
            if (erroMessage.InvokeRequired)
            {
                erroMessage.Invoke((MethodInvoker)delegate {

                    if(ex == null) 
                    erroMessage.Text = _msg;  // Safely update the control
                    else
                     erroMessage.Text = _msg + ex.Message;
                });
            }
            else
            {
                if (ex == null)
                    erroMessage.Text = _msg;   // Update directly if already on the UI thread
                else
                    erroMessage.Text = _msg + ex.Message;
            }
        }
    }


}


