using komaxApp.BusinessLayer;
using komaxApp.DatabaseLayer;
using komaxApp.Utility.ExtensionMethod;
using KomaxApp.GenericCode;
using KomaxApp.Model.Create;
using KomaxApp.Model.Reporting;
using KomaxApp.Model.ViewModel;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Microsoft.Reporting.WinForms;
using KomaxApp.Model.Reporting.Model.Page1;
using KomaxApp.Model.Reporting.Model.Page2;
using KomaxApp.Model.Reporting.Model.Page3;
using KomaxApp.Model.Reporting.Model.Page4And5;

namespace KomaxApp.UI_Design
{
    public partial class Display : BaseForm//Form
    {
        private ParentForm parentForm;
        public string _powerMeter;
        public string _torqueMeter;
        public string _rpm;
        public string _temperature;
        public string _TorqueNmConfiguration;
        public Display(string powerMeter, string torqueMeter, string rpm, string temperature, string TorqueNmConfiguration, ParentForm parent)
        {
            InitializeComponent();
            _powerMeter = powerMeter;
            _torqueMeter = torqueMeter;
            _rpm = rpm;
            _temperature = temperature;
            _TorqueNmConfiguration = TorqueNmConfiguration;
            parentForm = parent;
        }

        private void Display_Load(object sender, EventArgs e)
        {
            displayGridView();
            this.reportViewer.RefreshReport();
            this.reportViewer.RefreshReport();
        }

        public async void displayGridView()
        {
            // Clear existing rows in the DataGridView before displaying new data
            GridViewDisplay.Rows.Clear();

            // Get the DataTable from the business logic layer
            await  new GetListBL().GetDisplayListBL(GridViewDisplay);

            //// Check if the IsFilled column exists in the DataTable
            //if (!dt.Columns.Contains("IsFilled"))
            //{
            //    Console.WriteLine("The column 'IsFilled' does not exist in the DataTable.");
            //    return; // Exit the method if the column does not exist
            //}

            //// Iterate through each row in the DataTable
            //foreach (DataRow row in dt.Rows)
            //{
            //    // Determine the index for the new row in DataGridView
            //    int rowIndex = GridViewDisplay.Rows.Add(
            //        row["ReportNo"].ToString(),
            //        row["TestDate"].ToString(),
            //        row["IsFilled"].ToString()
            //    );

            //    // Get the value from the IsFilled column
            //    string isFilledValue = row["IsFilled"].ToString(); // Convert to string

            //    // Compare the value to "True"
            //    if (isFilledValue.Equals("True", StringComparison.OrdinalIgnoreCase))
            //    {
            //        // Set the row color to Transparent for "True"
            //        GridViewDisplay.Rows[rowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
            //    }
            //    else
            //    {
            //        // Set the row color to Green for "False"
            //        GridViewDisplay.Rows[rowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
            //    }
            //}
        }


        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            new GetListBL().SearchDataFromGrid(GridViewDisplay, "ReportNo", textBoxSearch);
        }
        private void GridViewDisplay_DoubleClick(object sender, EventArgs e)
        {

            #region Code
            // Check if a row is selected
            if (GridViewDisplay.CurrentRow != null)
            {
                DataGridViewRow selectedRow = GridViewDisplay.CurrentRow;
                string reportNo = selectedRow.Cells["ReportNo"].Value.ToString();
                Create _form = new Create(reportNo, parentForm);
                _form.MdiParent = this.MdiParent;
                _form.Dock = DockStyle.Fill;
                _form.Show();

            }
            #endregion

        }


        private void GridViewDisplay_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the button column
            if (e.RowIndex >= 0 && e.ColumnIndex == GridViewDisplay.Columns["Actions"].Index)
            {
                // Get the selected rowconflicts Thread v1
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

                    LoadTest testForm = null;
                    //if (testForm != null)
                    //{
                    //    testForm.Close();  // Close the dashboard form
                    //    testForm = null;   // Set the dashboard instance to null
                    //}
                    // Create and show TestForm with the collected data
                    testForm = new LoadTest(ReportNo, _powerMeter, _torqueMeter, _rpm, _temperature, _TorqueNmConfiguration, parentForm);
                    testForm.MdiParent = this.MdiParent; // Set MDI parent if needed
                    testForm.Dock = DockStyle.Fill; // Adjust docking as needed
                    testForm.Show();

                  
                }
                if (buttonCell.Value.ToString() == "Download Report")
                {
                    string ReportNo = row.Cells["ReportNo"].Value.ToString();
                    AutoCreatePDFFile(ReportNo);

                    // Set the row color to Green for "Download Report"
                    row.DefaultCellStyle.BackColor = Color.Green;
                }
            }
        }

       
        #region ReportStructure
        private void InitializeReportViewer()
        {
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.Dock = DockStyle.Fill;
            this.Controls.Add(reportViewer);
        }

        private async void AutoCreatePDFFile(string ReportNo)
        {
            ReportingModel reportingModel = await new GetListBL().GetDataForReportBL0(ReportNo);
            Reporting(reportingModel);

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            reportViewer.LocalReport.EnableExternalImages = true;

            // Render the PDF content into a byte array
            byte[] bytes = reportViewer.LocalReport.Render(
                "PDF",
                null,
                out mimeType,
                out encoding,
                out extension,
                out streamIds,
                out warnings);
            string filename_ = "filename_" + DateTime.Now.ToString("yy-MM-dd hh-mm-ss tt");

            // Use a SaveFileDialog to ask the user where to save the PDF
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog.Title = "Save PDF File";
                saveFileDialog.FileName = filename_ + ".pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Write the byte array to the specified file path
                    string filePath = saveFileDialog.FileName;
                    try
                    {
                        File.WriteAllBytes(filePath, bytes);
                        MessageBox.Show("PDF file saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving PDF file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void Reporting(ReportingModel reportingModel)
        {
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.ReportPath = @"Reports/FivePageReport.rdlc";
            if (reportingModel == null)
            {
                JIMessageBox.WarningMessage("No Record Found");
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.RefreshReport();
                return;
            }

            // Wrap the model in a list
            var _page1 = new List<Page1ModelFinilize> { reportingModel.page1Mdl };
            ReportDataSource Rds1 = new ReportDataSource("DataSetPage1", _page1);
            reportViewer.LocalReport.DataSources.Add(Rds1);

            var _page2 = new List<Page2ModelFinilize> { reportingModel.page2Mdl };
            ReportDataSource Rds2 = new ReportDataSource("DataSetPage2", _page2);
            reportViewer.LocalReport.DataSources.Add(Rds2);

            var _page3 = new List<Page3ModelFinilize> { reportingModel.page3Mdl };
            ReportDataSource Rds3 = new ReportDataSource("DataSetPage3", _page3);
            reportViewer.LocalReport.DataSources.Add(Rds3);

            ReportDataSource Rds45 = new ReportDataSource("DataSet45", new List<Page4And5ModelFinilize> { reportingModel.page4And5Mdl });
            reportViewer.LocalReport.DataSources.Add(Rds45);
            DataTable dt = new Conversion().ListToDataTable(reportingModel._Chart1Model);
            ReportDataSource RdsChart1 = new ReportDataSource("DataSetChart1", dt);
            reportViewer.LocalReport.DataSources.Add(RdsChart1);



            reportViewer.RefreshReport();


        }

        #endregion
    }
}
