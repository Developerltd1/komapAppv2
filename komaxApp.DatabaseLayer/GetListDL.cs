using Dapper;
using komaxApp.Utility.ExtensionMethod;
using KomaxApp.Model.Create;
using KomaxApp.Model.Display;
using KomaxApp.Model.LoadTest;
using KomaxApp.Model.Page1;
using KomaxApp.Model.Reporting;
using KomaxApp.Model.Reporting.Model.Page2;
using KomaxApp.Model.Reporting.ViewModel;
using KomaxApp.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KomaxApp.Model.Page1.StructureMulti;
using KomaxApp.Model.Page1.Entity;
using System.Reflection;
using System.Diagnostics.Contracts;
using KomaxApp.Model.Reporting.Model.Page1.Structure;
using static KomaxApp.Model.Reporting.ReportModel;
using KomaxApp.Model.Reporting.Model.Page1;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Policy;
using KomaxApp.Model.Reporting.Model.Page2.Entity;
using KomaxApp.Model.Reporting.Model.Page3.Entity;
using KomaxApp.Model.Reporting.Model.Page3;
using KomaxApp.Model.Reporting.Model.Page4And5;
using KomaxApp.Model.Reporting.Model.Page4And5.Charts;
using System.IO.Ports;

namespace komaxApp.DatabaseLayer
{
    public class GetListDL
    {
        private IDbConnection db = DbConnections.getCon();



        public async Task<VmCreateMotor> GetDatausingReportDL(string reportNo)
        {
            var response = new VmCreateMotor();

            try
            {
                using (var db = new SqlConnection(DbConnections.getCon().ConnectionString)) // Use your connection string
                {
                    await db.OpenAsync();

                    var multi = await db.QueryMultipleAsync("GetMotorAndBatteryData", new { ReportNo = reportNo }, commandType: CommandType.StoredProcedure);

                    // Read the results
                    response.createModel = (await multi.ReadAsync<CreateModel.Request>()).FirstOrDefault();
                    response.lstShaftPower = (await multi.ReadAsync<ShaftPowerObj.Request>()).ToList();
                    response.lstSpeedInRPM = (await multi.ReadAsync<SpeedInRPMObj.Request>()).ToList();
                    response.lstEfficiency = (await multi.ReadAsync<EfficiencyObj.Request>()).ToList();
                    response.lstCurrentInAmps = (await multi.ReadAsync<CurrentInAmpsObj.Request>()).ToList();
                    response.lstCos = (await multi.ReadAsync<CosObj.Request>()).ToList();
                    response.imageObj = (await multi.ReadAsync<ImageObj.Request>()).FirstOrDefault();
                    // Optionally, log or process data
                    var count = response.lstShaftPower.Count; // Example: Check the count
                }
            }
            catch (SqlException sqlEx)
            {
                // Handle SQL exceptions
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error: {ex.Message}");
            }

            return response;
        }
        public async Task<ReportModel> GetDataForReportDL(string reportNo)
        {
            var response = new ReportModel();
            Page1 page1 = new Page1();
            try
            {
                using (var db = new SqlConnection(DbConnections.getCon().ConnectionString)) // Use your connection string
                {
                    await db.OpenAsync();

                    var multi = await db.QueryMultipleAsync("GetReportDataByReportId0", new { ReportNo = reportNo }, commandType: CommandType.StoredProcedure);


                    List<Page1> page1Lst = (await multi.ReadAsync<Page1>()).ToList();
                    response.page1 = page1Lst.FirstOrDefault();

                    #region ShaftPowerAll
                    foreach (var item in page1Lst)
                    {
                        if (item.ShaftPowerAll != null && item.ShaftRowNo == 1)
                        {
                            response.page1.ShaftPower1 = item.ShaftPowerAll;
                        }
                        if (item.ShaftPowerAll != null && item.ShaftRowNo == 2)
                        {
                            response.page1.ShaftPower2 = item.ShaftPowerAll;
                        }
                        if (item.ShaftPowerAll != null && item.ShaftRowNo == 3)
                        {
                            response.page1.ShaftPower3 = item.ShaftPowerAll;
                        }
                        if (item.ShaftPowerAll != null && item.ShaftRowNo == 4)
                        {
                            response.page1.ShaftPower4 = item.ShaftPowerAll;
                        }
                        if (item.ShaftPowerAll != null && item.ShaftRowNo == 5)
                        {
                            response.page1.ShaftPower5 = item.ShaftPowerAll;
                        }
                        if (item.ShaftPowerAll != null && item.ShaftRowNo == 6)
                        {
                            response.page1.ShaftPower6 = item.ShaftPowerAll;
                        }
                        if (item.ShaftPowerAll != null && item.ShaftRowNo == 7)
                        {
                            response.page1.ShaftPower7 = item.ShaftPowerAll;
                        }
                    }
                    #endregion
                    #region EfficiencAll
                    foreach (var item in page1Lst)
                    {
                        if (item.EfficiencAll != null && item.EfficiencRowNo == 1)
                        {
                            response.page1.Efficiency1 = item.EfficiencAll;
                        }
                        if (item.EfficiencAll != null && item.EfficiencRowNo == 2)
                        {
                            response.page1.Efficiency2 = item.EfficiencAll;
                        }
                        if (item.EfficiencAll != null && item.EfficiencRowNo == 3)
                        {
                            response.page1.Efficiency3 = item.EfficiencAll;
                        }
                        if (item.EfficiencAll != null && item.EfficiencRowNo == 4)
                        {
                            response.page1.Efficiency4 = item.EfficiencAll;
                        }
                        if (item.EfficiencAll != null && item.EfficiencRowNo == 5)
                        {
                            response.page1.Efficiency5 = item.EfficiencAll;
                        }
                        if (item.EfficiencAll != null && item.EfficiencRowNo == 6)
                        {
                            response.page1.Efficiency6 = item.EfficiencAll;
                        }
                        if (item.EfficiencAll != null && item.EfficiencRowNo == 7)
                        {
                            response.page1.Efficiency7 = item.EfficiencAll;
                        }
                    }
                    #endregion
                    #region SpeedAll
                    foreach (var item in page1Lst)
                    {
                        if (item.SpeedAll != null && item.SpeedRowNo == 1)
                        {
                            response.page1.Speed1 = item.SpeedAll;
                        }
                        if (item.SpeedAll != null && item.SpeedRowNo == 2)
                        {
                            response.page1.Speed2 = item.SpeedAll;
                        }
                        if (item.SpeedAll != null && item.SpeedRowNo == 3)
                        {
                            response.page1.Speed3 = item.SpeedAll;
                        }
                        if (item.SpeedAll != null && item.SpeedRowNo == 4)
                        {
                            response.page1.Speed4 = item.SpeedAll;
                        }
                        if (item.SpeedAll != null && item.SpeedRowNo == 5)
                        {
                            response.page1.Speed5 = item.SpeedAll;
                        }
                        if (item.SpeedAll != null && item.SpeedRowNo == 6)
                        {
                            response.page1.Speed6 = item.SpeedAll;
                        }
                        if (item.SpeedAll != null && item.SpeedRowNo == 7)
                        {
                            response.page1.Speed7 = item.SpeedAll;
                        }
                    }
                    #endregion
                    #region CurrentInAmpsAll
                    foreach (var item in page1Lst)
                    {
                        if (item.CurrentInAmpsAll != null && item.CurrentInAmpsRowNo == 1)
                        {
                            response.page1.CurrentInAmps1 = item.CurrentInAmpsAll;
                        }
                        if (item.CurrentInAmpsAll != null && item.CurrentInAmpsRowNo == 2)
                        {
                            response.page1.CurrentInAmps2 = item.CurrentInAmpsAll;
                        }
                        if (item.CurrentInAmpsAll != null && item.CurrentInAmpsRowNo == 3)
                        {
                            response.page1.CurrentInAmps3 = item.CurrentInAmpsAll;
                        }
                        if (item.CurrentInAmpsAll != null && item.CurrentInAmpsRowNo == 4)
                        {
                            response.page1.CurrentInAmps4 = item.CurrentInAmpsAll;
                        }
                        if (item.CurrentInAmpsAll != null && item.CurrentInAmpsRowNo == 5)
                        {
                            response.page1.CurrentInAmps5 = item.CurrentInAmpsAll;
                        }
                        if (item.CurrentInAmpsAll != null && item.CurrentInAmpsRowNo == 6)
                        {
                            response.page1.CurrentInAmps6 = item.CurrentInAmpsAll;
                        }
                        if (item.CurrentInAmpsAll != null && item.CurrentInAmpsRowNo == 7)
                        {
                            response.page1.CurrentInAmps7 = item.CurrentInAmpsAll;
                        }
                    }
                    #endregion
                    #region CosAll
                    foreach (var item in page1Lst)
                    {
                        if (item.CosAll != null && item.CosRowNo == 1)
                        {
                            response.page1.Cos1 = item.CosAll;
                        }
                        if (item.CosAll != null && item.CosRowNo == 2)
                        {
                            response.page1.Cos2 = item.CosAll;
                        }
                        if (item.CosAll != null && item.CosRowNo == 3)
                        {
                            response.page1.Cos3 = item.CosAll;
                        }
                        if (item.CosAll != null && item.CosRowNo == 4)
                        {
                            response.page1.Cos4 = item.CosAll;
                        }
                        if (item.CosAll != null && item.CosRowNo == 5)
                        {
                            response.page1.Cos5 = item.CosAll;
                        }
                        if (item.CosAll != null && item.CosRowNo == 6)
                        {
                            response.page1.Cos6 = item.CosAll;
                        }
                        if (item.CosAll != null && item.CosRowNo == 7)
                        {
                            response.page1.Cos7 = item.CosAll;
                        }
                    }
                    #endregion



                    #region loopfor
                    //foreach (var item in page1Lst)
                    //{
                    //    if (item.ShaftPower1 != null)
                    //    {
                    //        response.page1.ShaftPower1 = item.ShaftPower1; 
                    //    }                                                  
                    //    if (item.ShaftPower2 != null)                      
                    //    {                                                  
                    //        response.page1.ShaftPower2 = item.ShaftPower2; 
                    //    }                                                  
                    //    if (item.ShaftPower3 != null)                      
                    //    {                                                  
                    //        response.page1.ShaftPower3 = item.ShaftPower3; 
                    //    }                                                  
                    //    if (item.ShaftPower4 != null)                      
                    //    {                                                  
                    //        response.page1.ShaftPower4 = item.ShaftPower4; 
                    //    }                                                  
                    //    if (item.ShaftPower5 != null)                      
                    //    {                                                  
                    //        response.page1.ShaftPower5 = item.ShaftPower5; 
                    //    }                                                  
                    //    if (item.ShaftPower6 != null)                      
                    //    {                                                  
                    //        response.page1.ShaftPower6 = item.ShaftPower6; 
                    //    }                                                  
                    //    if (item.ShaftPower7 != null)                      
                    //    {                                                  
                    //        response.page1.ShaftPower7 = item.ShaftPower7; 
                    //    }
                    //}
                    #endregion




                    response.page2 = (await multi.ReadAsync<Page2>()).FirstOrDefault();
                    response.page3 = (await multi.ReadAsync<Page3>()).FirstOrDefault();
                    response.page4Charts = (await multi.ReadAsync<Page4Charts>()).FirstOrDefault();
                }
            }
            catch (SqlException sqlEx)
            {
                // Handle SQL exceptions
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error: {ex.Message}");
            }


            return response;
        }


