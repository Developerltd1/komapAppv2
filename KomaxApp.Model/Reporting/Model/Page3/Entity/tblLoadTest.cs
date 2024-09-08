using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.Model.Reporting.Model.Page3.Entity
{
    public class tblLoadTest
    {
        public string TorqueNm { get; set; }
        public string SpeedRPM { get; set; }
        public string ShaftPowerkW { get; set; }
        public string LoadingFactor { get; set; }
        public string MotorSize { get; set; }
        public string VoltageV_Value1 { get; set; }
        public string VoltageV_Value2 { get; set; }
        public string VoltageV_Value3 { get; set; }
        public string VoltageV_Value4 { get; set; }
        public string CurrentA_Value1 { get; set; }
        public string CurrentA_Value2 { get; set; }
        public string CurrentA_Value3 { get; set; }
        public string CurrentA_Value4 { get; set; }
        public string ActivePower_Value1 { get; set; }
        public string ActivePower_Value2 { get; set; }
        public string ActivePower_Value3 { get; set; }
        public string ActivePower_Value4 { get; set; }
        public string FrequencyHZ { get; set; }
        public string Frequency_Value1 { get; set; }
        public string Frequency_Value2 { get; set; }
        public string Frequency_Value3 { get; set; }
        public string Frequency_Value4 { get; set; }
        public string AmbientTemperature { get; set; }
        public string MotorTemperature { get; set; }
        public string EstimitedEfficiency { get; set; }
        public string Pt100_Temp1 { get; set; }
        public string Pt100_Temp2 { get; set; }
        public int LabelCount { get; set; }
        public string LoadStatus { get; set; }
    }
}
