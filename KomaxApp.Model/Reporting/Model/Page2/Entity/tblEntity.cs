using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.Model.Reporting.Model.Page2.Entity
{
    public class tblEntity
    {
        public string FrequencyInHz { get; set; }
        public string LineoLineVoltageinV { get; set; }
        public string LineCurrentInA { get; set; }
        public string PowerFactorInPU { get; set; }
        public string StatorPowerinkW { get; set; }
        public string ObservedSpeedinRmin { get; set; }
        public string StatorWindingTemperatureInC { get; set; }
        public string CorrectedSpeedInRmin { get; set; }
    }
}
