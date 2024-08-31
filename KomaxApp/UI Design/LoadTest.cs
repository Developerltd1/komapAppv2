using komaxApp.BusinessLayer;
using komaxApp.Utility.ExtensionMethod;
using KomaxApp.GenericCode;
using KomaxApp.Model;
using KomaxApp.Model.Dashboard;
using KomaxApp.Model.LoadTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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


        private string ReportNo;

        private void LoadTest_Load(object sender, EventArgs e)
        {
            try
            {
                isPollingEnabled = !isPollingEnabled;
                if (isPollingEnabled)
                {
                    isPollSelected = true;
                    InitializePollingTimer();
                    StartPolling();
                }
                else
                {
                    isPollSelected = false;
                    StopPolling();
                }
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

            serialPortManager = new SerialPortManager();
            serialPortManager.DataReceived += SerialPortManager_DataReceived;
            serialPortManager.ErrorOccurred += SerialPortManager_ErrorOccurred;
        }
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
            var dataParts = cleanedData.Split(',');

            var dataResponse = new DashboardModel.Manupulation
            {
                labelV1 = dataParts.ElementAtOrDefault(4) ?? "N/A",
                // Other properties...
            };

            // Update UI
            UpdateLabels(dataResponse);
        }



        private void UpdateLabels(DashboardModel.Manupulation response)
        {
            labelV1.Text = response.labelV1;
            // Update other labels...
        }


        private void InitializeMultipleSerialPorts(List<string> comPorts)
        {
            foreach (var comPort in comPorts)
            {
                serialPortManager.InitializeSerialPort(comPort);
            }
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

            //testModel.TorqueNm = textBoxTorqueNm.Text.ToDouble();
            //testModel.SpeedRPM = textBoxSpeedRPM.Text.ToDouble();
            //testModel.ShaftPawerKw = textBoxShaftPawerKw.Text.ToDouble();
            //testModel.LoadingFactor = textBoxLoadingFactor.Text.ToDouble();
            //testModel.MotorSizeHP = textBoxMotorSizeHP.Text.ToDouble();

            //testModel.VoltageV1 = textBoxVoltageV1.Text.ToDouble();
            //testModel.VoltageV2 = textBoxVoltageV2.Text.ToDouble();
            //testModel.VoltageV3 = textBoxVoltageV3.Text.ToDouble();
            //testModel.VoltageV4 = textBoxVoltageV4.Text.ToDouble();
            //testModel.CurrentA1 = textBoxCurrentA1.Text.ToDouble();
            //testModel.CurrentA2 = textBoxCurrentA2.Text.ToDouble();
            //testModel.CurrentA3 = textBoxCurrentA3.Text.ToDouble();
            //testModel.CurrentA4 = textBoxCurrentA4.Text.ToDouble();
            //testModel.PF1 = textBoxPF1.Text.ToDouble();
            //testModel.PF2 = textBoxPF2.Text.ToDouble();
            //testModel.PF3 = textBoxPF3.Text.ToDouble();
            //testModel.PF4 = textBoxPF4.Text.ToDouble();
            //testModel.ActivePower1 = textBoxActivePower1.Text.ToDouble();
            //testModel.ActivePower2 = textBoxActivePower2.Text.ToDouble();
            //testModel.ActivePower3 = textBoxActivePower3.Text.ToDouble();
            //testModel.ActivePower4 = textBoxActivePower4.Text.ToDouble();
            //testModel.FrequencyHZ = textBoxFrequencyHZ.Text.ToDouble();

            //testModel.AmbientTempC = textBoxAmbientTempC.Text.ToDouble();
            //testModel.motorTempC = textBoxmotorTempC.Text.ToDouble();
            //testModel.EstimitedEfficency = textBoxEstimitedEfficency.Text.ToDouble();
            //testModel.label1Count = null; 
            #endregion
            #region Value From DummyData
            testModel.ReportNo = Convert.ToInt32(ReportNo);//DummyData.btnRecordNoLoadPoint_Click.GetReportNo();
            testModel.TorqueNm = DummyData.btnRecordNoLoadPoint_Click.GetTorqueNm();
            testModel.SpeedRPM = DummyData.btnRecordNoLoadPoint_Click.GetSpeedRPM();
            testModel.ShaftPowerkW = DummyData.btnRecordNoLoadPoint_Click.GetShaftPowerKw();
            testModel.LoadingFactor = DummyData.btnRecordNoLoadPoint_Click.GetLoadingFactor();
            testModel.MotorSize = DummyData.btnRecordNoLoadPoint_Click.GetMotorSize();

            testModel.VoltageV_Value1 = DummyData.btnRecordNoLoadPoint_Click.GetVoltageV1();
            testModel.VoltageV_Value2 = DummyData.btnRecordNoLoadPoint_Click.GetVoltageV2();
            testModel.VoltageV_Value3 = DummyData.btnRecordNoLoadPoint_Click.GetVoltageV3();
            testModel.VoltageV_Value4 = DummyData.btnRecordNoLoadPoint_Click.GetVoltageV4();
            testModel.CurrentA_Value1 = DummyData.btnRecordNoLoadPoint_Click.GetCurrentA1();
            testModel.CurrentA_Value2 = DummyData.btnRecordNoLoadPoint_Click.GetCurrentA2();
            testModel.CurrentA_Value3 = DummyData.btnRecordNoLoadPoint_Click.GetCurrentA3();
            testModel.CurrentA_Value4 = DummyData.btnRecordNoLoadPoint_Click.GetCurrentA4();
            testModel.FrequencyHZ = DummyData.btnRecordNoLoadPoint_Click.GetFrequencyHZ();
            testModel.Frequency_Value1 = DummyData.btnRecordNoLoadPoint_Click.GetFrequency_Value1();
            testModel.Frequency_Value2 = DummyData.btnRecordNoLoadPoint_Click.GetFrequency_Value2();
            testModel.Frequency_Value3 = DummyData.btnRecordNoLoadPoint_Click.GetFrequency_Value3();
            testModel.Frequency_Value4 = DummyData.btnRecordNoLoadPoint_Click.GetFrequency_Value4();
            testModel.ActivePower_Value1 = DummyData.btnRecordNoLoadPoint_Click.GetActivePower1();
            testModel.ActivePower_Value2 = DummyData.btnRecordNoLoadPoint_Click.GetActivePower2();
            testModel.ActivePower_Value3 = DummyData.btnRecordNoLoadPoint_Click.GetActivePower3();
            testModel.ActivePower_Value4 = DummyData.btnRecordNoLoadPoint_Click.GetActivePower4();

            testModel.AmbientTemperature = DummyData.btnRecordNoLoadPoint_Click.GetAmbientTemperature();
            testModel.MotorTemperature = DummyData.btnRecordNoLoadPoint_Click.GetMotorTemperature();
            testModel.EstimitedEfficiency = DummyData.btnRecordNoLoadPoint_Click.GetEstimitedEfficiency();
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
