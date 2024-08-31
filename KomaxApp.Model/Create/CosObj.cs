using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.Model.Create
{
    public class CosObj
    {
        public class Request
        {
            public int ReportNo { get; set; }
            public int? Row { get; set; }
            public int? RowNo { get; set; }
            public DateTime EntryDateTime { get; set; } = DateTime.Now;
        }
        public class Response
        {
            public object response { get; set; }
        }
    }
}
