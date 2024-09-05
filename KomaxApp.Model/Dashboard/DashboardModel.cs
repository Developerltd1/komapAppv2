using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.Model.Dashboard
{
    public class DashboardModel
    {
        public class Manupulation
        {

            public string labelV1 { get; set; }
            public string labelV2 { get; set; }
            public string labelV3 { get; set; }
            public string labelV0 { get; set; }
            public string labelA1 { get; set; }
            public string labelA2 { get; set; }
            public string labelA3 { get; set; }
            public string labelA0 { get; set; }
            public string labelPf1 { get; set; }
            public string labelPf2 { get; set; }
            public string labelPf3 { get; set; }
            public string labelPf0 { get; set; }
            public string labelHertz { get; set; }
            public string labelPower1 { get; set; }
            public string labelPower2 { get; set; }
            public string labelPower3 { get; set; }
            public string labelPower0 { get; set; }

            public string _tbTorqueNm { get; set; }
            public string _tbSpeedRPM { get; set; }
            public string _tbLoadingFactorPercentage { get; set; }
            public string _tbMototSizeHP { get; set; }
            public string _tbShaftPawerKw { get; set; }
            public string _tbserialResponseCOM7Temp1 { get; set; }
            public string __tbserialResponseCOM7Temp2 { get; set; }

        }
        public class SerialResponseModel
        {
            public string _serialResponseCOM4  { get; set; }
            public string _serialResponseCOM5  { get; set; }
            public string _serialResponseCOM6  { get; set; }
            public string _serialResponseCOM7Temp1  { get; set; }
            public string _serialResponseCOM7Temp2 { get; set; }
        }
    }
}
