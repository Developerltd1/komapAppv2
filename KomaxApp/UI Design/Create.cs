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
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;
using static System.Net.Mime.MediaTypeNames;

namespace KomaxApp.UI_Design
{
    public partial class Create : BaseForm
    {
        string reportNo;
        public Create(string reportNo)
        {
            InitializeComponent();
            this.reportNo = reportNo;
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
                vmCreate.createModel.ReportNo = tbReportNo.Text.ToInt32();
                if (buttonSave.Text != "Edit")
                {
                    IsReportNoExistinDb(vmCreate.createModel.ReportNo.ToString());
                }
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
                vmCreate.createModel.Efficency = tbEfficency.Text.ToDoble();
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
                vmCreate.createModel.RatedCurves = cbRatedCurves.Checked;
                if (cbRatedCurves.Checked)
                {
                    #region ShaftPower
                    var textBoxes = new List<TextBox>
                {
                    tbShaftPower_P2_in_kW1,
                    tbShaftPower_P2_in_kW2,
                    tbShaftPower_P2_in_kW3,
                    tbShaftPower_P2_in_kW4,
                    tbShaftPower_P2_in_kW5,
                    tbShaftPower_P2_in_kW6,
                    tbShaftPower_P2_in_kW7
                };

                    for (int i = 0; i < textBoxes.Count; i++)
                    {
                        var textBox = textBoxes[i];

                        // Check if the text box contains a valid integer
                        var rowValue = textBox.Text.ToNullableInt32();
                        if (rowValue.HasValue)
                        {
                            var _shaftPower = new ShaftPowerObj.Request
                            {
                                ReportNo = vmCreate.createModel.ReportNo,
                                EntryDateTime = DateTime.Now,
                                RowNo = i + 1, // Row number starts from 1
                                Row = rowValue.Value
                            };

                            vmCreate.lstShaftPower.Add(_shaftPower);
                        }
                    }

                    #endregion
                    #region Efficiency
                    var textBoxesEf = new List<TextBox>
                {
                    tbEfficiency1,
                    tbEfficiency2,
                    tbEfficiency3,
                    tbEfficiency4,
                    tbEfficiency5,
                    tbEfficiency6,
                    tbEfficiency7

                };




                    for (int i = 0; i < textBoxesEf.Count; i++)
                    {
                        var textBox = textBoxesEf[i];

                        // Check if the text box contains a valid integer
                        var rowValue = textBox.Text.ToNullableInt32();
                        if (rowValue.HasValue)
                        {
                            var _efficiency = new EfficiencyObj.Request
                            {
                                ReportNo = vmCreate.createModel.ReportNo,
                                EntryDateTime = DateTime.Now,
                                RowNo = i + 1, // Row number starts from 1
                                Row = rowValue.Value
                            };

                            vmCreate.lstEfficiency.Add(_efficiency);
                        }
                    }

                    #endregion
                    #region SpeedinRPM
                    var textBoxesSp = new List<TextBox>
                {
                    tbSpeedinRPM1,
                    tbSpeedinRPM2,
                    tbSpeedinRPM3,
                    tbSpeedinRPM4,
                    tbSpeedinRPM5,
                    tbSpeedinRPM6,
                    tbSpeedinRPM7
                };

                    for (int i = 0; i < textBoxesSp.Count; i++)
                    {
                        var textBox = textBoxesSp[i];

                        // Check if the text box contains a valid integer
                        var rowValue = textBox.Text.ToNullableInt32();
                        if (rowValue.HasValue)
                        {
                            var _speedInRPMObj = new SpeedInRPMObj.Request
                            {
                                ReportNo = vmCreate.createModel.ReportNo,
                                EntryDateTime = DateTime.Now,
                                RowNo = i + 1, // Row number starts from 1
                                Row = rowValue.Value
                            };

                            vmCreate.lstSpeedInRPM.Add(_speedInRPMObj);
                        }
                    }

                    #endregion
                    #region CurrentInAmps
                    var textBoxesCr = new List<TextBox>
                {
                    tbCurrentInAmps1,
                    tbCurrentInAmps2,
                    tbCurrentInAmps3,
                    tbCurrentInAmps4,
                    tbCurrentInAmps5,
                    tbCurrentInAmps6,
                    tbCurrentInAmps7
                };

                    for (int i = 0; i < textBoxesCr.Count; i++)
                    {
                        var textBox = textBoxesCr[i];

                        // Check if the text box contains a valid integer
                        var rowValue = textBox.Text.ToNullableInt32();
                        if (rowValue.HasValue)
                        {
                            var _currentObj = new CurrentInAmpsObj.Request
                            {
                                ReportNo = vmCreate.createModel.ReportNo,
                                EntryDateTime = DateTime.Now,
                                RowNo = i + 1, // Row number starts from 1
                                Row = rowValue.Value
                            };

                            vmCreate.lstCurrentInAmps.Add(_currentObj);
                        }
                    }
                    #endregion
                    #region Cos
                    var textBoxesCos = new List<TextBox>
                {
                    tbCos1,
                    tbCos2,
                    tbCos3,
                    tbCos4,
                    tbCos5,
                    tbCos6,
                    tbCos7

                };

                    for (int i = 0; i < textBoxesCos.Count; i++)
                    {
                        var textBox = textBoxesCos[i];

                        // Check if the text box contains a valid integer
                        var rowValue = textBox.Text.ToNullableInt32();
                        if (rowValue.HasValue)
                        {
                            var _currentObj = new CosObj.Request
                            {
                                ReportNo = vmCreate.createModel.ReportNo,
                                EntryDateTime = DateTime.Now,
                                RowNo = i + 1, // Row number starts from 1
                                Row = rowValue.Value
                            };

                            vmCreate.lstCos.Add(_currentObj);
                        }
                    }
                    #endregion
                }
                #region Image
                if (pictureBox.Image == null || pictureBoxLogo.Image == null)
                {
                    JIMessageBox.WarningMessage("please select image");
                    return;
                }
                //else  //Path
                //{
                //    string folderPath = Path.Combine(System.Windows.Forms.Application.StartupPath, "ImagesFolder");
                //    // Ensure the folder exists
                //    if (!Directory.Exists(folderPath))
                //    {
                //        Directory.CreateDirectory(folderPath);
                //    }
                //    string fullPath = Path.Combine(folderPath, vmCreate.createModel.ReportNo.ToString());
                //    vmCreate.imageObj.ReportNo = vmCreate.createModel.ReportNo;
                //    vmCreate.imageObj.EntryDateTime = DateTime.Now;
                //    vmCreate.imageObj.Image = fullPath;
                //}
                //Db
                vmCreate.imageObj.ReportNo = vmCreate.createModel.ReportNo;
                vmCreate.imageObj.Image = ImageClass.GetBase64StringFromImage(Imager.Resize(pictureBox.Image, 200, 200, true)); //Resize & Convert to String
                vmCreate.imageObj.LogoImage = ImageClass.GetBase64StringFromImage(Imager.Resize(pictureBoxLogo.Image, 200, 200, true)); //Resize & Convert to String

                #endregion
                CreateModel.Response response = new InsertBL().InsertRecordsBL(vmCreate, buttonSave.Text);
              

                if (response.StatusCode == 1)
                {
                    if (buttonSave.Text == "Edit")
                    {
                        JIMessageBox.InformationMessage("Record Updates Successfully!");
                        Display display = new Display(null,null,null,null);
                        display.MdiParent = this.MdiParent;
                        display.Dock = DockStyle.Fill;
                        display.Show();
                    }
                    else
                    {
                        JIMessageBox.InformationMessage("Record Saved Successfully!");
                        Display display = new Display(null, null, null, null);
                        display.MdiParent = this.MdiParent;
                        display.Dock = DockStyle.Fill;
                        display.Show();
                    }
                }


            }
            catch (Exception ex)
            {
                JIMessageBox.ErrorMessage("Record Not Saved,  Error: " + ex.Message);
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

        private async void Create_Load(object sender, EventArgs e)
        {
            RatedCurvesPanel.Visible = false;
            //Edit
            if (reportNo != null)
            {
                labelHeader.Text = "Edit Motor Test";
                 VmCreateMotor model = await new GetListBL().GetDatausingReportBL(reportNo);
                GetEditRecord(model);
            }

            gridView();
        }

        private void GetEditRecord(VmCreateMotor vmCreate)
        {
            try
            {
                buttonSave.Text = "Edit";
                tbReportNo.Enabled = false;
                tbReportNo.Text = vmCreate.createModel.ReportNo.ToString();
                dateTimePickerTestDate.Value = vmCreate.createModel.TestDate;
                tbHitachiCruve.Text = vmCreate.createModel.HitachiCruve.ToString();
                tbManufacturer.Text = vmCreate.createModel.Manufacturer.ToString();
                tbModel.Text = vmCreate.createModel.MotorModel.ToString();
                tbType.Text = vmCreate.createModel.MotorType.ToString();
                tbFrame.Text = vmCreate.createModel.Frame.ToString();
                tbPhase.Text = vmCreate.createModel.Phase.ToString();
                tbMotorRatedKw.Text = vmCreate.createModel.MotorRatedKw.ToString();
                tbMotorRatedHP.Text = vmCreate.createModel.MotorRatedHP.ToString();
                tbMotorRatedVoltage.Text = vmCreate.createModel.MotorRatedVoltage.ToString();
                tbHERTZ.Text = vmCreate.createModel.MotorRatedFrequency.ToString();
                tbAMPS.Text = vmCreate.createModel.MotorRatedCurrent.ToString();
                tbPF.Text = vmCreate.createModel.MotorRatedPowerFactor.ToString();
                tbRPM.Text = vmCreate.createModel.MotorRatedRPM.ToString();
                tbPole.Text = vmCreate.createModel.NofPoles.ToString();
                tbEfficency.Text = vmCreate.createModel.Efficency.ToString();
                tbDuty_SerivceFactor.Text = vmCreate.createModel.Duty_SerivceFactor.ToString();
                tbInsClass.Text = vmCreate.createModel.InsulationClass.ToString();
                tbIC_CoolingClass.Text = vmCreate.createModel.CoolingClass.ToString();
                tbIPRating.Text = vmCreate.createModel.IPRating.ToString();
                tbConnectionType.Text = vmCreate.createModel.ConnectionType.ToString();
                tbSerialNo.Text = vmCreate.createModel.SerialNo.ToString();
                tbSpecifiedTemperature.Text = vmCreate.createModel.SpecifiedTemperature.ToString();
                tbWindingResistanceinOhm.Text = vmCreate.createModel.WindingResistanceinOhm.ToString();
                tbTempAtWindingResistanceIsMeasured.Text = vmCreate.createModel.TempAtWindingResistanceIsMeasured.ToString();
                rbDescription.Text = vmCreate.createModel.rbDescription.ToString();
                cbRatedCurves.Checked = vmCreate.createModel.RatedCurves;

                if (cbRatedCurves.Checked == true)
                {

                    #region ShaftPower
                    for (int i = 0; i < vmCreate.lstShaftPower.Count; i++)
                    {
                        var model = vmCreate.lstShaftPower[i];

                        // Check if the text box contains a valid integer
                        var rowValue = model.Row;
                        if (rowValue.HasValue)
                        {
                            if (model.RowNo == 1)
                                tbShaftPower_P2_in_kW1.Text = rowValue.ToString();
                            if (model.RowNo == 2)
                                tbShaftPower_P2_in_kW2.Text = rowValue.ToString();
                            if (model.RowNo == 3)
                                tbShaftPower_P2_in_kW3.Text = rowValue.ToString();
                            if (model.RowNo == 4)
                                tbShaftPower_P2_in_kW4.Text = rowValue.ToString();
                            if (model.RowNo == 5)
                                tbShaftPower_P2_in_kW5.Text = rowValue.ToString();
                            if (model.RowNo == 6)
                                tbShaftPower_P2_in_kW6.Text = rowValue.ToString();
                            if (model.RowNo == 7)
                                tbShaftPower_P2_in_kW7.Text = rowValue.ToString();
                        }
                    }
                    #endregion
                    #region lstEfficiency
                    for (int i = 0; i < vmCreate.lstEfficiency.Count; i++)
                    {
                        var model = vmCreate.lstEfficiency[i];

                        // Check if the text box contains a valid integer
                        var rowValue = model.Row;
                        if (rowValue.HasValue)
                        {
                            if (model.RowNo == 1)
                                tbEfficiency1.Text = rowValue.ToString();
                            if (model.RowNo == 2)
                                tbEfficiency2.Text = rowValue.ToString();
                            if (model.RowNo == 3)
                                tbEfficiency3.Text = rowValue.ToString();
                            if (model.RowNo == 4)
                                tbEfficiency4.Text = rowValue.ToString();
                            if (model.RowNo == 5)
                                tbEfficiency5.Text = rowValue.ToString();
                            if (model.RowNo == 6)
                                tbEfficiency6.Text = rowValue.ToString();
                            if (model.RowNo == 7)
                                tbEfficiency7.Text = rowValue.ToString();
                        }
                    }
                    #endregion
                    #region lstCurrentInAmps
                    for (int i = 0; i < vmCreate.lstCurrentInAmps.Count; i++)
                    {
                        var model = vmCreate.lstCurrentInAmps[i];

                        // Check if the text box contains a valid integer
                        var rowValue = model.Row;
                        if (rowValue.HasValue)
                        {
                            if (model.RowNo == 1)
                                tbCurrentInAmps1.Text = rowValue.ToString();
                            if (model.RowNo == 2)
                                tbCurrentInAmps2.Text = rowValue.ToString();
                            if (model.RowNo == 3)
                                tbCurrentInAmps3.Text = rowValue.ToString();
                            if (model.RowNo == 4)
                                tbCurrentInAmps4.Text = rowValue.ToString();
                            if (model.RowNo == 5)
                                tbCurrentInAmps5.Text = rowValue.ToString();
                            if (model.RowNo == 6)
                                tbCurrentInAmps6.Text = rowValue.ToString();
                            if (model.RowNo == 7)
                                tbCurrentInAmps7.Text = rowValue.ToString();
                        }
                    }
                    #endregion
                    #region lstSpeedInRPM
                    for (int i = 0; i < vmCreate.lstSpeedInRPM.Count; i++)
                    {
                        var model = vmCreate.lstSpeedInRPM[i];

                        // Check if the text box contains a valid integer
                        var rowValue = model.Row;
                        if (rowValue.HasValue)
                        {
                            if (model.RowNo == 1)
                                tbSpeedinRPM1.Text = rowValue.ToString();
                            if (model.RowNo == 2)
                                tbSpeedinRPM2.Text = rowValue.ToString();
                            if (model.RowNo == 3)
                                tbSpeedinRPM3.Text = rowValue.ToString();
                            if (model.RowNo == 4)
                                tbSpeedinRPM4.Text = rowValue.ToString();
                            if (model.RowNo == 5)
                                tbSpeedinRPM5.Text = rowValue.ToString();
                            if (model.RowNo == 6)
                                tbSpeedinRPM6.Text = rowValue.ToString();
                            if (model.RowNo == 7)
                                tbSpeedinRPM7.Text = rowValue.ToString();
                        }
                    }
                    #endregion
                    #region lstCos
                    for (int i = 0; i < vmCreate.lstCos.Count; i++)
                    {
                        var model = vmCreate.lstCos[i];

                        // Check if the text box contains a valid integer
                        var rowValue = model.Row;
                        if (rowValue.HasValue)
                        {
                            if (model.RowNo == 1)
                                tbCos1.Text = rowValue.ToString();
                            if (model.RowNo == 2)
                                tbCos2.Text = rowValue.ToString();
                            if (model.RowNo == 3)
                                tbCos3.Text = rowValue.ToString();
                            if (model.RowNo == 4)
                                tbCos4.Text = rowValue.ToString();
                            if (model.RowNo == 5)
                                tbCos5.Text = rowValue.ToString();
                            if (model.RowNo == 6)
                                tbCos6.Text = rowValue.ToString();
                            if (model.RowNo == 7)
                                tbCos7.Text = rowValue.ToString();
                        }
                    }
                    #endregion
                }


                #region Image
                //if (!string.IsNullOrEmpty(vmCreate.imageObj) && File.Exists(vmCreate.imageObj))
                //{

                //}
                #endregion
                pictureBox.Image = ImageClass.GetImageFromBase64(vmCreate.imageObj.Image.ToString());
                pictureBoxLogo.Image = ImageClass.GetImageFromBase64(vmCreate.imageObjIsLog.Image.ToString());
                //gridView();
            }
            catch (Exception ex)
            {
                JIMessageBox.ErrorMessage("Record Not Get,  Error: " + ex.Message);
            }

        }

        private void gridView()
        {
            // new GetListBL().GetMotorListBL(dataGridViewCreate);
        }

        private void buttonSelectImage_Click(object sender, EventArgs e)
        {
            pictureBox = ImageDialog(pictureBox);
        }

        private PictureBox ImageDialog(PictureBox pictureBox0)
        {
            //PictureBox pictureBox0 = new PictureBox();
            try
            {
                OpenFileDialog openFileDialogSelectPicture = new OpenFileDialog();
                openFileDialogSelectPicture.FileName = "";
                openFileDialogSelectPicture.Filter = "Image Files(*.JPG;*.PNG;*.BMP)|*.JPG;*.PNG;*.BMP";
                openFileDialogSelectPicture.FilterIndex = 2;
                openFileDialogSelectPicture.RestoreDirectory = true;
                if (openFileDialogSelectPicture.ShowDialog() == DialogResult.OK)
                {
                    pictureBox0.ImageLocation = openFileDialogSelectPicture.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return pictureBox0;

        }

        private void cbRatedCurves_Click(object sender, EventArgs e)
        {

        }

        private void cbRatedCurves_CheckedChanged_1(object sender, EventArgs e)
        {
            if (cbRatedCurves.Checked == true)
            {
                RatedCurvesPanel.Visible = true;
            }
            else
            {
                RatedCurvesPanel.Visible = false;
            }
        }

        private void tbReportNo_Leave(object sender, EventArgs e)
        {
            IsReportNoExistinDb(tbReportNo.Text);
        }

        private void IsReportNoExistinDb(string ReportNo)
        {
            bool IsExist = new GetListBL().IsReportNoExistinDbBL(ReportNo.Trim());
            if (IsExist == true)
            {
                JIMessageBox.WarningMessage(ReportNo +" is already exists !");
                return;
            }
        }

        private void btnSelectLogo_Click(object sender, EventArgs e)
        {
            pictureBoxLogo = ImageDialog(pictureBoxLogo);
        }
    }
}