        public async Task<ReportingModel> GetDataForReportDL0(string reportNo)
        {
            ReportingModel reportingModel = new ReportingModel();

            try
            {
                using (var db = new SqlConnection(DbConnections.getCon().ConnectionString)) // Use your connection string
                {
                    //await db.OpenAsync();

                    var multi = await db.QueryMultipleAsync("GetReportDataByReportId0", new { ReportNo = reportNo }, commandType: CommandType.StoredProcedure);

                    //Page1
                    tblMotor _tblMotor = (await multi.ReadAsync<tblMotor>()).FirstOrDefault();
                    List<tblShaftPawer> tblShaftPawer = (await multi.ReadAsync<tblShaftPawer>()).ToList();
                    List<tblEfficiency> tblEfficiency = (await multi.ReadAsync<tblEfficiency>()).ToList();
                    List<tblSpeedRPM> tblSpeedRPM = (await multi.ReadAsync<tblSpeedRPM>()).ToList();
                    List<tblCurrentAmps> tblCurrentAmps = (await multi.ReadAsync<tblCurrentAmps>()).ToList();
                    List<tblCos> tblCos = (await multi.ReadAsync<tblCos>()).ToList();
                    tblImages tblImages = (await multi.ReadAsync<tblImages>()).FirstOrDefault();



                    reportingModel.page1Mdl = ReportingPage1(_tblMotor, tblImages, tblShaftPawer, tblEfficiency, tblSpeedRPM, tblCurrentAmps, tblCos);
                    //Page2
                    tblEntity _tblEntity = (await multi.ReadAsync<tblEntity>()).FirstOrDefault();
                    reportingModel.page2Mdl = ReportingPage2(_tblEntity, _tblMotor);
                    //Page3
                    tblMotorModify _tblMotorModify = (await multi.ReadAsync<tblMotorModify>()).FirstOrDefault();

                    List<KomaxApp.Model.Reporting.Model.Page3.Entity.tblLoadTest> _tblLoadTest = (await multi.ReadAsync<KomaxApp.Model.Reporting.Model.Page3.Entity.tblLoadTest>()).ToList();
                    reportingModel.page3Mdl = ReportingPage3(_tblMotorModify, _tblLoadTest, _tblMotor);
                    //Page4And5
                    reportingModel.page4And5Mdl = ReportingPage4And5(reportingModel.page1Mdl);

                    //Charts
                    //reportingModel._Chart1Model = tblChart1;    //ReportingCharts1(tblShaftPawer, tblEfficiency);

                }

                using (var db = new SqlConnection(DbConnections.getCon().ConnectionString)) // Use your connection string
                {
                    var multi = await db.QueryMultipleAsync("GetReportCharts", new { ReportNo = reportNo }, commandType: CommandType.StoredProcedure);
                    reportingModel._Chart1Model = (await multi.ReadAsync<Chart1Model>()).ToList();


                }
            }
            catch (SqlException sqlEx)
            {
                // Handle SQL exceptions
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"Error: {ex.Message}");
            }

            return reportingModel;
        }

        private List<Chart1Model> ReportingCharts1(List<tblShaftPawer> tblShaftPawer, List<tblEfficiency> tblEfficiency)
        {
            List<Chart1Model> chart1Model = new List<Chart1Model>();
            #region Manual
            #region Shaft
            // foreach(var item in tblShaftPawer)
            //   {
            //       if (item.Row != null)
            //       {
            //           switch (item.RowNo)
            //           {
            //               case 1:
            //                   chart1Model.Add(new Chart1Model());
            //                   chart1Model[0].ShaftPawerRow = item.Row;
            //                   break;
            //               case 2:
            //                   chart1Model.Add(new Chart1Model());
            //                   chart1Model[1].ShaftPawerRow = item.Row;
            //                   break;
            //               case 3:
            //                   chart1Model.Add(new Chart1Model());
            //                   chart1Model[2].ShaftPawerRow = item.Row;
            //                   break;
            //               case 4:
            //                   chart1Model.Add(new Chart1Model());
            //                   chart1Model[3].ShaftPawerRow = item.Row;
            //                   break;
            //               case 5:
            //                   chart1Model.Add(new Chart1Model());
            //                   chart1Model[4].ShaftPawerRow = item.Row;
            //                   break;
            //               case 6:
            //                   chart1Model.Add(new Chart1Model());
            //                   chart1Model[5].ShaftPawerRow = item.Row;
            //                   break;
            //               case 7:
            //                   chart1Model.Add(new Chart1Model());
            //                   chart1Model[6].ShaftPawerRow = item.Row;
            //                   break;
            //           }
            //       }
            //   }
            #endregion
            #region Eff
            //foreach (var item in tblEfficiency)
            //{
            //    if (item.Row != null)
            //    {
            //        switch (item.RowNo)
            //        {
            //            case 1:

            //                chart1Model[0].EfficiencyRow = item.Row;
            //                break;
            //            case 2:
            //                chart1Model[1].EfficiencyRow = item.Row;
            //                break;
            //            case 3:
            //                chart1Model[2].EfficiencyRow = item.Row;
            //                break;
            //            case 4:
            //                chart1Model[3].EfficiencyRow = item.Row;
            //                break;
            //            case 5:
            //                chart1Model[4].EfficiencyRow = item.Row;
            //                break;
            //            case 6:
            //                chart1Model[5].EfficiencyRow = item.Row;
            //                break;
            //            case 7:
            //                chart1Model[6].EfficiencyRow = item.Row;
            //                break;
            //        }
            //    }
            //}
            #endregion
            #endregion

            return chart1Model;
        }

        private Page4And5ModelFinilize ReportingPage4And5(Page1ModelFinilize page1ModelFinilize)
        {
            Page4And5ModelFinilize page4And5Model = new Page4And5ModelFinilize();
            new Conversion().CopyProperties(page1ModelFinilize, page4And5Model);
            return page4And5Model;

        }

        //public Page4And5ModelFinilize ReportingPage4And5(tblMotor tblMotor, List<tblShaftPawer> _tblShaftPawer, List<tblEfficiency> tblEfficiency, List<tblSpeedRPM> tblSpeedRPM, List<tblCurrentAmps> tblCurrentAmps, List<tblCos> tblCos)
        //{
        //    Page4And5ModelFinilize page4And5Model = new Page4And5ModelFinilize();
        //    new Conversion().CopyProperties(tblMotor, page4And5Model);

        //    List<KomaxApp.Model.Page4And5.Entity.tblShaftPawer> clonedShaftPawer = _tblShaftPawer
        //        .Select(item => new KomaxApp.Model.Page4And5.Entity.tblShaftPawer
        //        {
        //            Row = item.Row,
        //            RowNo = item.RowNo
        //        })
        //        .ToList();
        //    page4And5Model._tblShaftPawer = clonedShaftPawer;

        //    List<KomaxApp.Model.Page4And5.Entity.tblEfficiency> clonedEfficiency = tblEfficiency
        //    .Select(item => new KomaxApp.Model.Page4And5.Entity.tblEfficiency
        //    {
        //        Row = item.Row,
        //        RowNo = item.RowNo
        //    })
        //    .ToList();
        //    page4And5Model._tblEfficiency = clonedEfficiency;

        //    List<KomaxApp.Model.Page4And5.Entity.tblSpeedRPM> clonedSpeedRPM = tblSpeedRPM
        //     .Select(item => new KomaxApp.Model.Page4And5.Entity.tblSpeedRPM
        //     {
        //         Row = item.Row,
        //         RowNo = item.RowNo
        //     })
        //     .ToList();
        //    page4And5Model._tblSpeedRPM = clonedSpeedRPM;

        //    List<KomaxApp.Model.Page4And5.Entity.tblCurrentAmps> clonedCurrentAmps = tblCurrentAmps
        //    .Select(item => new KomaxApp.Model.Page4And5.Entity.tblCurrentAmps
        //    {
        //        Row = item.Row,
        //        RowNo = item.RowNo
        //    })
        //    .ToList();
        //    page4And5Model._tblCurrentAmps = clonedCurrentAmps;

        //    List<KomaxApp.Model.Page4And5.Entity.tblCos> clonedCos = tblCos
        //    .Select(item => new KomaxApp.Model.Page4And5.Entity.tblCos
        //    {
        //        Row = item.Row,
        //        RowNo = item.RowNo
        //    })
        //    .ToList();
        //    page4And5Model._tblCos = clonedCos;

        //    return page4And5Model;
        //}

