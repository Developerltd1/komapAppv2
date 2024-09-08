using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.Model.LoadTest
{
    public class RequestLoadTestModel
    {
        public class Request
        {
            public int? ReportNo { get; set; }
            public double? TorqueNm { get; set; }
            public double? SpeedRPM { get; set; }
            public double? ShaftPowerkW { get; set; }

            public double? LoadingFactor { get; set; }

            public double? MotorSize { get; set; }
                       
            public double? VoltageV_Value1 { get; set; }
            public double? VoltageV_Value2 { get; set; }
            public double? VoltageV_Value3 { get; set; }
            public double? VoltageV_Value4 { get; set; }
            public double? CurrentA_Value1 { get; set; }
            public double? CurrentA_Value2 { get; set; }
            public double? CurrentA_Value3 { get; set; }
            public double? CurrentA_Value4 { get; set; }
            public double? ActivePower_Value1 { get; set; }
            public double? ActivePower_Value2 { get; set; }
            public double? ActivePower_Value3 { get; set; }
            public double? ActivePower_Value4 { get; set; }
            public double? Frequency_Value1 { get; set; }
            public double? Frequency_Value2 { get; set; }
            public double? Frequency_Value3 { get; set; }
            public double? Frequency_Value4 { get; set; }
            public double? FrequencyHZ { get; set; }
                     
            public double? AmbientTemperature { get; set; }
            public double? MotorTemperature { get; set; }
            public double? EstimitedEfficiency { get; set; }
            public DateTime? EntryDate { get; set; }

            public double? Pt100_Temp1 { get; set; }
            public double? Pt100_Temp2 { get; set; }

        }
        public class Response
        {
            public int StatusCode { get; set; }
            public string StatusDetails { get; set; }
            public int LabelCount { get; set; }
            public string LabelStatus { get; set; }
        }




        #region LabelCountModel
        public class LabelCountModel
        {
            public int? ReportNo { get; set; }
            public int? LabelCount { get; set; }
            public string LoadStatus { get; set; }
            public double? TorqueNm { get; set; }
            public double? SpeedRPM { get; set; }
            public double? ShaftPowerkW { get; set; }

            public double? LoadingFactor { get; set; }

            public double? MotorSize { get; set; }

            public double? VoltageV_Value1 { get; set; }
            public double? VoltageV_Value2 { get; set; }
            public double? VoltageV_Value3 { get; set; }
            public double? VoltageV_Value4 { get; set; }
            public double? CurrentA_Value1 { get; set; }
            public double? CurrentA_Value2 { get; set; }
            public double? CurrentA_Value3 { get; set; }
            public double? CurrentA_Value4 { get; set; }
            public double? ActivePower_Value1 { get; set; }
            public double? ActivePower_Value2 { get; set; }
            public double? ActivePower_Value3 { get; set; }
            public double? ActivePower_Value4 { get; set; }
            public double? Frequency_Value1 { get; set; }
            public double? Frequency_Value2 { get; set; }
            public double? Frequency_Value3 { get; set; }
            public double? Frequency_Value4 { get; set; }
            //public double? FrequencyHZ { get; set; }

            public double? AmbientTemperature { get; set; }
            public double? MotorTemperature { get; set; }
            public double? EstimitedEfficiency { get; set; }
            public DateTime? EntryDate { get; set; }

        }
        #endregion
    }
}
