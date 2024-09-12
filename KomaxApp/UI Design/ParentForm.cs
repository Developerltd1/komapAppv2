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
    public partial class ParentForm : Form
    {

        Dashboard dashboard;
        Create create;
        Display display;
        ReportForm reportForm;
        ConfigurationForm configurationForm;
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
                                         );
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


    }
}
