﻿using komaxApp.BusinessLayer;
using komaxApp.DatabaseLayer;
using komaxApp.Utility.ExtensionMethod;
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
        public Display()
        {
            InitializeComponent();
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
                    // Create and show TestForm with the collected data
                    LoadTest testForm = new LoadTest(ReportNo);
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
