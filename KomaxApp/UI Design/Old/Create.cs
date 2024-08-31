using komaxApp.BusinessLayer;
using komaxApp.DatabaseLayer;
using komaxApp.Utility.ExtensionMethod;
using KomaxApp.Model.Create;
using KomaxApp.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;

namespace KomaxApp.UI_Design
{
    public partial class Create : BaseForm
    {
        public Create()
        {
            InitializeComponent();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                // KomaxApp.Model.Create.CreateModel.Rqeuest rqeuest = new Model.Create.CreateModel.Rqeuest();
                VmCreateMotor vmCreate = new VmCreateMotor();
                vmCreate.createModel.ReportNo =  tbReportNo.Text.ToInt32(); 
                vmCreate.createModel.TestDate = dateTimePickerTestDate.Value;
                vmCreate.createModel.HitachiCruve = tbHitachiCruve.Text;
                vmCreate.createModel.Manufacturer = tbManufacturer.Text;
                vmCreate.createModel.MotorModel = tbModel.Text;
                vmCreate.createModel.MotorType = tbType.Text;
                vmCreate.createModel.Frame = tbFrame.Text;
                vmCreate.createModel.Phase = tbPhase.Text.ToInt32();
                vmCreate.createModel.MotorRatedKw = tbMotorRatedKw.Text.ToInt32();
                vmCreate.createModel.MotorRatedHP = tbMotorRatedHP.Text.ToInt32(); 
                vmCreate.createModel.MotorRatedVoltage = tbMotorRatedVoltage.Text.ToInt32(); 
                vmCreate.createModel.MotorRatedFrequency = tbHERTZ.Text.ToInt32();
                vmCreate.createModel.MotorRatedCurrent = tbAMPS.Text.ToInt32();
                vmCreate.createModel.MotorRatedPowerFactor = tbPF.Text.ToInt32();
                vmCreate.createModel.MotorRatedRPM = tbRPM.Text.ToInt32();
                vmCreate.createModel.NofPoles = tbPole.Text.ToInt32();
                vmCreate.createModel.Efficency = (double)tbEfficency.Text.ToDouble();
                vmCreate.createModel.Duty_SerivceFactor = tbDuty_SerivceFactor.Text.ToInt32(); 
                vmCreate.createModel.InsulationClass = tbInsClass.Text;
                vmCreate.createModel.CoolingClass = tbIC_CoolingClass.Text;
                vmCreate.createModel.IPRating = tbIPRating.Text;
                vmCreate.createModel.ConnectionType = tbConnectionType.Text;
                vmCreate.createModel.SerialNo = tbSerialNo.Text;
                vmCreate.createModel.SpecifiedTemperature = tbSpecifiedTemperature.Text.ToInt32();
                vmCreate.createModel.WindingResistanceinOhm = tbWindingResistanceinOhm.Text.ToInt32();
                vmCreate.createModel.TempAtWindingResistanceIsMeasured = tbTempAtWindingResistanceIsMeasured.Text.ToInt32();
                vmCreate.createModel.rbDescription = rbDescription.Text;

                //List<ShaftPowerObj.Request> lstshaftPower = new List<ShaftPowerObj.Request>();
                vmCreate.shaftPowerObj.EntryDateTime = DateTime.Now;
                vmCreate.shaftPowerObj.Row1 = tbShaftPower_P2_in_kW1.Text.ToNullableInt32();
                vmCreate.shaftPowerObj.Row2 =  tbShaftPower_P2_in_kW2.Text.ToNullableInt32();
                vmCreate.shaftPowerObj.Row3 =  tbShaftPower_P2_in_kW3.Text.ToNullableInt32();
                vmCreate.shaftPowerObj.Row4 =  tbShaftPower_P2_in_kW4.Text.ToNullableInt32();
                vmCreate.shaftPowerObj.Row5 =  tbShaftPower_P2_in_kW5.Text.ToNullableInt32();
                vmCreate.shaftPowerObj.Row6 =  tbShaftPower_P2_in_kW6.Text.ToNullableInt32();
                vmCreate.shaftPowerObj.Row7 =  tbShaftPower_P2_in_kW7.Text.ToNullableInt32();

                vmCreate.efficiencyObj.Row1 =  tbEfficiency1.Text.ToNullableInt32();
                vmCreate.efficiencyObj.Row2 =  tbEfficiency2.Text.ToNullableInt32();
                vmCreate.efficiencyObj.Row3 =  tbEfficiency3.Text.ToNullableInt32();
                vmCreate.efficiencyObj.Row4 =  tbEfficiency4.Text.ToNullableInt32();
                vmCreate.efficiencyObj.Row5 =  tbEfficiency5.Text.ToNullableInt32();
                vmCreate.efficiencyObj.Row6 =  tbEfficiency6.Text.ToNullableInt32();
                vmCreate.efficiencyObj.Row7 =  tbEfficiency7.Text.ToNullableInt32();

                vmCreate.speedInRPMObj.Row1 =  tbSpeedinRPM1.Text.ToNullableInt32();
                vmCreate.speedInRPMObj.Row2 =  tbSpeedinRPM2.Text.ToNullableInt32();
                vmCreate.speedInRPMObj.Row3 =  tbSpeedinRPM3.Text.ToNullableInt32();
                vmCreate.speedInRPMObj.Row4 =  tbSpeedinRPM4.Text.ToNullableInt32();
                vmCreate.speedInRPMObj.Row5 =  tbSpeedinRPM5.Text.ToNullableInt32();
                vmCreate.speedInRPMObj.Row6 =  tbSpeedinRPM6.Text.ToNullableInt32();
                vmCreate.speedInRPMObj.Row7 =  tbSpeedinRPM7.Text.ToNullableInt32();

                vmCreate.currentInAmpsObj.Row1 =  tbCurrentInAmps1.Text.ToNullableInt32();
                vmCreate.currentInAmpsObj.Row2 =  tbCurrentInAmps2.Text.ToNullableInt32();
                vmCreate.currentInAmpsObj.Row3 =  tbCurrentInAmps3.Text.ToNullableInt32();
                vmCreate.currentInAmpsObj.Row4 =  tbCurrentInAmps4.Text.ToNullableInt32();
                vmCreate.currentInAmpsObj.Row5 =  tbCurrentInAmps5.Text.ToNullableInt32();
                vmCreate.currentInAmpsObj.Row6 =  tbCurrentInAmps6.Text.ToNullableInt32();
                vmCreate.currentInAmpsObj.Row7 =  tbCurrentInAmps7.Text.ToNullableInt32();
                new InsertBL().InsertRecordsBL(vmCreate);
                JIMessageBox.InformationMessage("Record Saved Successfully!");
                gridView();
            }
            catch (Exception ex)
            {
                JIMessageBox.ErrorMessage("Record Not Saved,  Error: "+ ex.Message);
            }

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            string saveDirectory = @"D:\Company\AppsparqTech\0.ALL_ASSETS\SupportedProjects\Series P2\KomaxApp\KomaxApp\Images\";
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (!Directory.Exists(saveDirectory))
                    {
                        Directory.CreateDirectory(saveDirectory);
                    }

                    string fileName = Path.GetFileName(openFileDialog1.FileName);
                    string fileSavePath = Path.Combine(saveDirectory, fileName);
                    File.Copy(openFileDialog1.FileName, fileSavePath, true);

                    string constr = @"Data Source=.\SQL2014;Initial Catalog=AjaxSamples;Integrated Security=true";
                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        string sql = "INSERT INTO Files VALUES(@Name, @Path)";
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@Name", Path.GetFileName(fileName));
                            cmd.Parameters.AddWithValue("@Path", fileSavePath);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }

                   // this.BindDataGridView();
                }
            }
        }

        private void Create_Load(object sender, EventArgs e)
        {
            gridView();
        }

        private void gridView()
        {
            new GetListBL().GetMotorListBL(dataGridViewCreate);
        }
    }
}
