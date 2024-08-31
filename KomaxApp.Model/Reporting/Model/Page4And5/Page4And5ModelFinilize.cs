using KomaxApp.Model.Page4And5.Entity;
using KomaxApp.Model.Reporting.Model.Page4And5.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.Model.Reporting.Model.Page4And5
{
    public class Page4And5ModelFinilize // : BaseClass
    {
        public string TestReportNo { get; set; }
        public string Dated { get; set; }
        public string MotorModel { get; set; }
        public string SerialNo { get; set; }
        #region Shaft
        public string ShaftPower1 { get; set; }
        public string ShaftPower2 { get; set; }
        public string ShaftPower3 { get; set; }
        public string ShaftPower4 { get; set; }
        public string ShaftPower5 { get; set; }
        public string ShaftPower6 { get; set; }
        public string ShaftPower7 { get; set; }
        #endregion
        #region Efficiency
        public string Efficiency1 { get; set; }
        public string Efficiency2 { get; set; }
        public string Efficiency3 { get; set; }
        public string Efficiency4 { get; set; }
        public string Efficiency5 { get; set; }
        public string Efficiency6 { get; set; }
        public string Efficiency7 { get; set; }
        #endregion
        #region Speed
        public string Speed1 { get; set; }
        public string Speed2 { get; set; }
        public string Speed3 { get; set; }
        public string Speed4 { get; set; }
        public string Speed5 { get; set; }
        public string Speed6 { get; set; }
        public string Speed7 { get; set; }
        #endregion
       
        #region CurrentInAmps
        public string CurrentInAmps1 { get; set; }
        public string CurrentInAmps2 { get; set; }
        public string CurrentInAmps3 { get; set; }
        public string CurrentInAmps4 { get; set; }
        public string CurrentInAmps5 { get; set; }
        public string CurrentInAmps6 { get; set; }
        public string CurrentInAmps7 { get; set; }
        #endregion
        #region Cos
        public string Cos1 { get; set; }
        public string Cos2 { get; set; }
        public string Cos3 { get; set; }
        public string Cos4 { get; set; }
        public string Cos5 { get; set; }
        public string Cos6 { get; set; }
        public string Cos7 { get; set; }
        #endregion
    }
}
