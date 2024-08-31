using komaxApp.BusinessLayer;
using komaxApp.DatabaseLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KomaxApp.UI_Design
{
    public partial class ConfigurationForm : BaseForm
    {
        private GenericRepo genericRepo;
        public ConfigurationForm()
        {
            InitializeComponent();
            genericRepo = new GenericRepo();
            btnComportRefresh_Click(null, null);
        }

        public static  string ddPowerMeter;
        public static string ddTorqueMeter;
        public static string ddRPM;
        public static string ddTemperature;
        private void ConfigurationForm_Load(object sender, EventArgs e)
        {
            infoMessages.Clear();
            (string PowerMeterPort, string TorqueMeterPort, string RPMPort, string TemperaturePort) = new GetListDL().GetLastComboPortsOrFetchFromSerialPort();
            if(!string.IsNullOrEmpty(PowerMeterPort))
            {
                cbPowerMeter.Text = PowerMeterPort;
                infoMessages.Text = "PowerMeterPort, ";
            }
            else
            {
                genericRepo.RefreshPortList(cbPowerMeter, labelInfo);
            }
            if (!string.IsNullOrEmpty(TorqueMeterPort))
            {
                cbTorqueMeter.Text = TorqueMeterPort;
                infoMessages.Text += "TorqueMeterPort, ";
            }
            else
            {
                genericRepo.RefreshPortList(cbTorqueMeter, labelInfo);
            }
            if (!string.IsNullOrEmpty(RPMPort))
            {
                cbRPM.Text = RPMPort;
                infoMessages.Text += "RPMPort, ";
            }
            else
            {
                genericRepo.RefreshPortList(cbRPM, labelInfo);
            }
            if (!string.IsNullOrEmpty(TemperaturePort))
            {
                cbTemperature.Text = TemperaturePort;
                infoMessages.Text += "TemperaturePort.";
            }
            else
            {
                genericRepo.RefreshPortList(cbTemperature, labelInfo);
            }
            ddPowerMeter = cbPowerMeter.Text;
            ddTorqueMeter = cbTorqueMeter.Text;
            ddRPM = cbRPM.Text;
            ddTemperature = cbTemperature.Text;
        }

        private void btnComportRefresh_Click(object sender, EventArgs e)
        {
            genericRepo.RefreshPortList(cbPowerMeter, labelInfo);
            genericRepo.RefreshPortList(cbTorqueMeter, labelInfo);
            genericRepo.RefreshPortList(cbRPM, labelInfo);
            genericRepo.RefreshPortList(cbTemperature, labelInfo);
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            string _cbPowerMeter = cbPowerMeter.Text;
            string _cbTorqueMeter = cbTorqueMeter.Text;
            string _cbRPM = cbRPM.Text;
            string _cbTemperature = cbTemperature.Text;
            new InsertBL().InsertComboPortsBL(_cbPowerMeter, _cbTorqueMeter, _cbRPM, _cbTemperature);
            ddPowerMeter = _cbPowerMeter;
            ddTorqueMeter = _cbTorqueMeter;
            ddRPM = _cbRPM;
            ddTemperature = _cbTemperature;
        }
    }
}
