using komaxApp.BusinessLayer;
using KomaxApp.Model.Page1;
using KomaxApp.Model.Reporting;
using KomaxApp.Model.Reporting.Model.Page1;
using KomaxApp.Model.ViewModel;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;
using komaxApp.Utility.ExtensionMethod;
using KomaxApp.Model.Reporting.Model.Page2;
using KomaxApp.Model.Reporting.Model.Page3;
using KomaxApp.Model.Reporting.Model.Page4And5;
using KomaxApp.Model.Page4And5.Entity;
using System.IO;

namespace KomaxApp.UI_Design
{
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {

            this.reportViewer.RefreshReport();
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
        private void textBoxSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                //SearchData(textBoxSearch.Text.Trim());
                AutoCreatePDFFile("0");
            }
        }

        private async void SearchData(string reportNo)
        {
            ReportingModel reportingModel = await new GetListBL().GetDataForReportBL0(reportNo);
            Reporting(reportingModel);
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
    
    
    }
}
