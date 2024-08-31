using KomaxApp.Model.Reporting.Model.Page1;
using KomaxApp.Model.Reporting.Model.Page2;
using KomaxApp.Model.Reporting.Model.Page3;
using KomaxApp.Model.Reporting.Model.Page4And5;
using KomaxApp.Model.Reporting.Model.Page4And5.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.Model.Reporting
{
    public class ReportingModel
    {
        public ReportingModel()
        {
            page1Mdl = new Page1ModelFinilize();
            page2Mdl = new Page2ModelFinilize();
            page3Mdl = new Page3ModelFinilize();
            page4And5Mdl = new Page4And5ModelFinilize();
            _Chart1Model = new List<Chart1Model>();
        }
        public Page1ModelFinilize page1Mdl { get; set; }
        public Page2ModelFinilize page2Mdl { get; set; }
        public Page3ModelFinilize page3Mdl { get; set; }
        public Page4And5ModelFinilize page4And5Mdl { get; set; }

        public List<Chart1Model> _Chart1Model { get; set; }
    }
}
