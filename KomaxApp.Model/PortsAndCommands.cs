using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.Model
{
    public static class PortsAndCommands
    {
        public const string COM6_MEAS = ":MEAS?";
        public const string COM5_x23x30x30x30x0d = "x23 x30 x30 x30 x0d";
        public const string COM4_x05x01x00x00x00x00x06xAA = "x05 x01 x00 x00 x00 x00 x06 xAA";
        public const string COM7_Empty = "";
    }
}
