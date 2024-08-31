using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.Model
{
    public static class DummyData
    {
        public static class btnRecordNoLoadPoint_Click
        {
            public static int GetReportNo() => 15;  
            public static double GetTorqueNm() => 1.1;
            public static double GetSpeedRPM() => 1.2;
            public static double GetShaftPowerKw() => 1.3;
            public static double GetLoadingFactor() => 1.4;
            public static double GetMotorSize() => 1.5;

            public static double GetVoltageV1() => 1.1;
            public static double GetVoltageV2() => 1.2;
            public static double GetVoltageV3() => 1.3;
            public static double GetVoltageV4() => 1.4;
            public static double GetCurrentA1() => 1.1;
            public static double GetCurrentA2() => 1.2;
            public static double GetCurrentA3() => 1.3;
            public static double GetCurrentA4() => 1.4;
            public static double GetFrequencyHZ() => 0.1;
            public static double GetFrequency_Value1() => 1.1;
            public static double GetFrequency_Value2() => 1.2;
            public static double GetFrequency_Value3() => 1.3;
            public static double GetFrequency_Value4() => 1.4;
            public static double GetActivePower1() => 1.1;
            public static double GetActivePower2() => 1.2;
            public static double GetActivePower3() => 1.3;
            public static double GetActivePower4() => 1.4;

            public static double GetAmbientTemperature() => 1.1;
            public static double GetMotorTemperature() => 1.2;
            public static double GetEstimitedEfficiency() => 1.3;
        }
    }
}
