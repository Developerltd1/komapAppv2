using komaxApp.DatabaseLayer;
using KomaxApp.Model.Create;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using komaxApp.Utility.ExtensionMethod;
using System.Windows.Forms;
using Utility;
using KomaxApp.Model.Display;
using KomaxApp.Model.LoadTest;
using KomaxApp.Model.ViewModel;
using KomaxApp.Model.Reporting;
using KomaxApp.Model.Reporting.ViewModel;
using KomaxApp.Model.Page1;
using KomaxApp.Model.Reporting.Model.Page1;

namespace komaxApp.BusinessLayer
{
    public class GetListBL
    {
        public DataTable GetMotorListBL(DataGridView dataGridView)
        {
            DataTable dt = new Conversion().ListToDataTable(new GetListDL().GetMotorList());
            dataGridView.Rows.Clear();
            foreach (DataRow r in dt.Rows)
            {
                // Parse the TestDate column to DateTime
                DateTime testDate = DateTime.Parse(r["TestDate"].ToString());
                // Format the DateTime to the desired format
                string formattedTestDate = testDate.ToString("dd-MMM-yy hh:mm tt");
                dataGridView.Rows.Add(
                    r["ReportNo"].ToString(), formattedTestDate, r["Manufacturer"].ToString(), r["MotorModel"].ToString(), r["MotorType"].ToString(), r["Frame"].ToString(), r["Phase"].ToString(), r["MotorRatedKw"].ToString(), r["MotorRatedHP"].ToString(), r["MotorRatedVoltage"].ToString());
            }
            return dt;
        }

        public async Task<VmCreateMotor> GetDatausingReportBL(string ReportNo)
        {
            return await  new GetListDL().GetDatausingReportDL(ReportNo);
        }
            public DataTable GetDisplayListBL(DataGridView dataGridView)
        {
            DataTable dt = new Conversion().ListToDataTable(new GetListDL().GetDisplayList());

            // Clear existing rows and columns in the DataGridView
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            // Define columns if not already defined
            dataGridView.Columns.Add("SerialNo", "Serial No");
            dataGridView.Columns.Add("ReportNo", "Report No");
            dataGridView.Columns.Add("TestDate", "Test Date");

            // Add a button column
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.Name = "Actions";
            buttonColumn.HeaderText = "Actions";
            dataGridView.Columns.Add(buttonColumn);


            // Set all columns to fill the remaining space
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }



            foreach (DataRow r in dt.Rows)
            {
                string editButtonText = "Edit";

                DateTime testDate = DateTime.Parse(r["TestDate"].ToString());
                string formattedTestDate = testDate.ToString("dd-MMM-yy hh:mm tt");



                // Determine the button text and enabled state based on the isFilled condition
                bool isFilled = r["IsFilled"].ToString().ToLower() == "true";
                string buttonText = isFilled ? "Test Completed" : "Start Test";
                bool isButtonEnabled = !isFilled;






                int rowIndex = dataGridView.Rows.Add(
                    r["SerialNo"].ToString(), r["ReportNo"].ToString(), formattedTestDate, editButtonText, buttonText);


                // Access the button cell and set its properties
                DataGridViewButtonCell editCell = (DataGridViewButtonCell)dataGridView.Rows[rowIndex].Cells["Actions"];
                editCell.Value = editButtonText;
                editCell.Selected = isButtonEnabled;

                DataGridViewButtonCell buttonCell = (DataGridViewButtonCell)dataGridView.Rows[rowIndex].Cells["Actions"];
                buttonCell.Value = buttonText;



                dataGridView.Rows[rowIndex].Cells["Actions"].ReadOnly = !isButtonEnabled; // Disable the button if not enabled


            }

            return dt;
        }
        public bool IsReportNoExistinDbBL(string ReportNo)
        {
            return new GetListDL().IsReportNoExistinDbDL(ReportNo);
        }
        public object GetDataFromDbUsingReportNoBL(string ReportNo)
        {
           return new GetListDL().GetDataFromDbUsingReportNoDL(ReportNo);
        }
        public RequestLoadTestModel.LabelCountModel CheckLoadTestRecordExistBL(string ReportNo)
        {
            return new GetListDL().CheckLoadTestRecordExistDL(ReportNo);
        }
        public void SearchDataFromGrid(DataGridView GridViewDisplay, string cellRow, TextBox textBoxSearch)
        {
            try
            {
                foreach (DataGridViewRow row in GridViewDisplay.Rows)
                {
                    if (row.Cells[cellRow].Value.ToString().ToLower().Contains(textBoxSearch.Text.ToLower()))
                        row.Visible = true;
                    else
                        row.Visible = false;
                }
            }
            catch (Exception ex)
            {
                JIMessageBox.ErrorMessage(ex.ToString());
            }
        }







        #region Reporting
        //public async Task<ReportModel> GetDataForReportBL(string ReportNo)
        //{

        //    var response = new ReportModel();
        //    Page1Model page1Model = new Page1Model();

        //    page1Model = await new GetListDL().GetDataForReportDL0(ReportNo);

        //    return 0;

        //}
        public async Task<ReportingModel> GetDataForReportBL0(string ReportNo)
        {
            var _ReportingModel = new ReportingModel();
            _ReportingModel = await new GetListDL().GetDataForReportDL0(ReportNo);

            #region Calculations
            //_ReportingModel.page2Mdl
            #endregion


            return _ReportingModel;
        }
        #endregion

    }
}
