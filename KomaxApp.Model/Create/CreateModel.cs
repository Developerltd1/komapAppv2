using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.Model.Create
{
    public class CreateModel
    {
        public class Request
        {
            public int ReportNo { get; set; }
            public DateTime TestDate { get; set; }
            public string HitachiCruve { get; set; }
            public string Manufacturer { get; set; }
            public string MotorModel { get; set; }
            public string MotorType { get; set; }
            public string Frame { get; set; }
            public int Phase { get; set; }
            public int MotorRatedKw { get; set; }
            public int MotorRatedHP { get; set; }
            public int MotorRatedVoltage { get; set; }
            public int MotorRatedFrequency   { get; set; }  //HERTZ
            public int MotorRatedCurrent  { get; set; } //AMPS
            public int MotorRatedPowerFactor  { get; set; }  //PF
            public int MotorRatedRPM   { get; set; }  //RPM
            public int NofPoles  { get; set; } //Pole
            public double Efficency { get; set; }
            public int Duty_SerivceFactor { get; set; }
            public string InsulationClass  { get; set; }  //InsClass
            public string CoolingClass { get; set; }  //IC_
            public string IPRating { get; set; }
            public string ConnectionType { get; set; }
            public string SerialNo { get; set; }
            public string Picture { get; set; }
            public int SpecifiedTemperature { get; set; }
            public int WindingResistanceinOhm { get; set; }
            public int TempAtWindingResistanceIsMeasured { get; set; }
            public string rbDescription { get; set; }

            public bool RatedCurves { get; set; }
            
        }
        public class Response
        {
            public int StatusCode { get; set; }
            public string StatusDetails { get; set; }
        }
    }
}
