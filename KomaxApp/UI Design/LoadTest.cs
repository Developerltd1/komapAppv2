﻿using komaxApp.BusinessLayer;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;

namespace KomaxApp.UI_Design
{
    public partial class LoadTest : BaseForm
    {
        private SerialPortManager serialPortManager;
        private Timer pollingTimer;
        public string _powerMeter;
        public string _torqueMeter;
        public string _rpm;
        public string _temperature;
        private bool isPollingEnabled = false;
        public static bool isPollSelected = false;
        private Dictionary<string, SerialPort> serialPorts = new Dictionary<string, SerialPort>();

        private string ReportNo;

        private void LoadTest_Load(object sender, EventArgs e)
        {
            try
            {
                //isPollingEnabled = !isPollingEnabled;
                //if (isPollingEnabled)
                //{
                    isPollSelected = true;
                    InitializePollingTimer();
                    StartPolling();
                //}
                //else
                //{
                //    isPollSelected = false;
                //    StopPolling();
                //}
            }
            catch (Exception ex)
            {
                JIMessageBox.ErrorMessage(ex.Message);
            }
        }

        public LoadTest(string ReportNo, string powerMeter, string torqueMeter, string rpm, string temperature)
        {
            // Store the configuration values
            _powerMeter = powerMeter;
            _torqueMeter = torqueMeter;
            _rpm = rpm;
            _temperature = temperature;

            InitializeComponent();
            this.ReportNo = ReportNo;
            LoadData();


            this.Load += LoadTest_Load;

           
        }

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
                        labelInfo.Text = $"Error opening serial port {comPort}: {ex.Message}" + Environment.NewLine;
                        labelInfo.ForeColor = System.Drawing.Color.Red;
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
                labelInfo.Text = $"Error initializing serial port {comPort}: {ex.Message}" + Environment.NewLine;
                labelInfo.ForeColor = System.Drawing.Color.Red;

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

        #endregion





        #region PoolingCode
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
                List<string> comPorts = new List<string>
                    {
                        _powerMeter,_torqueMeter,_rpm,_temperature,
                    };
                InitializeMultipleSerialPorts(comPorts);


                //InitializeSerialPort(_powerMeter);
                //InitializeSerialPort(_torqueMeter);
                //InitializeSerialPort(_rpm);
                //InitializeSerialPort(_temperature);
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
                pollingTimer = new Timer();
                pollingTimer.Interval = 1000;
                pollingTimer.Tick += PollingTimer_Tick;
            }
            catch (Exception ex)
            {
                Utility.JIMessageBox.ErrorMessage(ex.Message);
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
                //labelInfo.Text = "StopPolling Time: " + ex.Message;
                //labelInfo.ForeColor = Color.Red;
            }
        }
        private void StartPolling()
        {
            pollingTimer.Start();
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
                Invoke(new Action(() => richTextBox1.AppendText(errorMessage + Environment.NewLine)));
            }
            else
            {
                richTextBox1.AppendText(errorMessage + Environment.NewLine);
            }
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
                        Display display = new Display(null,null,null,null);
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


    }
}
