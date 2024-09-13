using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;

namespace KomaxApp.UI_Design
{
    public partial class ParentForm : Form
    {

        Dashboard dashboard;
        Create create;
        Display display;
        ReportForm reportForm;
        ConfigurationForm configurationForm;

        private System.Windows.Forms.Timer pollingTimer;
        public string _powerMeter;
        public string _torqueMeter;
        public string _rpm;
        public string _temperature;
        private Dictionary<string, SerialPort> serialPorts = new Dictionary<string, SerialPort>();
        private SerialPort serialPort;

        public ParentForm(string powerMeter, string torqueMeter, string rpm, string temperature)
        {
            InitializeComponent();
            _powerMeter = powerMeter;
            _torqueMeter = torqueMeter;
            _rpm = rpm;
            _temperature = temperature;
        }


        private void btnStartReadng_Click(object sender, EventArgs e)
        {
            try
            {
                btnStartReadng.BackColor = System.Drawing.Color.FromArgb(38, 166, 66);

                if (_powerMeter == null && _torqueMeter == null && _rpm == null && _temperature == null)
                {
                    JIMessageBox.WarningMessage("COM Ports are not Configure");
                    return;
                }

              

                List<string> comPorts = new List<string>  //dynamic
                    {
                        _powerMeter,
                        _torqueMeter
                        ,_rpm,
                        _temperature,
                    };



                #region Logic
                foreach (string portName in comPorts)
                {
                    if (!serialPorts.ContainsKey(portName))
                    {
                        SerialPort serialPort = new SerialPort()
                        {
                            PortName = portName,
                            BaudRate = 9600,
                            Parity = Parity.None,
                            DataBits = 8,
                            StopBits = StopBits.One,
                            Handshake = Handshake.None,
                            ReadTimeout = 5000
                        };

                        serialPorts[portName] = serialPort;
                    }

                    if (!serialPorts[portName].IsOpen)
                    {
                        serialPorts[portName].Open();
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                JIMessageBox.WarningMessage("Exception: " + ex.Message);
            }
        }






        public ParentForm()
        {
            InitializeComponent();
        }

        private void ParentForm_Load(object sender, EventArgs e)
        {
            //if (dashboard == null)
            //{
            //    dashboard = new Dashboard(null, null, null, null, null);
            //    dashboard.MdiParent = this;
            //    dashboard.Dock = DockStyle.Fill;
            //    dashboard.Show();
            //}
            //else
            //{
            //    dashboard.Activate();
            //}

            if (configurationForm == null)
            {
                configurationForm = new ConfigurationForm();
                configurationForm.MdiParent = this;
                configurationForm.Dock = DockStyle.Fill;
                configurationForm.Show();
            }
            else
            {
                configurationForm.Activate();
            }


        }

        private void pnBtnDashboard_Click(object sender, EventArgs e)
        {   // Check if the Dashboard form is open and close it
            if (dashboard != null)
            {
                dashboard.Close();
                dashboard = null;   // Set the dashboard instance to null
            }
            if (dashboard == null)
            {
                //dashboard = new Dashboard(null);

                dashboard = new Dashboard(null,
                                          ConfigurationForm.ddPowerMeter,  // Access static field using the class name
                                          ConfigurationForm.ddTorqueMeter, // Access static field using the class name
                                          ConfigurationForm.ddRPM,         // Access static field using the class name
                                          ConfigurationForm.ddTemperature  // Access static field using the class name
                                        , this );
                dashboard.MdiParent = this;
                dashboard.Dock = DockStyle.Fill;
                dashboard.Show();
            }
            else
            {
                dashboard._powerMeter = ConfigurationForm.ddPowerMeter;
                dashboard._torqueMeter = ConfigurationForm.ddTorqueMeter;
                dashboard._rpm = ConfigurationForm.ddRPM;
                dashboard._temperature = ConfigurationForm.ddTemperature;
                dashboard.Activate();
            }
        }

        private void pnBtnMotorTestingCreate_Click(object sender, EventArgs e)
        {
            if (create != null)
            {
                create.Close();  // Close the dashboard form
                create = null;   // Set the dashboard instance to null
            }
            if (create == null)
            {
                create = new Create(null);
                create.MdiParent = this;
                create.Dock = DockStyle.Fill;
                create.Show();
            }
            else
            {
                create.Activate();
            }
        }

        private void iconButtonDisplay_Click(object sender, EventArgs e)
        {// Check if the Display form is open and close it
            if (display != null)
            {
                display.Close();
                display = null;   // Set the display instance to null
            }
            if (display == null)
            {
                display = new Display(
                                    ConfigurationForm.ddPowerMeter,  // Access static field using the class name
                                    ConfigurationForm.ddTorqueMeter, // Access static field using the class name
                                    ConfigurationForm.ddRPM,         // Access static field using the class name
                                    ConfigurationForm.ddTemperature  // Access static field using the class name
                                   );

                display.MdiParent = this;
                display.Dock = DockStyle.Fill;
                display.Show();
            }
            else
            {
                display._powerMeter = ConfigurationForm.ddPowerMeter;
                display._torqueMeter = ConfigurationForm.ddTorqueMeter;
                display._rpm = ConfigurationForm.ddRPM;
                display._temperature = ConfigurationForm.ddTemperature;
                display.displayGridView();
                display.Activate();
            }
        }

        private void pnBtnLogout_Click(object sender, EventArgs e)
        {
            if (configurationForm != null)
            {
                configurationForm.Close();  // Close the dashboard form
                configurationForm = null;   // Set the dashboard instance to null
            }
            if (configurationForm == null)
            {
                configurationForm = new ConfigurationForm();
                configurationForm.MdiParent = this;
                configurationForm.Dock = DockStyle.Fill;
                configurationForm.Show();
            }
            else
            {
                configurationForm.Activate();
            }
        }

        private void iconButtonReport_Click(object sender, EventArgs e)
        {
            if (reportForm != null)
            {
                reportForm.Close();  // Close the dashboard form
                reportForm = null;   // Set the dashboard instance to null
            }
            if (reportForm == null)
            {
                reportForm = new ReportForm();
                reportForm.MdiParent = this;
                reportForm.Dock = DockStyle.Fill;
                reportForm.Show();
            }
            else
            {
                reportForm.Activate();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close all serial ports
            foreach (var serialPort in serialPorts.Values)
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
            }
        }

        // Method to provide access to open serial ports
        //public SerialPort GetSerialPort(string portName)
        //{
        //    if (serialPorts.ContainsKey(portName) && serialPorts[portName].IsOpen)
        //    {
        //        return serialPorts[portName];
        //    }
        //    return null;
        //}

        // Method to return all open serial ports
        public Dictionary<string, SerialPort> GetAllOpenSerialPorts()
        {
            return serialPorts.Where(sp => sp.Value.IsOpen).ToDictionary(sp => sp.Key, sp => sp.Value);
        }

    }
}
