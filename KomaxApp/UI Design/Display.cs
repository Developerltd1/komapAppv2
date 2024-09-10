using komaxApp.BusinessLayer;
using komaxApp.DatabaseLayer;
using komaxApp.Utility.ExtensionMethod;
using KomaxApp.GenericCode;
using KomaxApp.Model.Create;
using KomaxApp.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;

namespace KomaxApp.UI_Design
{
    public partial class Display : BaseForm//Form
    {
        public string _powerMeter;
        public string _torqueMeter;
        public string _rpm;
        public string _temperature;
        public Display(string powerMeter, string torqueMeter, string rpm, string temperature)
        {
            InitializeComponent();
            _powerMeter = powerMeter;
            _torqueMeter = torqueMeter;
            _rpm = rpm;
            _temperature = temperature;

        }

        private void Display_Load(object sender, EventArgs e)
        {
            displayGridView();
        }

        public void displayGridView()
        {

            new GetListBL().GetDisplayListBL(GridViewDisplay);
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            new GetListBL().SearchDataFromGrid(GridViewDisplay, "ReportNo", textBoxSearch);
        }

        private void GridViewDisplay_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the button column
            if (e.RowIndex >= 0 && e.ColumnIndex == GridViewDisplay.Columns["Actions"].Index)
            {
                // Get the selected row
                DataGridViewRow row = GridViewDisplay.Rows[e.RowIndex];

                // Check if the button text is "Start Test"
                DataGridViewButtonCell buttonCell = (DataGridViewButtonCell)row.Cells["Actions"];
                if (buttonCell.Value.ToString() == "Start Test")
                {
                    string ReportNo = row.Cells["ReportNo"].Value.ToString();
                    #region MyRegion
                    if (_powerMeter == null && _torqueMeter == null && _rpm == null && _temperature == null)
                    {
                        JIMessageBox.WarningMessage("COM Ports are not Configure");
                        return;
                    }
                    else if (_powerMeter == "No COM" && _torqueMeter == "No COM" && _rpm == "No COM" && _temperature == "No COM")
                    {
                        JIMessageBox.WarningMessage("COM Ports are not Configure");
                        return;
                    }
                    #endregion
                    // Create and show TestForm with the collected data
                    LoadTest testForm = new LoadTest(ReportNo, _powerMeter, _torqueMeter, _rpm, _temperature);
                    testForm.MdiParent = this.MdiParent; // Set MDI parent if needed
                    testForm.Dock = DockStyle.Fill; // Adjust docking as needed
                    testForm.Show();
                }
            }
        }

        private void GridViewDisplay_DoubleClick(object sender, EventArgs e)
        {

            #region Code
            // Check if a row is selected
            if (GridViewDisplay.CurrentRow != null)
            {
                DataGridViewRow selectedRow = GridViewDisplay.CurrentRow;
                string reportNo = selectedRow.Cells["ReportNo"].Value.ToString();
                Create _form = new Create(reportNo);
                _form.MdiParent = this.MdiParent;
                _form.Dock = DockStyle.Fill;
                _form.Show();

            }
            #endregion

        }
    }
}
