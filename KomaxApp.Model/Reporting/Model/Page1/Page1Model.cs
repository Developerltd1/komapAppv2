using KomaxApp.Model.Reporting.Model.Page1.Structure;
using System.Collections.Generic;
using KomaxApp.Model.Page1.Entity;
using KomaxApp.Model.Page1.StructureMulti;
namespace KomaxApp.Model.Page1
{
    public class Page1Model
    {
        public Page1Model()
        {
            singleMotorModel = new tblMotor();
            singleImageModel = new tblImages();
            lstShaftPawer = new List<ShaftPowerP2inK>();
            lstEfficiency = new List<EfficiencyInPercentagge>();
            lstSpeedRPM = new List<SpeedInRPM>();
            lstSlipInPU = new List<SlipInPU>();
            lstCurrentAmps = new List<CurrentInAmps>();
            lstCos = new List<CosInPercentage>();
        }
        public tblMotor singleMotorModel { get; set; }
        public tblImages singleImageModel { get; set; }
        public List<ShaftPowerP2inK> lstShaftPawer { get; set; }
        public List<EfficiencyInPercentagge> lstEfficiency { get; set; }
        public List<SpeedInRPM> lstSpeedRPM { get; set; }
        public List<SlipInPU> lstSlipInPU { get; set; }
        public List<CurrentInAmps> lstCurrentAmps { get; set; }
        public List<CosInPercentage> lstCos { get; set; }
    }


}




