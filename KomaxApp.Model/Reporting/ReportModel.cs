using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.Model.Reporting
{
    public class ReportModel
    {
        public Page1 page1 { get; set; }
        public Page2 page2 { get; set; }
        public Page3 page3 { get; set; }
        public Page4Charts page4Charts { get; set; }
        
        public class Page1
        {

            public string ReportNo { get; set; }
            public string TestDate { get; set; }
            public string Image { get; set; }
            public string TestReportNo { get; set; }
            public string Dated { get; set; }
            public string MotorModel { get; set; }
            public string SerialNo { get; set; }
            public string MotorImage { get; set; }
            public string Manufacturer { get; set; }
            public string Model { get; set; }
            public string Type { get; set; }
            public string Frame { get; set; }
            public string Phase { get; set; }
            public string KW { get; set; }
            public string HP { get; set; }
            public string VOLTS { get; set; }
            public string HERTZ { get; set; }
            public string AMPS { get; set; }
            public string PF { get; set; }
            public string RPM { get; set; }
            public string POLE { get; set; }
            public string EFFICIENCY { get; set; }
            public string DUTY { get; set; }
            public string INS_CLASS { get; set; }
            public string IC { get; set; }
            public string IP { get; set; }
            public string CONNECTION { get; set; }
            // public string SerialNo { get; set; } //

            public string ShaftPowerAll { get; set; }
            public int ShaftRowNo { get; set; }
            public string ShaftPower1 { get; set; }
            public string ShaftPower2 { get; set; }
            public string ShaftPower3 { get; set; }
            public string ShaftPower4 { get; set; }
            public string ShaftPower5 { get; set; }
            public string ShaftPower6 { get; set; }
            public string ShaftPower7 { get; set; }

            public string EfficiencAll { get; set; }
            public int EfficiencRowNo { get; set; }
            public string Efficiency1 { get; set; }
            public string Efficiency2 { get; set; }
            public string Efficiency3 { get; set; }
            public string Efficiency4 { get; set; }
            public string Efficiency5 { get; set; }
            public string Efficiency6 { get; set; }
            public string Efficiency7 { get; set; }


            public string SpeedAll { get; set; }
            public int SpeedRowNo { get; set; }
            public string Speed1 { get; set; }
            public string Speed2 { get; set; }
            public string Speed3 { get; set; }
            public string Speed4 { get; set; }
            public string Speed5 { get; set; }
            public string Speed6 { get; set; }
            public string Speed7 { get; set; }



            public string Slip1 { get; set; }
            public string Slip2 { get; set; }
            public string Slip3 { get; set; }
            public string Slip4 { get; set; }
            public string Slip5 { get; set; }
            public string Slip6 { get; set; }
            public string Slip7 { get; set; }


            public string CurrentInAmpsAll { get; set; }
            public int CurrentInAmpsRowNo { get; set; }
            public string CurrentInAmps1 { get; set; }
            public string CurrentInAmps2 { get; set; }
            public string CurrentInAmps3 { get; set; }
            public string CurrentInAmps4 { get; set; }
            public string CurrentInAmps5 { get; set; }
            public string CurrentInAmps6 { get; set; }
            public string CurrentInAmps7 { get; set; }


            public string CosAll { get; set; }
            public int CosRowNo { get; set; }
            public string Cos1 { get; set; }
            public string Cos2 { get; set; }
            public string Cos3 { get; set; }
            public string Cos4 { get; set; }
            public string Cos5 { get; set; }
            public string Cos6 { get; set; }
            public string Cos7 { get; set; }
        }
        public class Page2
        {

            public string SpecifiedTemperatureTsInC { get; set; }
            public string StatorResistanceColdInOhms { get; set; }
            public string StatorResistanceColdMasureAtTempInC { get; set; }


          
        }
        public class Page3
        {
            public string SpecifiedTemperature { get; set; }
            public string StatorResistanceOhms { get; set; }
            public string StatorResistancemeasureatTempinC { get; set; }

            #region Seven
            public string StatorWindingTempTtInC { get; set; }
            public string AmbientTemperatureIinC { get; set; }
            public string LinetoLineVoltageinV { get; set; }
            public string FrequencyInHz { get; set; }
            public string SynchronousSpeedNsInRPM { get; set; }
            public string ObservedSpeedInRmin { get; set; }
            public string ObservedSlipInRmin { get; set; }
            public string ObservedSlipInPU { get; set; }
            public string CorrectedSlipInPU { get; set; }
            public string CorrectedSpeedInPU { get; set; }
            public string TorqueInNm { get; set; }
            public string DynamometerCorrectionnNm { get; set; }
            public string CorrectedTorqueinNm { get; set; }
            public string ShaftPowerinkW { get; set; }
            public string LineCurrentinA { get; set; }
            public string StatorPowerinkW { get; set; }
            public string StatorI2RLossinkWttt { get; set; }
            public string WindingResistanceatts { get; set; }
            public string StatorI2RLossinkWatts { get; set; }
            public string StatorPowerCorrectioninkW { get; set; }
            public string CorrectedStatorPowerinkW { get; set; }
            public string EfficiencyInPercentage { get; set; }
            public string PowerFactorInPercentage { get; set; }
            #endregion


        }

        public class Page4Charts
        {
            public string ShaftPowerinkW1Common { get; set; }
            public string ShaftPowerinkW2Common { get; set; }
            public string ShaftPowerinkW3Common { get; set; }
            public string ShaftPowerinkW4Common { get; set; }
            public string ShaftPowerinkW5Common { get; set; }
            public string ShaftPowerinkW6Common { get; set; }
            public string ShaftPowerinkW7Common { get; set; }

            public string MeasuredEfficiencyinPercentage1 { get; set; }
            public string MeasuredEfficiencyinPercentage2 { get; set; }
            public string MeasuredEfficiencyinPercentage3 { get; set; }
            public string MeasuredEfficiencyinPercentage4 { get; set; }
            public string MeasuredEfficiencyinPercentage5 { get; set; }
            public string MeasuredEfficiencyinPercentage6 { get; set; }
            public string MeasuredEfficiencyinPercentage7 { get; set; }

            public string MeasuredEfficiencyinAmps1 { get; set; }
            public string MeasuredEfficiencyinAmps2 { get; set; }
            public string MeasuredEfficiencyinAmps3 { get; set; }
            public string MeasuredEfficiencyinAmps4 { get; set; }
            public string MeasuredEfficiencyinAmps5 { get; set; }
            public string MeasuredEfficiencyinAmps6 { get; set; }
            public string MeasuredEfficiencyinAmps7 { get; set; }

            public string MeasuredEfficiencyinRPM1 { get; set; }
            public string MeasuredEfficiencyinRPM2 { get; set; }
            public string MeasuredEfficiencyinRPM3 { get; set; }
            public string MeasuredEfficiencyinRPM4 { get; set; }
            public string MeasuredEfficiencyinRPM5 { get; set; }
            public string MeasuredEfficiencyinRPM6 { get; set; }
            public string MeasuredEfficiencyinRPM7 { get; set; }

            public string MeasuredCosinPercentage1 { get; set; }
            public string MeasuredCosinPercentage2 { get; set; }
            public string MeasuredCosinPercentage3 { get; set; }
            public string MeasuredCosinPercentage4 { get; set; }
            public string MeasuredCosinPercentage5 { get; set; }
            public string MeasuredCosinPercentage6 { get; set; }
            public string MeasuredCosinPercentage7 { get; set; }

        }
    }
}
