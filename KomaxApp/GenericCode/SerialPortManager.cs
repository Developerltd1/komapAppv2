using System;
using System.IO.Ports;

namespace KomaxApp.GenericCode
{
    public class SerialPortManager
    {

        public byte[] HexStringToByteArray(string hex)
        {
            // Remove spaces and convert to upper case
            hex = hex.Replace(" ", "").Replace("x", "").ToUpper();
            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException("Invalid length of hex string.");
            }

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }











        public event EventHandler<string> DataReceived;
        public event EventHandler<string> ErrorOccurred;

        private SerialPort serialPort;

        public void InitializeSerialPort(string comPort, int baudRate = 9600, int readTimeout = 90000)
        {
            try
            {
                serialPort = new SerialPort
                {
                    PortName = comPort,
                    BaudRate = baudRate,
                    Parity = Parity.None,
                    DataBits = 8,
                    StopBits = StopBits.One,
                    Handshake = Handshake.None,
                    ReadTimeout = readTimeout
                };

                serialPort.DataReceived += SerialPort_DataReceived;
                serialPort.Open();
            }
            catch (Exception ex)
            {
                OnErrorOccurred($"Error initializing serial port {comPort}: {ex.Message}");
                serialPort?.Close();
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                var PortName = sp.PortName;
                string inData = sp.ReadLine();
                if (!string.IsNullOrEmpty(inData))
                {
                    OnDataReceived(inData);
                }
            }
            catch (Exception ex)
            {
                OnErrorOccurred($"Error receiving data: {ex.Message}");
            }
        }

        public void CloseSerialPort()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        protected virtual void OnDataReceived(string data)
        {
            DataReceived?.Invoke(this, data);
        }

        protected virtual void OnErrorOccurred(string errorMessage)
        {
            ErrorOccurred?.Invoke(this, errorMessage);
        }

    }
}
