using KomaxApp.Model.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.Model.ViewModel
{
    public class VmCreateMotor
    {
        public VmCreateMotor()
        {
            createModel = new CreateModel.Request();
            shaftPowerObj = new ShaftPowerObj.Request();
            efficiencyObj = new EfficiencyObj.Request();
            speedInRPMObj = new SpeedInRPMObj.Request();
            currentInAmpsObj = new CurrentInAmpsObj.Request();
            cosObj = new CosObj.Request();
            lstShaftPower = new List<ShaftPowerObj.Request>();
            lstEfficiency = new List<EfficiencyObj.Request>();
            lstSpeedInRPM = new List<SpeedInRPMObj.Request>();
            lstCurrentInAmps = new List<CurrentInAmpsObj.Request>();
            lstCos  =   new List<CosObj.Request>();
            imageObj = new ImageObj.Request();
        }
        public CreateModel.Request createModel { get; set; }
        public ShaftPowerObj.Request shaftPowerObj { get; set; }
        public EfficiencyObj.Request efficiencyObj { get; set; }
        public SpeedInRPMObj.Request speedInRPMObj { get; set; }
        public CurrentInAmpsObj.Request currentInAmpsObj { get; set; }
        public CosObj.Request cosObj { get; set; }


        public ImageObj.Request  imageObj { get; set; }

    public List<ShaftPowerObj.Request> lstShaftPower { get; set; }
        public List<EfficiencyObj.Request> lstEfficiency { get; set; }
        public List<SpeedInRPMObj.Request> lstSpeedInRPM { get; set; }
        public List<CurrentInAmpsObj.Request> lstCurrentInAmps { get; set; }

        public List<CosObj.Request> lstCos { get; set; }

    }


}