        private Page3ModelFinilize ReportingPage3(tblMotorModify tblMotorModify, List<KomaxApp.Model.Reporting.Model.Page3.Entity.tblLoadTest> tblLoadTest, tblMotor tblMotor)
        {



            Page3ModelFinilize page3ModelFinilize = new Page3ModelFinilize();
            Page3Model page3Model = new Page3Model();



            page3Model.TestReportNo = tblMotor.TestReportNo;
            page3Model.MotorModel = tblMotor.MotorModel;
            page3Model.Dated = tblMotor.Dated;
            page3Model.SerialNo = tblMotor.SerialNo;
            //A1 Specified temperature, ts, in °C
            page3Model.SpecifiedtemperatureTsInC = tblMotorModify.SpecifiedtemperatureTsInC;
            //B2 Stator Resistance (Cold), in Ohms
            page3Model.StatorResistanceColdInOhms = tblMotorModify.StatorResistanceColdInOhms;
            //C3 Stator Resistance (Cold) measure at Temp, in °C
            page3Model.StatorResistanceColdMeasureAtTempInC = tblMotorModify.StatorResistanceColdMeasureAtTempInC;

            //D4 Stator Winding Temp, tt in °C   From => LoadTest -> PT100[tbtemp1]
            foreach (var item in tblLoadTest)
            {
                if (item.Pt100_Temp1 != null)
                {
                    switch (item.LabelCount)
                    {
                        case 0:
                            page3Model.StatorWindingTemp1 = item.Pt100_Temp1.ToString();
                            break;
                        case 25:
                            page3Model.StatorWindingTemp2 = item.Pt100_Temp1.ToString();
                            break;
                        case 50:
                            page3Model.StatorWindingTemp3 = item.Pt100_Temp1.ToString();
                            break;
                        case 75:
                            page3Model.StatorWindingTemp4 = item.Pt100_Temp1.ToString();
                            break;
                        case 100:
                            page3Model.StatorWindingTemp5 = item.Pt100_Temp1.ToString();
                            break;
                        case 115:
                            page3Model.StatorWindingTemp6 = item.Pt100_Temp1.ToString();
                            break;
                        case 130:
                            page3Model.StatorWindingTemp7 = item.Pt100_Temp1.ToString();
                            break;
                    }
                }
            }

            //E5  Stator Resistance (Cold) measure at Temp, in °C    From => LoadTest -> PT100[tbtemp2]
            foreach (var item in tblLoadTest)
            {
                if (item.Pt100_Temp2 != null)
                {
                    switch (item.LabelCount)
                    {
                        case 0:
                            page3Model.StatorWindingTemp1 = item.Pt100_Temp2.ToString();
                            break;
                        case 25:
                            page3Model.StatorWindingTemp2 = item.Pt100_Temp2.ToString();
                            break;
                        case 50:
                            page3Model.StatorWindingTemp3 = item.Pt100_Temp2.ToString();
                            break;
                        case 75:
                            page3Model.StatorWindingTemp4 = item.Pt100_Temp2.ToString();
                            break;
                        case 100:
                            page3Model.StatorWindingTemp5 = item.Pt100_Temp2.ToString();
                            break;
                        case 115:
                            page3Model.StatorWindingTemp6 = item.Pt100_Temp2.ToString();
                            break;
                        case 130:
                            page3Model.StatorWindingTemp7 = item.Pt100_Temp2.ToString();
                            break;
                    }
                }
            }

            //F6 Line-to-Line Voltage, in V  =?
            foreach (var item in tblLoadTest)
            {
                if (item.VoltageV_Value1 != null)
                {
                    switch (item.LabelCount)
                    {
                        case 0:
                            page3Model.LinetoLineCol1Voltage1 = item.VoltageV_Value1.ToString();
                            page3Model.LinetoLineCol1Voltage2 = item.VoltageV_Value2.ToString();
                            page3Model.LinetoLineCol1Voltage3 = item.VoltageV_Value3.ToString();
                            page3Model.LinetoLineCol1Voltage4 = item.VoltageV_Value4.ToString();
                            break;
                        case 25:
                            page3Model.LinetoLineCol1Voltage1 = item.VoltageV_Value1.ToString();
                            page3Model.LinetoLineCol1Voltage2 = item.VoltageV_Value2.ToString();
                            page3Model.LinetoLineCol1Voltage3 = item.VoltageV_Value3.ToString();
                            page3Model.LinetoLineCol1Voltage4 = item.VoltageV_Value4.ToString();
                            break;
                        case 50:
                            page3Model.LinetoLineCol1Voltage1 = item.VoltageV_Value1.ToString();
                            page3Model.LinetoLineCol1Voltage2 = item.VoltageV_Value2.ToString();
                            page3Model.LinetoLineCol1Voltage3 = item.VoltageV_Value3.ToString();
                            page3Model.LinetoLineCol1Voltage4 = item.VoltageV_Value4.ToString();
                            break;
                        case 75:
                            page3Model.LinetoLineCol1Voltage1 = item.VoltageV_Value1.ToString();
                            page3Model.LinetoLineCol1Voltage2 = item.VoltageV_Value2.ToString();
                            page3Model.LinetoLineCol1Voltage3 = item.VoltageV_Value3.ToString();
                            page3Model.LinetoLineCol1Voltage4 = item.VoltageV_Value4.ToString();
                            break;
                        case 100:
                            page3Model.LinetoLineCol1Voltage1 = item.VoltageV_Value1.ToString();
                            page3Model.LinetoLineCol1Voltage2 = item.VoltageV_Value2.ToString();
                            page3Model.LinetoLineCol1Voltage3 = item.VoltageV_Value3.ToString();
                            page3Model.LinetoLineCol1Voltage4 = item.VoltageV_Value4.ToString();
                            break;
                        case 115:
                            page3Model.LinetoLineCol1Voltage1 = item.VoltageV_Value1.ToString();
                            page3Model.LinetoLineCol1Voltage2 = item.VoltageV_Value2.ToString();
                            page3Model.LinetoLineCol1Voltage3 = item.VoltageV_Value3.ToString();
                            page3Model.LinetoLineCol1Voltage4 = item.VoltageV_Value4.ToString();
                            break;
                        case 130:
                            page3Model.LinetoLineCol1Voltage1 = item.VoltageV_Value1.ToString();
                            page3Model.LinetoLineCol1Voltage2 = item.VoltageV_Value2.ToString();
                            page3Model.LinetoLineCol1Voltage3 = item.VoltageV_Value3.ToString();
                            page3Model.LinetoLineCol1Voltage4 = item.VoltageV_Value4.ToString();
                            break;
                    }
                }
            }
            //G7  Frequency, in Hz    LoadTestFrom =>  GridLabel -> labelHertz
            foreach (var item in tblLoadTest)
            {
                if (item.FrequencyHZ != null)
                {
                    switch (item.LabelCount)
                    {
                        case 0:
                            page3Model.FrequencyinHz1 = item.FrequencyHZ.ToString();
                            page3Model.FrequencyinHz1Col1 = item.Frequency_Value1.ToString();
                            page3Model.FrequencyinHz1Col2 = item.Frequency_Value2.ToString();
                            page3Model.FrequencyinHz1Col3 = item.Frequency_Value3.ToString();
                            page3Model.FrequencyinHz1Col4 = item.Frequency_Value4.ToString();
                            break;
                        case 25:
                            page3Model.FrequencyinHz2 = item.FrequencyHZ.ToString();
                            page3Model.FrequencyinHz1Col1 = item.Frequency_Value1.ToString();
                            page3Model.FrequencyinHz1Col2 = item.Frequency_Value2.ToString();
                            page3Model.FrequencyinHz1Col3 = item.Frequency_Value3.ToString();
                            page3Model.FrequencyinHz1Col4 = item.Frequency_Value4.ToString();
                            break;
                        case 50:
                            page3Model.FrequencyinHz3 = item.FrequencyHZ.ToString();
                            page3Model.FrequencyinHz1Col1 = item.Frequency_Value1.ToString();
                            page3Model.FrequencyinHz1Col2 = item.Frequency_Value2.ToString();
                            page3Model.FrequencyinHz1Col3 = item.Frequency_Value3.ToString();
                            page3Model.FrequencyinHz1Col4 = item.Frequency_Value4.ToString();
                            break;
                        case 75:
                            page3Model.FrequencyinHz4 = item.FrequencyHZ.ToString();
                            page3Model.FrequencyinHz1Col1 = item.Frequency_Value1.ToString();
                            page3Model.FrequencyinHz1Col2 = item.Frequency_Value2.ToString();
                            page3Model.FrequencyinHz1Col3 = item.Frequency_Value3.ToString();
                            page3Model.FrequencyinHz1Col4 = item.Frequency_Value4.ToString();
                            break;
                        case 100:
                            page3Model.FrequencyinHz5 = item.FrequencyHZ.ToString();
                            page3Model.FrequencyinHz1Col1 = item.Frequency_Value1.ToString();
                            page3Model.FrequencyinHz1Col2 = item.Frequency_Value2.ToString();
                            page3Model.FrequencyinHz1Col3 = item.Frequency_Value3.ToString();
                            page3Model.FrequencyinHz1Col4 = item.Frequency_Value4.ToString();
                            break;
                        case 115:
                            page3Model.FrequencyinHz6 = item.FrequencyHZ.ToString();
                            page3Model.FrequencyinHz1Col1 = item.Frequency_Value1.ToString();
                            page3Model.FrequencyinHz1Col2 = item.Frequency_Value2.ToString();
                            page3Model.FrequencyinHz1Col3 = item.Frequency_Value3.ToString();
                            page3Model.FrequencyinHz1Col4 = item.Frequency_Value4.ToString();
                            break;
                        case 130:
                            page3Model.FrequencyinHz7 = item.FrequencyHZ.ToString();
                            page3Model.FrequencyinHz1Col1 = item.Frequency_Value1.ToString();
                            page3Model.FrequencyinHz1Col2 = item.Frequency_Value2.ToString();
                            page3Model.FrequencyinHz1Col3 = item.Frequency_Value3.ToString();
                            page3Model.FrequencyinHz1Col4 = item.Frequency_Value4.ToString();
                            break;
                    }
                }
            }
            //H8 Synchronous speed, ns, in RPM 
            foreach (var item in tblLoadTest)
            {
                // H = (120*G/o)
                double H8Formula = (120 * Convert.ToDouble(item.SpeedRPM) / Convert.ToDouble(tblMotor.POLE));
                switch (item.LabelCount)
                {
                    case 0:
                        page3Model.SynchronousspeednsinRPM1 = H8Formula.ToString(); //item.SpeedRPM.ToString();
                        break;
                    case 25:
                        page3Model.SynchronousspeednsinRPM2 = H8Formula.ToString();
                        break;
                    case 50:
                        page3Model.SynchronousspeednsinRPM3 = H8Formula.ToString();
                        break;
                    case 75:
                        page3Model.SynchronousspeednsinRPM4 = H8Formula.ToString();
                        break;
                    case 100:
                        page3Model.SynchronousspeednsinRPM5 = H8Formula.ToString();
                        break;
                    case 115:
                        page3Model.SynchronousspeednsinRPM6 = H8Formula.ToString();
                        break;
                    case 130:
                        page3Model.SynchronousspeednsinRPM7 = H8Formula.ToString();
                        break;
                }
            }
            //I9    Observed Speed from RPM Sensor  LoadTestFrom =>  Textbox -> Speed RPM[textBoxSpeedRPM]
            foreach (var item in tblLoadTest)
            {
                if (item.SpeedRPM != null)
                {
                    switch (item.LabelCount)
                    {
                        case 0:
                            page3Model.ObservedSpeedinmin1 = item.SpeedRPM.ToString();
                            break;
                        case 25:
                            page3Model.ObservedSpeedinmin2 = item.SpeedRPM.ToString();
                            break;
                        case 50:
                            page3Model.ObservedSpeedinmin3 = item.SpeedRPM.ToString();
                            break;
                        case 75:
                            page3Model.ObservedSpeedinmin4 = item.SpeedRPM.ToString();
                            break;
                        case 100:
                            page3Model.ObservedSpeedinmin5 = item.SpeedRPM.ToString();
                            break;
                        case 115:
                            page3Model.ObservedSpeedinmin6 = item.SpeedRPM.ToString();
                            break;
                        case 130:
                            page3Model.ObservedSpeedinmin7 = item.SpeedRPM.ToString();
                            break;
                    }
                }
            }
            //J10  Observed Slip, in r/min  => J10 = H-I
            foreach (var item in tblLoadTest)
            {
                switch (item.LabelCount)
                {
                    case 0:
                        page3Model.ObservedSlipinmin1 = (page3Model.SynchronousspeednsinRPM1.ToDoble() - page3Model.ObservedSpeedinmin1.ToDoble()).ToString();
                        break;
                    case 25:
                        page3Model.ObservedSlipinmin2 = (page3Model.SynchronousspeednsinRPM2.ToDoble() - page3Model.ObservedSpeedinmin2.ToDoble()).ToString();
                        break;
                    case 50:
                        page3Model.ObservedSlipinmin3 = (page3Model.SynchronousspeednsinRPM3.ToDoble() - page3Model.ObservedSpeedinmin3.ToDoble()).ToString();
                        break;
                    case 75:
                        page3Model.ObservedSlipinmin4 = (page3Model.SynchronousspeednsinRPM4.ToDoble() - page3Model.ObservedSpeedinmin4.ToDoble()).ToString();
                        break;
                    case 100:
                        page3Model.ObservedSlipinmin5 = (page3Model.SynchronousspeednsinRPM5.ToDoble() - page3Model.ObservedSpeedinmin5.ToDoble()).ToString();
                        break;
                    case 115:
                        page3Model.ObservedSlipinmin6 = (page3Model.SynchronousspeednsinRPM6.ToDoble() - page3Model.ObservedSpeedinmin6.ToDoble()).ToString();
                        break;
                    case 130:
                        page3Model.ObservedSlipinmin7 = (page3Model.SynchronousspeednsinRPM7.ToDoble() - page3Model.ObservedSpeedinmin7.ToDoble()).ToString();
                        break;
                }
            }
            //K11  Observed Slip, in p.u.  => K = J/H
            foreach (var item in tblLoadTest)
            {
                switch (item.LabelCount)
                {
                    case 0:

                        page3Model.ObservedSlipinpu1 = (page3Model.ObservedSlipinmin1.ToDoble() / page3Model.SynchronousspeednsinRPM1.ToDoble()).ToString();
                        break;
                    case 25:
                        page3Model.ObservedSlipinpu2 = (page3Model.ObservedSlipinmin2.ToDoble() / page3Model.SynchronousspeednsinRPM2.ToDoble()).ToString();
                        break;
                    case 50:
                        page3Model.ObservedSlipinpu3 = (page3Model.ObservedSlipinmin3.ToDoble() / page3Model.SynchronousspeednsinRPM3.ToDoble()).ToString();
                        break;
                    case 75:
                        page3Model.ObservedSlipinpu4 = (page3Model.ObservedSlipinmin4.ToDoble() / page3Model.SynchronousspeednsinRPM4.ToDoble()).ToString();
                        break;
                    case 100:
                        page3Model.ObservedSlipinpu5 = (page3Model.ObservedSlipinmin5.ToDoble() / page3Model.SynchronousspeednsinRPM5.ToDoble()).ToString();
                        break;
                    case 115:
                        page3Model.ObservedSlipinpu6 = (page3Model.ObservedSlipinmin6.ToDoble() / page3Model.SynchronousspeednsinRPM6.ToDoble()).ToString();
                        break;
                    case 130:
                        page3Model.ObservedSlipinpu7 = (page3Model.ObservedSlipinmin7.ToDoble() / page3Model.SynchronousspeednsinRPM7.ToDoble()).ToString();
                        break;
                }
            }
            //L12 Corrected Slip, in p.u => L = K * (A+234.5)/(D+234.5)
            foreach (var item in tblLoadTest)
            {
                switch (item.LabelCount)
                {
                    case 0:

                        page3Model.CorrectedSlipinpu1 = (page3Model.ObservedSlipinpu1.ToDoble() * (page3Model.SpecifiedtemperatureTsInC.ToDoble() + 234.5) / (page3Model.StatorWindingTemp1.ToDoble() + 234.5)).ToString();
                        break;
                    case 25:
                        page3Model.CorrectedSlipinpu2 = (page3Model.ObservedSlipinpu2.ToDoble() * (page3Model.SpecifiedtemperatureTsInC.ToDoble() + 234.5) / (page3Model.StatorWindingTemp2.ToDoble() + 234.5)).ToString();
                        break;
                    case 50:
                        page3Model.CorrectedSlipinpu3 = (page3Model.ObservedSlipinpu3.ToDoble() * (page3Model.SpecifiedtemperatureTsInC.ToDoble() + 234.5) / (page3Model.StatorWindingTemp3.ToDoble() + 234.5)).ToString();
                        break;
                    case 75:
                        page3Model.CorrectedSlipinpu4 = (page3Model.ObservedSlipinpu4.ToDoble() * (page3Model.SpecifiedtemperatureTsInC.ToDoble() + 234.5) / (page3Model.StatorWindingTemp4.ToDoble() + 234.5)).ToString();
                        break;
                    case 100:
                        page3Model.CorrectedSlipinpu5 = (page3Model.ObservedSlipinpu5.ToDoble() * (page3Model.SpecifiedtemperatureTsInC.ToDoble() + 234.5) / (page3Model.StatorWindingTemp5.ToDoble() + 234.5)).ToString();
                        break;
                    case 115:
                        page3Model.CorrectedSlipinpu6 = (page3Model.ObservedSlipinpu6.ToDoble() * (page3Model.SpecifiedtemperatureTsInC.ToDoble() + 234.5) / (page3Model.StatorWindingTemp6.ToDoble() + 234.5)).ToString();
                        break;
                    case 130:
                        page3Model.CorrectedSlipinpu7 = (page3Model.ObservedSlipinpu7.ToDoble() * (page3Model.SpecifiedtemperatureTsInC.ToDoble() + 234.5) / (page3Model.StatorWindingTemp7.ToDoble() + 234.5)).ToString();
                        break;
                }
            }
            //M13 Corrected Speed, in r/min  => M = (120*k/o) * (1-L)
            foreach (var item in tblLoadTest)
            {
                switch (item.LabelCount)
                {
                    case 0:
                        page3Model.CorrectedSlipinpu1 = ((120 * page3Model.SpecifiedtemperatureTsInC.ToDoble() / tblMotor.POLE.ToDoble()) * (1 / page3Model.ObservedSlipinpu1.ToDoble())).ToString();
                        break;
                    case 25:
                        page3Model.CorrectedSlipinpu2 = ((120 * page3Model.SpecifiedtemperatureTsInC.ToDoble() / tblMotor.POLE.ToDoble()) * (1 / page3Model.ObservedSlipinpu2.ToDoble())).ToString();
                        break;
                    case 50:
                        page3Model.CorrectedSlipinpu3 = ((120 * page3Model.SpecifiedtemperatureTsInC.ToDoble() / tblMotor.POLE.ToDoble()) * (1 / page3Model.ObservedSlipinpu3.ToDoble())).ToString();
                        break;
                    case 75:
                        page3Model.CorrectedSlipinpu4 = ((120 * page3Model.SpecifiedtemperatureTsInC.ToDoble() / tblMotor.POLE.ToDoble()) * (1 / page3Model.ObservedSlipinpu4.ToDoble())).ToString();
                        break;
                    case 100:
                        page3Model.CorrectedSlipinpu5 = ((120 * page3Model.SpecifiedtemperatureTsInC.ToDoble() / tblMotor.POLE.ToDoble()) * (1 / page3Model.ObservedSlipinpu5.ToDoble())).ToString();
                        break;
                    case 115:
                        page3Model.CorrectedSlipinpu6 = ((120 * page3Model.SpecifiedtemperatureTsInC.ToDoble() / tblMotor.POLE.ToDoble()) * (1 / page3Model.ObservedSlipinpu6.ToDoble())).ToString();
                        break;
                    case 130:
                        page3Model.CorrectedSlipinpu7 = ((120 * page3Model.SpecifiedtemperatureTsInC.ToDoble() / tblMotor.POLE.ToDoble()) * (1 / page3Model.ObservedSlipinpu7.ToDoble())).ToString();
                        break;
                }
            }
            //N14   N = Torque from Torque Sensor - LoadTestFrom =>  Textbox -> Speed RPM[textBoxTorqueNm]
            foreach (var item in tblLoadTest)
            {
                if (item.TorqueNm != null)
                {
                    switch (item.LabelCount)
                    {
                        case 0:
                            page3Model.TorqueinNm1 = item.TorqueNm.ToString();
                            break;
                        case 25:
                            page3Model.TorqueinNm2 = item.TorqueNm.ToString();
                            break;
                        case 50:
                            page3Model.TorqueinNm3 = item.TorqueNm.ToString();
                            break;
                        case 75:
                            page3Model.TorqueinNm4 = item.TorqueNm.ToString();
                            break;
                        case 100:
                            page3Model.TorqueinNm5 = item.TorqueNm.ToString();
                            break;
                        case 115:
                            page3Model.TorqueinNm6 = item.TorqueNm.ToString();
                            break;
                        case 130:
                            page3Model.TorqueinNm7 = item.TorqueNm.ToString();
                            break;
                    }
                }
            }
            //O15  O = Static Zero
            foreach (var item in tblLoadTest)
            {
                switch (item.LabelCount)
                {
                    case 0:
                        page3Model.DynamometerCorrectioninNm1 = "0";
                        break;
                    case 25:
                        page3Model.DynamometerCorrectioninNm2 = "0";
                        break;
                    case 50:
                        page3Model.DynamometerCorrectioninNm3 = "0";
                        break;
                    case 75:
                        page3Model.DynamometerCorrectioninNm4 = "0";
                        break;
                    case 100:
                        page3Model.DynamometerCorrectioninNm5 = "0";
                        break;
                    case 115:
                        page3Model.DynamometerCorrectioninNm6 = "0";
                        break;
                    case 130:
                        page3Model.DynamometerCorrectioninNm7 = "0";
                        break;
                }
            }
            //P16 Corrected Torque, in N-m    -  P = N+O   
            foreach (var item in tblLoadTest)
            {
                if (item.TorqueNm != null)
                {
                    switch (item.LabelCount)
                    {
                        case 0:
                            page3Model.CorrectedTorqueinNm1 = (page3Model.TorqueinNm1.ToDoble() + page3Model.DynamometerCorrectioninNm1.ToDoble()).ToString();
                            break;
                        case 25:
                            page3Model.CorrectedTorqueinNm2 = (page3Model.TorqueinNm2.ToDoble() + page3Model.DynamometerCorrectioninNm2.ToDoble()).ToString();
                            break;
                        case 50:
                            page3Model.CorrectedTorqueinNm3 = (page3Model.TorqueinNm3.ToDoble() + page3Model.DynamometerCorrectioninNm3.ToDoble()).ToString();
                            break;
                        case 75:
                            page3Model.CorrectedTorqueinNm4 = (page3Model.TorqueinNm4.ToDoble() + page3Model.DynamometerCorrectioninNm4.ToDoble()).ToString();
                            break;
                        case 100:
                            page3Model.CorrectedTorqueinNm5 = (page3Model.TorqueinNm5.ToDoble() + page3Model.DynamometerCorrectioninNm5.ToDoble()).ToString();
                            break;
                        case 115:
                            page3Model.CorrectedTorqueinNm6 = (page3Model.TorqueinNm6.ToDoble() + page3Model.DynamometerCorrectioninNm6.ToDoble()).ToString();
                            break;
                        case 130:
                            page3Model.CorrectedTorqueinNm7 = (page3Model.TorqueinNm7.ToDoble() + page3Model.DynamometerCorrectioninNm7.ToDoble()).ToString();
                            break;
                    }
                }
            }
            //Q17   - Q = I*P*0.00010472
            foreach (var item in tblLoadTest)
            {
                switch (item.LabelCount)
                {
                    case 0:
                        page3Model.ShaftPowerinkW1 = (page3Model.ObservedSpeedinmin1.ToDoble() * page3Model.CorrectedTorqueinNm1.ToDoble() * 0.00010472).ToString();
                        break;
                    case 25:
                        page3Model.ShaftPowerinkW2 = (page3Model.ObservedSpeedinmin2.ToDoble() * page3Model.CorrectedTorqueinNm2.ToDoble() * 0.00010472).ToString();
                        break;
                    case 50:
                        page3Model.ShaftPowerinkW3 = (page3Model.ObservedSpeedinmin3.ToDoble() * page3Model.CorrectedTorqueinNm3.ToDoble() * 0.00010472).ToString();
                        break;
                    case 75:
                        page3Model.ShaftPowerinkW4 = (page3Model.ObservedSpeedinmin4.ToDoble() * page3Model.CorrectedTorqueinNm4.ToDoble() * 0.00010472).ToString();
                        break;
                    case 100:
                        page3Model.ShaftPowerinkW5 = (page3Model.ObservedSpeedinmin5.ToDoble() * page3Model.CorrectedTorqueinNm5.ToDoble() * 0.00010472).ToString();
                        break;
                    case 115:
                        page3Model.ShaftPowerinkW6 = (page3Model.ObservedSpeedinmin6.ToDoble() * page3Model.CorrectedTorqueinNm6.ToDoble() * 0.00010472).ToString();
                        break;
                    case 130:
                        page3Model.ShaftPowerinkW7 = (page3Model.ObservedSpeedinmin7.ToDoble() * page3Model.CorrectedTorqueinNm7.ToDoble() * 0.00010472).ToString();
                        break;
                }
            }
            //R18  Line Current, in A  = ?
            foreach (var item in tblLoadTest)
            {
                switch (item.LabelCount)
                {
                    case 0:
                        page3Model.LineCurrentinA1 = "";
                        break;
                    case 25:
                        page3Model.LineCurrentinA2 = "";
                        break;
                    case 50:
                        page3Model.LineCurrentinA3 = "";
                        break;
                    case 75:
                        page3Model.LineCurrentinA4 = "";
                        break;
                    case 100:
                        page3Model.LineCurrentinA5 = "";
                        break;
                    case 115:
                        page3Model.LineCurrentinA6 = "";
                        break;
                    case 130:
                        page3Model.LineCurrentinA7 = "";
                        break;
                }
            }
            //S19 Stator Power, in kW  = ?
            foreach (var item in tblLoadTest)
            {
                if (item.ShaftPowerkW != null)
                {
                    switch (item.LabelCount)
                    {
                        case 0:
                            page3Model.StatorPowerinkW1 = "";
                            break;
                        case 25:
                            page3Model.StatorPowerinkW2 = "";
                            break;
                        case 50:
                            page3Model.StatorPowerinkW3 = "";
                            break;
                        case 75:
                            page3Model.StatorPowerinkW4 = "";
                            break;
                        case 100:
                            page3Model.StatorPowerinkW5 = "";
                            break;
                        case 115:
                            page3Model.StatorPowerinkW6 = "";
                            break;
                        case 130:
                            page3Model.StatorPowerinkW7 = "";
                            break;
                    }
                }
            }
            //T20 Stator I2R Loss, in kW, at tt  -  T = 1.5*R*R*B*(234.5+D)/(234.5+C)/1000
            foreach (var item in tblLoadTest)
            {
                if (item.ShaftPowerkW != null)
                {
                    switch (item.LabelCount)
                    {
                        case 0:
                            page3Model.StatorI2RLossinkWatts1 = (1.5 * page3Model.LineCurrentinA1.ToDoble() * page3Model.LineCurrentinA1.ToDoble() * page3Model.StatorResistanceColdInOhms.ToDoble() * (234.5 + page3Model.StatorWindingTemp1.ToDoble()) / (234.5 + page3Model.StatorResistanceColdMeasureAtTempInC.ToDoble()) / 1000).ToString();
                            break;
                        case 25:
                            page3Model.StatorI2RLossinkWatts2 = (1.5 * page3Model.LineCurrentinA2.ToDoble() * page3Model.LineCurrentinA2.ToDoble() * page3Model.StatorResistanceColdInOhms.ToDoble() * (234.5 + page3Model.StatorWindingTemp2.ToDoble()) / (234.5 + page3Model.StatorResistanceColdMeasureAtTempInC.ToDoble()) / 1000).ToString();
                            break;
                        case 50:
                            page3Model.StatorI2RLossinkWatts3 = (1.5 * page3Model.LineCurrentinA3.ToDoble() * page3Model.LineCurrentinA3.ToDoble() * page3Model.StatorResistanceColdInOhms.ToDoble() * (234.5 + page3Model.StatorWindingTemp3.ToDoble()) / (234.5 + page3Model.StatorResistanceColdMeasureAtTempInC.ToDoble()) / 1000).ToString();
                            break;
                        case 75:
                            page3Model.StatorI2RLossinkWatts4 = (1.5 * page3Model.LineCurrentinA4.ToDoble() * page3Model.LineCurrentinA4.ToDoble() * page3Model.StatorResistanceColdInOhms.ToDoble() * (234.5 + page3Model.StatorWindingTemp4.ToDoble()) / (234.5 + page3Model.StatorResistanceColdMeasureAtTempInC.ToDoble()) / 1000).ToString();
                            break;
                        case 100:
                            page3Model.StatorI2RLossinkWatts5 = (1.5 * page3Model.LineCurrentinA5.ToDoble() * page3Model.LineCurrentinA5.ToDoble() * page3Model.StatorResistanceColdInOhms.ToDoble() * (234.5 + page3Model.StatorWindingTemp5.ToDoble()) / (234.5 + page3Model.StatorResistanceColdMeasureAtTempInC.ToDoble()) / 1000).ToString();
                            break;
                        case 115:
                            page3Model.StatorI2RLossinkWatts6 = (1.5 * page3Model.LineCurrentinA6.ToDoble() * page3Model.LineCurrentinA6.ToDoble() * page3Model.StatorResistanceColdInOhms.ToDoble() * (234.5 + page3Model.StatorWindingTemp6.ToDoble()) / (234.5 + page3Model.StatorResistanceColdMeasureAtTempInC.ToDoble()) / 1000).ToString();
                            break;
                        case 130:
                            page3Model.StatorI2RLossinkWatts7 = (1.5 * page3Model.LineCurrentinA7.ToDoble() * page3Model.LineCurrentinA7.ToDoble() * page3Model.StatorResistanceColdInOhms.ToDoble() * (234.5 + page3Model.StatorWindingTemp7.ToDoble()) / (234.5 + page3Model.StatorResistanceColdMeasureAtTempInC.ToDoble()) / 1000).ToString();
                            break;
                    }
                }
            }
            //U21 Winding Resistance at ts    -   U = B*(A+234.5)/(C+234.5)
            foreach (var item in tblLoadTest)
            {
                switch (item.LabelCount)
                {
                    case 0:
                        page3Model.WindingResistanceatts1 = (page3Model.StatorResistanceColdInOhms.ToDoble() * (page3Model.SpecifiedtemperatureTsInC.ToDoble() + 234.5) / (page3Model.StatorResistanceColdMeasureAtTempInC.ToDoble() + 234.5)).ToString();
                        break;
                    case 25:
                        page3Model.WindingResistanceatts2 = (page3Model.StatorResistanceColdInOhms.ToDoble() * (page3Model.SpecifiedtemperatureTsInC.ToDoble() + 234.5) / (page3Model.StatorResistanceColdMeasureAtTempInC.ToDoble() + 234.5)).ToString();
                        break;
                    case 50:
                        page3Model.WindingResistanceatts3 = (page3Model.StatorResistanceColdInOhms.ToDoble() * (page3Model.SpecifiedtemperatureTsInC.ToDoble() + 234.5) / (page3Model.StatorResistanceColdMeasureAtTempInC.ToDoble() + 234.5)).ToString();
                        break;
                    case 75:
                        page3Model.WindingResistanceatts4 = (page3Model.StatorResistanceColdInOhms.ToDoble() * (page3Model.SpecifiedtemperatureTsInC.ToDoble() + 234.5) / (page3Model.StatorResistanceColdMeasureAtTempInC.ToDoble() + 234.5)).ToString();
                        break;
                    case 100:
                        page3Model.WindingResistanceatts5 = (page3Model.StatorResistanceColdInOhms.ToDoble() * (page3Model.SpecifiedtemperatureTsInC.ToDoble() + 234.5) / (page3Model.StatorResistanceColdMeasureAtTempInC.ToDoble() + 234.5)).ToString();
                        break;
                    case 115:
                        page3Model.WindingResistanceatts6 = (page3Model.StatorResistanceColdInOhms.ToDoble() * (page3Model.SpecifiedtemperatureTsInC.ToDoble() + 234.5) / (page3Model.StatorResistanceColdMeasureAtTempInC.ToDoble() + 234.5)).ToString();
                        break;
                    case 130:
                        page3Model.WindingResistanceatts7 = (page3Model.StatorResistanceColdInOhms.ToDoble() * (page3Model.SpecifiedtemperatureTsInC.ToDoble() + 234.5) / (page3Model.StatorResistanceColdMeasureAtTempInC.ToDoble() + 234.5)).ToString();
                        break;
                }
            }
            //V22 Stator I2R Loss, in kW, at ts   - V = 1.5*R*R*U/1000
            foreach (var item in tblLoadTest)
            {
                switch (item.LabelCount)
                {
                    case 0:
                        page3Model.StatorPowerinkW1 = (1.5 * page3Model.LineCurrentinA1.ToDoble() * page3Model.LineCurrentinA1.ToDoble() * page3Model.WindingResistanceatts1.ToDoble() / 1000).ToString();
                        break;
                    case 25:
                        page3Model.StatorPowerinkW2 = (1.5 * page3Model.LineCurrentinA2.ToDoble() * page3Model.LineCurrentinA2.ToDoble() * page3Model.WindingResistanceatts2.ToDoble() / 1000).ToString();
                        break;
                    case 50:
                        page3Model.StatorPowerinkW3 = (1.5 * page3Model.LineCurrentinA3.ToDoble() * page3Model.LineCurrentinA3.ToDoble() * page3Model.WindingResistanceatts3.ToDoble() / 1000).ToString();
                        break;
                    case 75:
                        page3Model.StatorPowerinkW4 = (1.5 * page3Model.LineCurrentinA4.ToDoble() * page3Model.LineCurrentinA4.ToDoble() * page3Model.WindingResistanceatts4.ToDoble() / 1000).ToString();
                        break;
                    case 100:
                        page3Model.StatorPowerinkW5 = (1.5 * page3Model.LineCurrentinA5.ToDoble() * page3Model.LineCurrentinA5.ToDoble() * page3Model.WindingResistanceatts5.ToDoble() / 1000).ToString();
                        break;
                    case 115:
                        page3Model.StatorPowerinkW6 = (1.5 * page3Model.LineCurrentinA6.ToDoble() * page3Model.LineCurrentinA6.ToDoble() * page3Model.WindingResistanceatts6.ToDoble() / 1000).ToString();
                        break;
                    case 130:
                        page3Model.StatorPowerinkW7 = (1.5 * page3Model.LineCurrentinA7.ToDoble() * page3Model.LineCurrentinA7.ToDoble() * page3Model.WindingResistanceatts7.ToDoble() / 1000).ToString();
                        break;
                }
            }

            //W23 Stator Power Correction, in kW  -  W = V-T
            foreach (var item in tblLoadTest)
            {
                switch (item.LabelCount)
                {
                    case 0:
                        page3Model.StatorPowerCorrectioninkW1 = (page3Model.StatorPowerinkW1.ToDoble() - page3Model.StatorI2RLossinkWatts1.ToDoble()).ToString();
                        break;
                    case 25:
                        page3Model.StatorPowerCorrectioninkW2 = (page3Model.StatorPowerinkW2.ToDoble() - page3Model.StatorI2RLossinkWatts2.ToDoble()).ToString();
                        break;
                    case 50:
                        page3Model.StatorPowerCorrectioninkW3 = (page3Model.StatorPowerinkW3.ToDoble() - page3Model.StatorI2RLossinkWatts3.ToDoble()).ToString();
                        break;
                    case 75:
                        page3Model.StatorPowerCorrectioninkW4 = (page3Model.StatorPowerinkW4.ToDoble() - page3Model.StatorI2RLossinkWatts4.ToDoble()).ToString();
                        break;
                    case 100:
                        page3Model.StatorPowerCorrectioninkW5 = (page3Model.StatorPowerinkW5.ToDoble() - page3Model.StatorI2RLossinkWatts5.ToDoble()).ToString();
                        break;
                    case 115:
                        page3Model.StatorPowerCorrectioninkW6 = (page3Model.StatorPowerinkW6.ToDoble() - page3Model.StatorI2RLossinkWatts6.ToDoble()).ToString();
                        break;
                    case 130:
                        page3Model.StatorPowerCorrectioninkW7 = (page3Model.StatorPowerinkW7.ToDoble() - page3Model.StatorI2RLossinkWatts7.ToDoble()).ToString();
                        break;
                }
            }

            //X24 Corrected Stator Power, in kW - X = S+W
            foreach (var item in tblLoadTest)
            {
                switch (item.LabelCount)
                {
                    case 0:
                        page3Model.CorrectedStatorPowerinkW1 = (page3Model.StatorPowerinkW1.ToDoble() + page3Model.StatorPowerCorrectioninkW1.ToDoble()).ToString();
                        break;
                    case 25:
                        page3Model.CorrectedStatorPowerinkW2 = (page3Model.StatorPowerinkW2.ToDoble() + page3Model.StatorPowerCorrectioninkW2.ToDoble()).ToString();
                        break;
                    case 50:
                        page3Model.CorrectedStatorPowerinkW3 = (page3Model.StatorPowerinkW3.ToDoble() + page3Model.StatorPowerCorrectioninkW3.ToDoble()).ToString();
                        break;
                    case 75:
                        page3Model.CorrectedStatorPowerinkW4 = (page3Model.StatorPowerinkW4.ToDoble() + page3Model.StatorPowerCorrectioninkW4.ToDoble()).ToString();
                        break;
                    case 100:
                        page3Model.CorrectedStatorPowerinkW5 = (page3Model.StatorPowerinkW5.ToDoble() + page3Model.StatorPowerCorrectioninkW5.ToDoble()).ToString();
                        break;
                    case 115:
                        page3Model.CorrectedStatorPowerinkW6 = (page3Model.StatorPowerinkW6.ToDoble() + page3Model.StatorPowerCorrectioninkW6.ToDoble()).ToString();
                        break;
                    case 130:
                        page3Model.CorrectedStatorPowerinkW7 = (page3Model.StatorPowerinkW7.ToDoble() + page3Model.StatorPowerCorrectioninkW7.ToDoble()).ToString();
                        break;
                }
            }
            //Y25 Efficiency, in %  -  Y = 100*Q/X
            foreach (var item in tblLoadTest)
            {
                switch (item.LabelCount)
                {
                    case 0:
                        page3Model.EfficiencyinPercentage1 = (100 * page3Model.ShaftPowerinkW1.ToDoble() / page3Model.CorrectedStatorPowerinkW1.ToDoble()).ToString();
                        break;
                    case 25:
                        page3Model.EfficiencyinPercentage2 = (100 * page3Model.ShaftPowerinkW2.ToDoble() / page3Model.CorrectedStatorPowerinkW2.ToDoble()).ToString();
                        break;
                    case 50:
                        page3Model.EfficiencyinPercentage3 = (100 * page3Model.ShaftPowerinkW3.ToDoble() / page3Model.CorrectedStatorPowerinkW3.ToDoble()).ToString();
                        break;
                    case 75:
                        page3Model.EfficiencyinPercentage4 = (100 * page3Model.ShaftPowerinkW4.ToDoble() / page3Model.CorrectedStatorPowerinkW4.ToDoble()).ToString();
                        break;
                    case 100:
                        page3Model.EfficiencyinPercentage5 = (100 * page3Model.ShaftPowerinkW5.ToDoble() / page3Model.CorrectedStatorPowerinkW5.ToDoble()).ToString();
                        break;
                    case 115:
                        page3Model.EfficiencyinPercentage6 = (100 * page3Model.ShaftPowerinkW6.ToDoble() / page3Model.CorrectedStatorPowerinkW6.ToDoble()).ToString();
                        break;
                    case 130:
                        page3Model.EfficiencyinPercentage7 = (100 * page3Model.ShaftPowerinkW7.ToDoble() / page3Model.CorrectedStatorPowerinkW7.ToDoble()).ToString();
                        break;
                }
            }

            //Z26 Power Factor, in %   -  Z = 100*X/(1.732*F*R)*1000
            foreach (var item in tblLoadTest)
            {
                switch (item.LabelCount)
                {
                    case 0:
                        page3Model.PowerFactorinPercentage1 = (1000 * page3Model.CorrectedStatorPowerinkW1.ToDoble() / (1.732 * page3Model.LinetoLineCol1Voltage1.ToDoble() * page3Model.LineCurrentinA1.ToDoble()) * 1000).ToString();
                        break;
                    case 25:
                        page3Model.PowerFactorinPercentage2 = (1000 * page3Model.CorrectedStatorPowerinkW2.ToDoble() / (1.732 * page3Model.LinetoLineCol1Voltage2.ToDoble() * page3Model.LineCurrentinA2.ToDoble()) * 1000).ToString();
                        break;
                    case 50:
                        page3Model.PowerFactorinPercentage3 = (1000 * page3Model.CorrectedStatorPowerinkW3.ToDoble() / (1.732 * page3Model.LinetoLineCol1Voltage3.ToDoble() * page3Model.LineCurrentinA3.ToDoble()) * 1000).ToString();
                        break;
                    case 75:
                        page3Model.PowerFactorinPercentage4 = (1000 * page3Model.CorrectedStatorPowerinkW4.ToDoble() / (1.732 * page3Model.LinetoLineCol1Voltage4.ToDoble() * page3Model.LineCurrentinA4.ToDoble()) * 1000).ToString();
                        break;
                    case 100:
                        page3Model.PowerFactorinPercentage5 = (1000 * page3Model.CorrectedStatorPowerinkW5.ToDoble() / (1.732 * page3Model.LinetoLineCol1Voltage5.ToDoble() * page3Model.LineCurrentinA5.ToDoble()) * 1000).ToString();
                        break;
                    case 115:
                        page3Model.PowerFactorinPercentage6 = (1000 * page3Model.CorrectedStatorPowerinkW6.ToDoble() / (1.732 * page3Model.LinetoLineCol1Voltage6.ToDoble() * page3Model.LineCurrentinA6.ToDoble()) * 1000).ToString();
                        break;
                    case 130:
                        page3Model.PowerFactorinPercentage7 = (1000 * page3Model.CorrectedStatorPowerinkW7.ToDoble() / (1.732 * page3Model.LinetoLineCol1Voltage7.ToDoble() * page3Model.LineCurrentinA7.ToDoble()) * 1000).ToString();
                        break;
                }
            }

            new Conversion().CopyProperties(page3Model, page3ModelFinilize);
            return page3ModelFinilize;
        }

