using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.Model.Reporting.Model.Page2
{
    public class tblLoadTest
    {
        public int ReportNo { get; set; }
        public decimal TorqueNm { get; set; }
        public decimal SpeedRPM { get; set; }
        public decimal ShaftPowerkW { get; set; }
        public decimal LoadingFactor { get; set; }
        public decimal MotorSize { get; set; }
        public decimal VoltageV_Value1 { get; set; }
        public decimal VoltageV_Value2 { get; set; }
        public decimal VoltageV_Value3 { get; set; }
        public decimal VoltageV_Value4 { get; set; }
        public decimal CurrentA_Value1 { get; set; }
        public decimal CurrentA_Value2 { get; set; }
        public decimal CurrentA_Value3 { get; set; }
        public decimal CurrentA_Value4 { get; set; }
        public decimal ActivePower_Value1 { get; set; }
        public decimal ActivePower_Value2 { get; set; }
        public decimal ActivePower_Value3 { get; set; }
        public decimal ActivePower_Value4 { get; set; }
        public decimal FrequencyHZ { get; set; }
        public decimal Frequency_Value1 { get; set; }
        public decimal Frequency_Value2 { get; set; }
        public decimal Frequency_Value3 { get; set; }
        public decimal Frequency_Value4 { get; set; }
        public decimal AmbientTemperature { get; set; }
        public decimal MotorTemperature { get; set; }
        public decimal stimitedEfficiency { get; set; }
        public int LabelCount { get; set; }
        public string LoadStatus { get; set; }

    }
}
