using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.Model.Reporting.ViewModel
{
    public class Page1Model
    {
        public Page1Model()
        {
            _tblMotor = new tblMotor();
            _tblShaftPawer = new List<tblShaftPawer>();
            _tblSpeedRPM = new List<tblSpeedRPM>();
            _tblEfficiency = new List<tblEfficiency>();
            _tblCurrentAmps = new List<tblCurrentAmps>();
            _tblCos = new List<tblCos>();
            _tblImages = new tblImages();
        }
        public tblMotor _tblMotor { get; set; }
        public List<tblShaftPawer> _tblShaftPawer { get; set; }
        public List<tblSpeedRPM> _tblSpeedRPM { get; set; }
        public List<tblEfficiency> _tblEfficiency { get; set; }
        public List<tblCurrentAmps> _tblCurrentAmps { get; set; }
        public List<tblCos> _tblCos { get; set; }
        public tblImages _tblImages { get; set; }
    }
}