        private Page2ModelFinilize ReportingPage2(tblEntity tblEntity, tblMotor _tblMotor)
        {
            Page2ModelFinilize modelFinilize = new Page2ModelFinilize();
            KomaxApp.Model.Reporting.Model.Page2.Page2Model page2Model = new KomaxApp.Model.Reporting.Model.Page2.Page2Model();

            new Conversion().CopyProperties(tblEntity, page2Model);
            page2Model.TestReportNo = _tblMotor.TestReportNo;
            page2Model.MotorModel = _tblMotor.MotorModel;
            page2Model.Dated = _tblMotor.Dated;
            page2Model.SerialNo = _tblMotor.SerialNo;


            new Conversion().CopyProperties(page2Model, modelFinilize);
            return modelFinilize;
        }

        private Page1ModelFinilize ReportingPage1(tblMotor _tblMotor, tblImages _tblImages, List<tblShaftPawer> tblShaftPawer,
                                    List<tblEfficiency> tblEfficiency, List<tblSpeedRPM> tblSpeedRPM,
                                    List<tblCurrentAmps> tblCurrentAmps, List<tblCos> tblCos)
        {
            Page1Model page1Model = new Page1Model();
            page1Model.singleMotorModel = _tblMotor;
            page1Model.singleImageModel = _tblImages;
            page1Model.singleMotorModel.Image = page1Model.singleImageModel.Image;
            page1Model.singleMotorModel.MotorImage = page1Model.singleImageModel.Image;
            page1Model.singleMotorModel.SR_NO = page1Model.singleMotorModel.SerialNo;
            page1Model.lstShaftPawer.Clear(); // Ensure the list is empty before adding items



            #region Page1ModelFinilize
            Page1ModelFinilize page1ModelFinilize = new Page1ModelFinilize();
            new Conversion().CopyProperties(page1Model.singleMotorModel, page1ModelFinilize);
            #endregion

            #region FORMULAS
            double EFACTOR, SFACTOR, PFACTOR;
            //Formula
            if (0.15 <= Convert.ToDouble(page1Model.singleMotorModel.KW))
            {
                EFACTOR = 0.15;
            }
            else
            {
                EFACTOR = 0.10;
            }

            //Formula
            if (Convert.ToDouble(page1Model.singleMotorModel.KW) >= 0.2)
            {
                SFACTOR = 0.2;
            }
            else
            {
                SFACTOR = 0.3;
            }

            //Formula
            if (Convert.ToDouble(page1Model.singleMotorModel.KW) >= 0.2)
            {
                SFACTOR = 0.2;
            }
            else
            {
                SFACTOR = 0.3;
            }



            PFACTOR = 7;  //Max And mix ?
            #endregion

            //Row1  ShaftPawer(P2)
            foreach (var item in tblShaftPawer)
            {
                if (item.Row != null)
                {
                    switch (item.RowNo)
                    {
                        case 1:
                            page1ModelFinilize.ShaftPower1 = item.Row.ToString();
                            page1ModelFinilize.ShaftPowerTableTwo1 = item.Row.ToString();

                            break;
                        case 2:
                            page1ModelFinilize.ShaftPower2 = item.Row.ToString();
                            page1ModelFinilize.ShaftPowerTableTwo2 = item.Row.ToString();
                            break;
                        case 3:
                            page1ModelFinilize.ShaftPower3 = item.Row.ToString();
                            page1ModelFinilize.ShaftPowerTableTwo3 = item.Row.ToString();
                            break;
                        case 4:
                            page1ModelFinilize.ShaftPower4 = item.Row.ToString();
                            page1ModelFinilize.ShaftPowerTableTwo4 = item.Row.ToString();
                            break;
                        case 5:
                            page1ModelFinilize.ShaftPower5 = item.Row.ToString();
                            page1ModelFinilize.ShaftPowerTableTwo5 = item.Row.ToString();
                            break;
                        case 6:
                            page1ModelFinilize.ShaftPower6 = item.Row.ToString();
                            page1ModelFinilize.ShaftPowerTableTwo6 = item.Row.ToString();
                            break;
                        case 7:
                            page1ModelFinilize.ShaftPower7 = item.Row.ToString();
                            page1ModelFinilize.ShaftPowerTableTwo7 = item.Row.ToString();
                            break;
                    }
                }
            }
            //Row2  Efficiency
            foreach (var item in tblEfficiency)
            {

                if (item.Row != null)
                {
                    switch (item.RowNo)
                    {
                        case 1:
                            page1ModelFinilize.Efficiency1 = item.Row.ToString();
                            page1ModelFinilize.EfficiencyTableTwo1 = (item.Row - (EFACTOR * (100 - Convert.ToDouble(page1ModelFinilize.Efficiency1)))).ToString();
                            break;
                        case 2:
                            page1ModelFinilize.Efficiency2 = item.Row.ToString();
                            page1ModelFinilize.EfficiencyTableTwo2 = (item.Row - (EFACTOR * (100 - Convert.ToDouble(page1ModelFinilize.Efficiency2)))).ToString();
                            break;
                        case 3:
                            page1ModelFinilize.Efficiency3 = item.Row.ToString();
                            page1ModelFinilize.EfficiencyTableTwo3 = (item.Row - (EFACTOR * (100 - Convert.ToDouble(page1ModelFinilize.Efficiency3)))).ToString();
                            break;

                        case 4:
                            page1ModelFinilize.Efficiency4 = item.Row.ToString();
                            page1ModelFinilize.EfficiencyTableTwo4 = (item.Row - (EFACTOR * (100 - Convert.ToDouble(page1ModelFinilize.Efficiency4)))).ToString();
                            break;
                        case 5:
                            page1ModelFinilize.Efficiency5 = item.Row.ToString();
                            page1ModelFinilize.EfficiencyTableTwo5 = (item.Row - (EFACTOR * (100 - Convert.ToDouble(page1ModelFinilize.Efficiency5)))).ToString();
                            break;
                        case 6:
                            page1ModelFinilize.Efficiency6 = item.Row.ToString();
                            page1ModelFinilize.EfficiencyTableTwo6 = (item.Row - (EFACTOR * (100 - Convert.ToDouble(page1ModelFinilize.Efficiency6)))).ToString();
                            break;
                        case 7:
                            page1ModelFinilize.Efficiency7 = item.Row.ToString();
                            page1ModelFinilize.EfficiencyTableTwo7 = (item.Row - (EFACTOR * (100 - Convert.ToDouble(page1ModelFinilize.Efficiency7)))).ToString();
                            break;
                    }
                }
            }
            //Row4  Slip -  not understand
            foreach (var item in tblCurrentAmps)
            {
                switch (item.RowNo)
                {
                    case 1:
                        page1ModelFinilize.Slip1 = item.Row.ToString();
                        page1ModelFinilize.Slip1 = (((120 * Convert.ToDouble(page1Model.singleMotorModel.HERTZ)) / 0) * (Convert.ToDouble(page1ModelFinilize.CurrentInAmps1) + (Convert.ToDouble(page1ModelFinilize.CurrentInAmps1) * SFACTOR))).ToString();
                        break;
                }
            }
            //Row3  SpeedRPM
            foreach (var item in tblSpeedRPM)
            {


                if (item.Row != null)
                {
                    switch (item.RowNo)
                    {
                        case 1:
                            page1ModelFinilize.Speed1 = item.Row.ToString();
                            page1ModelFinilize.SpeedTableTwo1 = (((120 * Convert.ToDouble(page1Model.singleMotorModel.HERTZ)) / 0) * (Convert.ToDouble(page1ModelFinilize.CurrentInAmps1) - (Convert.ToDouble(page1ModelFinilize.CurrentInAmps1) * SFACTOR))).ToString();
                            break;
                        case 2:
                            page1ModelFinilize.Speed2 = item.Row.ToString();
                            page1ModelFinilize.SpeedTableTwo2 = (((120 * Convert.ToDouble(page1Model.singleMotorModel.HERTZ)) / 0) * (Convert.ToDouble(page1ModelFinilize.CurrentInAmps2) - (Convert.ToDouble(page1ModelFinilize.CurrentInAmps2) * SFACTOR))).ToString();
                            break;
                        case 3:
                            page1ModelFinilize.Speed3 = item.Row.ToString();
                            page1ModelFinilize.SpeedTableTwo3 = (((120 * Convert.ToDouble(page1Model.singleMotorModel.HERTZ)) / 0) * (Convert.ToDouble(page1ModelFinilize.CurrentInAmps3) - (Convert.ToDouble(page1ModelFinilize.CurrentInAmps3) * SFACTOR))).ToString();
                            break;
                        case 4:
                            page1ModelFinilize.Speed4 = item.Row.ToString();
                            page1ModelFinilize.SpeedTableTwo4 = (((120 * Convert.ToDouble(page1Model.singleMotorModel.HERTZ)) / 0) * (Convert.ToDouble(page1ModelFinilize.CurrentInAmps4) - (Convert.ToDouble(page1ModelFinilize.CurrentInAmps4) * SFACTOR))).ToString();
                            break;
                        case 5:
                            page1ModelFinilize.Speed5 = item.Row.ToString();
                            page1ModelFinilize.SpeedTableTwo5 = (((120 * Convert.ToDouble(page1Model.singleMotorModel.HERTZ)) / 0) * (Convert.ToDouble(page1ModelFinilize.CurrentInAmps5) - (Convert.ToDouble(page1ModelFinilize.CurrentInAmps5) * SFACTOR))).ToString();
                            break;
                        case 6:
                            page1ModelFinilize.Speed6 = item.Row.ToString();
                            page1ModelFinilize.SpeedTableTwo6 = (((120 * Convert.ToDouble(page1Model.singleMotorModel.HERTZ)) / 0) * (Convert.ToDouble(page1ModelFinilize.CurrentInAmps6) - (Convert.ToDouble(page1ModelFinilize.CurrentInAmps6) * SFACTOR))).ToString();
                            break;
                        case 7:
                            page1ModelFinilize.Speed7 = item.Row.ToString();
                            page1ModelFinilize.SpeedTableTwo7 = (((120 * Convert.ToDouble(page1Model.singleMotorModel.HERTZ)) / 0) * (Convert.ToDouble(page1ModelFinilize.CurrentInAmps7) - (Convert.ToDouble(page1ModelFinilize.CurrentInAmps7) * SFACTOR))).ToString();
                            break;
                    }
                }
            }

            //Row6
            foreach (var item in tblCos)
            {
                if (item.Row != null)
                {
                    switch (item.RowNo)
                    {
                        case 1:
                            page1ModelFinilize.Cos1 = item.Row.ToString();
                            break;
                        case 2:
                            page1ModelFinilize.Cos2 = item.Row.ToString();
                            break;
                        case 3:
                            page1ModelFinilize.Cos3 = item.Row.ToString();
                            break;
                        case 4:
                            page1ModelFinilize.Cos4 = item.Row.ToString();
                            break;
                        case 5:
                            page1ModelFinilize.Cos5 = item.Row.ToString();
                            break;
                        case 6:
                            page1ModelFinilize.Cos6 = item.Row.ToString();
                            break;
                        case 7:
                            page1ModelFinilize.Cos7 = item.Row.ToString();
                            break;
                    }
                }
            }


            //Row5  CurrentAmps
            foreach (var item in tblCurrentAmps)
            {
                if (item.Row != null)
                {
                    switch (item.RowNo)
                    {
                        case 1:
                            page1ModelFinilize.CurrentInAmps1 = item.Row.ToString();
                            page1ModelFinilize.CurrentInAmpsTableTwo1 = ((1 / 6) * (100 - Convert.ToDouble(page1ModelFinilize.Cos1))).ToString();
                            break;
                        case 2:
                            page1ModelFinilize.CurrentInAmps2 = item.Row.ToString();
                            page1ModelFinilize.CurrentInAmpsTableTwo2 = ((1 / 6) * (100 - Convert.ToDouble(page1ModelFinilize.Cos2))).ToString();
                            break;
                        case 3:
                            page1ModelFinilize.CurrentInAmps3 = item.Row.ToString();
                            page1ModelFinilize.CurrentInAmpsTableTwo3 = ((1 / 6) * (100 - Convert.ToDouble(page1ModelFinilize.Cos2))).ToString();
                            break;
                        case 4:
                            page1ModelFinilize.CurrentInAmps4 = item.Row.ToString();
                            page1ModelFinilize.CurrentInAmpsTableTwo4 = ((1 / 6) * (100 - Convert.ToDouble(page1ModelFinilize.Cos2))).ToString();
                            break;
                        case 5:
                            page1ModelFinilize.CurrentInAmps5 = item.Row.ToString();
                            page1ModelFinilize.CurrentInAmpsTableTwo5 = ((1 / 6) * (100 - Convert.ToDouble(page1ModelFinilize.Cos2))).ToString();
                            break;
                        case 6:
                            page1ModelFinilize.CurrentInAmps6 = item.Row.ToString();
                            page1ModelFinilize.CurrentInAmpsTableTwo6 = ((1 / 6) * (100 - Convert.ToDouble(page1ModelFinilize.Cos2))).ToString();
                            break;
                        case 7:
                            page1ModelFinilize.CurrentInAmps7 = item.Row.ToString();
                            page1ModelFinilize.CurrentInAmpsTableTwo7 = ((1 / 6) * (100 - Convert.ToDouble(page1ModelFinilize.Cos2))).ToString();
                            break;
                    }
                }
            }
            return page1ModelFinilize;
        }



