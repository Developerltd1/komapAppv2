using KomaxApp.Model.Reporting.Model.Page2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.Model.Reporting.ViewModel
{
    public class Page2Model
    {
        public Page2Model()
        {
            _tblLoadTest = new List<tblLoadTest>();
        }
        public List<tblLoadTest> _tblLoadTest { get; set; }
    }
}
