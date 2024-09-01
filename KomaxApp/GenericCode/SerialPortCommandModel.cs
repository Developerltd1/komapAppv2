using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KomaxApp.GenericCode
{
    public class SerialPortCommandModel
    {
        public SerialPort SerialPort { get; set; }
        public string CommandName { get; set; }

        public SerialPortCommandModel(SerialPort serialPort, string commandName)
        {
            SerialPort = serialPort;
            CommandName = commandName;
        }
    }

}
