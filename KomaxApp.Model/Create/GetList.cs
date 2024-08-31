using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.Model.Create
{
    public class GetList
    {
        public int ReportNo { get; set; }
        public DateTime TestDate { get; set; }
        public string Manufacturer { get; set; }
        public string MotorModel { get; set; }
        public string MotorType { get; set; }
        public string Frame { get; set; }
        public int Phase { get; set; }
        public int MotorRatedKw { get; set; }
        public int MotorRatedHP { get; set; }
        public int MotorRatedVoltage { get; set; }
    }
}