        //public (VmCreateMotor, ShaftPowerObj.Request) GetDatausingReportDL(string ReportNo)
        //{
        //    var sql = "SELECT * FROM tblMotor m INNER JOIN tblShaftPawer s  ON s.ReportNo = m.ReportNo" +
        //        "INNER JOIN tblSpeedRPM sp  ON sp.ReportNo = m.ReportNo" +
        //        "INNER JOIN tblEfficiency e  ON e.ReportNo = m.ReportNo" +
        //        "INNER JOIN tblCurrentAmps ca  ON ca.ReportNo = m.ReportNo" +
        //        "INNER JOIN tblCos c  ON c.ReportNo = @ReportNo";
        //    var data = db.Query<VmCreateMotor, ShaftPowerObj.Request>(sql,
        //        new
        //        {
        //            ReportNo = ReportNo
        //        },
        //        commandType: CommandType.Text).ToList();
        //    return data.FirstOrDefault();
        //}

        public bool IsReportNoExistinDbDL(string ReportNo)
        {
            var sql = "SELECT TOP 1 ReportNo FROM  [dbo].[tblMotor] Where ReportNo=@ReportNo";
            var data = db.Query<string>(sql, new { ReportNo }, commandType: CommandType.Text).FirstOrDefault();

            if (data == ReportNo)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<GetList> GetMotorList()
        {
            var sql = "SELECT ReportNo,TestDate ,Manufacturer,MotorModel,MotorType,Frame,Phase,MotorRatedKw,MotorRatedHP,MotorRatedVoltage FROM  [dbo].[tblMotor]\r\n";
            var data = db.Query<GetList>(sql, commandType: CommandType.Text).ToList();
            return data;

        }
        public (string PowerMeterPort, string TorqueMeterPort, string RPMPort, string TemperaturePort) GetLastComboPortsOrFetchFromSerialPort()
        {
            try
            {
                // SQL query to get the last record from tblComboBox
                var sql = @"SELECT TOP 1 PowerMeterPort, TorqueMeterPort, RPMPort, TemperaturePort
                        FROM [dbo].[tblComboBox]
                        ORDER BY ID DESC;";  // Replace 'ID' with the appropriate column if needed

                // Attempt to retrieve the last record using the SQL query with CommandType.Text
                var lastRecord = db.QueryFirstOrDefault<(string PowerMeterPort, string TorqueMeterPort, string RPMPort, string TemperaturePort)>(
                sql, commandType: CommandType.Text);

                return lastRecord;

            }
            catch (Exception ex)
            {
                // Handle exceptions, such as database connection issues
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            // If no record was found or an exception occurred, return default empty strings
            return (string.Empty, string.Empty, string.Empty, string.Empty);
        }


        public List<DisplayModel> GetDisplayList()
        {
            var sql = "SELECT SerialNo,ReportNo,TestDate,IsFilled FROM [dbo].[tblMotor]";
            var data = db.Query<DisplayModel>(sql, commandType: CommandType.Text).ToList();
            return data;
        }

        public object GetDataFromDbUsingReportNoDL(string ReportNo)
        {
            var sql = "SELECT * FROM [dbo].[tblMotor] Where ReportNo = @ReportNo";
            var data = db.Query<object>(sql,
                new
                {
                    ReportNo = ReportNo
                },
                commandType: CommandType.Text).ToList();
            return data;
        }
        public RequestLoadTestModel.LabelCountModel CheckLoadTestRecordExistDL(string ReportNo)
        {
            var sql = "SELECT TOP 1 * " +
              "FROM tblLoadTest " +
              "WHERE ReportNo = @ReportNo " +
              "ORDER BY LabelCount DESC";
            var data = db.Query<RequestLoadTestModel.LabelCountModel>(sql, new { ReportNo }, commandType: CommandType.Text).FirstOrDefault();
            return data;
        }
    }
}
