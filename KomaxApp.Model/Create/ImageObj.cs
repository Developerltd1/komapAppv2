using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.Model.Create
{
    public class ImageObj
    {
        public class Request
        {
            public int ReportNo { get; set; }
            public string Image { get; set; }
            public DateTime EntryDateTime { get; set; } = DateTime.Now;
        }
        public class Response
        {
            public object response { get; set; }
        }

    }
}
