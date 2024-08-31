using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using KomaxApp.Model.LoadTest;
namespace KomaxApp.UI_Design
{
    public partial class PopUp : Form
    {
        private RequestLoadTestModel.Request request { get; set; }
        public PopUp(RequestLoadTestModel.Request _request)
        {
            InitializeComponent();
            request = _request;
        }

        private void PopUp_Load(object sender, EventArgs e)
        {
            GetRecordToPopup(request);

        }

        private void GetRecordToPopup(RequestLoadTestModel.Request request)
        {
            //lblReportNo.Text = request.ReportNo.ToString();
            lblActivePower_Value1.Text = request.ActivePower_Value1.ToString();
            lblActivePower_Value2.Text = request.ActivePower_Value2.ToString();
            lblActivePower_Value3.Text = request.ActivePower_Value3.ToString();
            lblActivePower_Value4.Text = request.ActivePower_Value4.ToString();
            lblAmbientTemperature.Text = request.AmbientTemperature.ToString();
            lblCurrentA_Value1.Text = request.CurrentA_Value1.ToString();
            lblCurrentA_Value2.Text = request.CurrentA_Value2.ToString();
            lblCurrentA_Value3.Text = request.CurrentA_Value3.ToString();
            lblCurrentA_Value4.Text = request.CurrentA_Value4.ToString();
            //lblEntryDate.Text = request.EntryDate.ToString();
            lblEstimitedEfficiency.Text = request.EstimitedEfficiency.ToString();
            lblFrequency_Value.Text = request.FrequencyHZ.ToString();
            lblFrequency_Value1.Text = request.Frequency_Value1.ToString();
            lblFrequency_Value2.Text = request.Frequency_Value2.ToString();
            lblFrequency_Value3.Text = request.Frequency_Value3.ToString();
            lblFrequency_Value4.Text = request.Frequency_Value4.ToString();
            lblLoadingFactor.Text = request.LoadingFactor.ToString();
            lblMotorSize.Text = request.MotorSize.ToString();
            lblMotorTemperature.Text = request.MotorTemperature.ToString();
            lblShaftPowerkW.Text = request.ShaftPowerkW.ToString();
            lblSpeedRPM.Text = request.SpeedRPM.ToString();
            lblTorqueNm.Text = request.TorqueNm.ToString();
            lblVoltageV_Value1.Text = request.VoltageV_Value1.ToString();
            lblVoltageV_Value2.Text = request.VoltageV_Value2.ToString();
            lblVoltageV_Value3.Text = request.VoltageV_Value3.ToString();
            lblVoltageV_Value4.Text = request.VoltageV_Value4.ToString();
        }

        private void btnRecordLoadPoint_Click(object sender, EventArgs e)
        {
            // Do something on Yes
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Do something on No
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
