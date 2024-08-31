using Dapper;
using KomaxApp.Model.Create;
using KomaxApp.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using komaxApp.Utility.ExtensionMethod;
using System.Data.SqlClient;
using Newtonsoft.Json;
using KomaxApp.Model.LoadTest;
using static System.Security.Cryptography.ECCurve;

namespace komaxApp.DatabaseLayer
{
    public class InsertDL
    {
        private IDbConnection db = DbConnections.getCon();
        CreateModel.Response response = null;

        public CreateModel.Response InsertRecords(VmCreateMotor rqeuest, string IsEdit)
        {
            string json = JsonConvert.SerializeObject(rqeuest);
            using (var scope = new TransactionScope())
            {
                response = new CreateModel.Response();
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@ReportNo", rqeuest.createModel.ReportNo);
                    parameters.Add("@TestDate", rqeuest.createModel.TestDate);
                    parameters.Add("@HitachiCruve", rqeuest.createModel.HitachiCruve);
                    parameters.Add("@Manufacturer", rqeuest.createModel.Manufacturer);
                    parameters.Add("@MotorModel", rqeuest.createModel.MotorModel);
                    parameters.Add("@MotorType", rqeuest.createModel.MotorType);
                    parameters.Add("@Frame", rqeuest.createModel.Frame);
                    parameters.Add("@Phase", rqeuest.createModel.Phase);
                    parameters.Add("@MotorRatedKw", rqeuest.createModel.MotorRatedKw);
                    parameters.Add("@MotorRatedHP", rqeuest.createModel.MotorRatedHP);
                    parameters.Add("@MotorRatedVoltage", rqeuest.createModel.MotorRatedVoltage);
                    parameters.Add("@HERTZ", rqeuest.createModel.MotorRatedFrequency);
                    parameters.Add("@AMPS", rqeuest.createModel.MotorRatedCurrent);
                    parameters.Add("@PF", rqeuest.createModel.MotorRatedPowerFactor);
                    parameters.Add("@RPM", rqeuest.createModel.MotorRatedRPM);
                    parameters.Add("@Pole", rqeuest.createModel.NofPoles);
                    parameters.Add("@Efficency", rqeuest.createModel.Efficency);
                    parameters.Add("@Duty_SerivceFactor", rqeuest.createModel.Duty_SerivceFactor);
                    parameters.Add("@InsulationClass", rqeuest.createModel.InsulationClass);
                    parameters.Add("@CoolingClass", rqeuest.createModel.CoolingClass);
                    parameters.Add("@IPRating", rqeuest.createModel.IPRating);
                    parameters.Add("@ConnectionType", rqeuest.createModel.ConnectionType);
                    parameters.Add("@SerialNo", rqeuest.createModel.SerialNo);
                    // parameters.Add("@Picture", rqeuest.createModel.Picture);
                    parameters.Add("@SpecifiedTemperature", rqeuest.createModel.SpecifiedTemperature);
                    parameters.Add("@WindingResistanceinOhm", rqeuest.createModel.WindingResistanceinOhm);
                    parameters.Add("@TempAtWindingResistanceIsMeasured", rqeuest.createModel.TempAtWindingResistanceIsMeasured);
                    parameters.Add("@rbDescription", rqeuest.createModel.rbDescription);
                    parameters.Add("@RatedCurves", rqeuest.createModel.RatedCurves);
                    parameters.Add("@EntryDateTime", DateTime.Now);

                    if (IsEdit == "Edit")
                    {
                        response.StatusCode = db.Execute("sp_UpdateRecords", parameters, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        response.StatusCode = db.Execute("sp_InsertRecords", parameters, commandType: CommandType.StoredProcedure);
                    }

                    if (rqeuest.createModel.RatedCurves)
                    {
                        if (rqeuest.lstShaftPower.Count > 0 && rqeuest.lstShaftPower != null)
                        {
                            string sql1 = null;
                            if (IsEdit == "Edit")
                            {
                                sql1 = "UPDATE tblShaftPawer SET Row=@Row, RowNo=@RowNo WHERE RowNo = @RowNo";
                            }
                            else
                            {
                                sql1 = "INSERT INTO tblShaftPawer (ReportNo,Row,RowNo,EntryDateTime) " +
                                         "VALUES (@ReportNo,@Row,@RowNo,@EntryDateTime)";
                            }
                            db.Execute(sql1, rqeuest.lstShaftPower);
                        }


                        if (rqeuest.lstEfficiency.Count > 0 && rqeuest.lstEfficiency != null)
                        {
                            string sql3 = null;
                            if (IsEdit == "Edit")
                            {
                                sql3 = "UPDATE tblEfficiency SET [Row]=@Row, RowNo=@RowNo WHERE RowNo = @RowNo";
                            }
                            else
                            {
                                sql3 = "INSERT INTO tblEfficiency (ReportNo,Row,RowNo,EntryDateTime) " +
                                         "VALUES (@ReportNo,@Row,@RowNo,@EntryDateTime)";
                                db.Execute(sql3, rqeuest.lstEfficiency);
                            }
                        }
                        if (rqeuest.lstCurrentInAmps.Count > 0 && rqeuest.lstCurrentInAmps != null)
                        {
                            string sql4 = null;
                            if (IsEdit == "Edit")
                            {
                                sql4 = "UPDATE tblEfficiency SET [Row]=@Row, RowNo=@RowNo WHERE RowNo = @RowNo";
                            }
                            else
                            {
                                 sql4 = "INSERT INTO tblCurrentAmps (ReportNo,Row,RowNo,EntryDateTime) " +
                                             "VALUES (@ReportNo,@Row,@RowNo,@EntryDateTime)";
                            }
                            db.Execute(sql4, rqeuest.lstCurrentInAmps);
                        }
                        if (rqeuest.lstSpeedInRPM.Count > 0 && rqeuest.lstSpeedInRPM != null)
                        {
                            string sql5 = null;
                            if (IsEdit == "Edit")
                            {
                                sql5 = "UPDATE tblEfficiency SET [Row]=@Row, RowNo=@RowNo WHERE RowNo = @RowNo";
                            }
                            else
                            {
                                sql5 = "INSERT INTO tblSpeedRPM (ReportNo,Row,RowNo,EntryDateTime) " +
                                             "VALUES (@ReportNo,@Row,@RowNo,@EntryDateTime)";
                            }
                            db.Execute(sql5, rqeuest.lstSpeedInRPM);
                        }

                        if (rqeuest.lstCos.Count > 0 && rqeuest.lstCos != null)
                        {
                            string sql6 = null;
                            if (IsEdit == "Edit")
                            {
                                sql6 = "UPDATE tblCos SET [Row]=@Row, RowNo=@RowNo WHERE RowNo = @RowNo";
                            }
                            else
                            {
                                sql6 = "INSERT INTO tblCos (ReportNo,Row,RowNo,EntryDateTime) " +
                                             "VALUES (@ReportNo,@Row,@RowNo,@EntryDateTime)";
                            }
                            db.Execute(sql6, rqeuest.lstCos);
                        }

                    }
                    if (IsEdit == "Edit")
                    {
                        string sql0 = "UPDATE tblImages SET Image=@Image WHERE ReportNo = @ReportNo";
                        db.Execute(sql0, rqeuest.imageObj);
                    }
                    else
                    {
                        string sql0 = "INSERT INTO tblImages (ReportNo,Image,EntryDateTime) " +
                                           "VALUES (@ReportNo,@Image,@EntryDateTime)";
                        db.Execute(sql0, rqeuest.imageObj);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new Exception("DL Exception: " + ex.Message);
                }
            }
            return response;
        }

        public RequestLoadTestModel.Response InsertRecordNoLoadPointDL(RequestLoadTestModel.Request rqeuest)
        {
            RequestLoadTestModel.Response response = new RequestLoadTestModel.Response();
            using (var scope = new TransactionScope())
            {
                response = new RequestLoadTestModel.Response();
                try
                {
                    rqeuest.EntryDate = DateTime.Now;
                    (response.StatusCode, response.StatusDetails, response.LabelCount, response.LabelStatus) = Repo.ExecuteStoredProcedureWithOutputs(rqeuest);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new Exception("DL Exception: " + ex.Message);
                }
            }
            return response;
        }


        public void InsertComboPortsDL(string _cbPowerMeter, string _cbTorqueMeter, string _cbRPM, string _cbTemperature)
        {
            try
            {
                // Define the stored procedure name
                var storedProcedure = "sp_InsertComboPorts";

                // Execute the stored procedure using Dapper
                db.Execute(storedProcedure, new
                {
                    PowerMeterPort = _cbPowerMeter,
                    TorqueMeterPort = _cbTorqueMeter,
                    RPMPort = _cbRPM,
                    TemperaturePort = _cbTemperature
                }, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                // Handle any exceptions (logging, displaying a message, etc.)
                Console.WriteLine("Error inserting data: " + ex.Message);
            }
        }

    }
}
