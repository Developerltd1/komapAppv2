using KomaxApp.Model.Dashboard;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KomaxApp.GenericCode
{
    public class SerialPortCode
    {
        // Define an event to handle data received
        public event Action<string> OnDataReceived;


        // Initializes multiple serial ports
        public void InitializeMultipleSerialPorts(List<string> comPorts, List<string> commands)
        {
            var serialPorts = new Dictionary<string, SerialPort>();

            for (int i = 0; i < comPorts.Count; i++)
            {
                string comPort = comPorts[i];
                string command = commands[i]; // Corresponding command for the port
                InitializeSerialPort(comPort, command, serialPorts);
            }
        }

        // Initializes a single serial port
        private void InitializeSerialPort(string comPort, string command, Dictionary<string, SerialPort> serialPorts)
        {
            try
            {
                // Check if the serial port is already initialized
                if (!serialPorts.ContainsKey(comPort))
                {
                    // Initialize the serial port with common settings
                    SerialPort serialPort = new SerialPort
                    {
                        PortName = comPort,
                        BaudRate = 9600,
                        Parity = Parity.None,
                        DataBits = 8,
                        StopBits = StopBits.One,
                        Handshake = Handshake.None,
                        ReadTimeout = 90000  // Set an optional timeout
                    };

                    try
                    {
                        // Open the serial port
                        if (!serialPort.IsOpen)
                        {
                            serialPort.Open();//dynamic
                        }
                        serialPorts[comPort] = serialPort; // Add the port to the dictionary
                    }
                    catch (Exception ex)
                    {
                        // Handle errors during the port opening process
                        Console.WriteLine($"Error opening serial port {comPort}: {ex.Message}");
                        return;
                    }
                   
                    // Set up the DataReceived event handler for the serial port
                    serialPort.DataReceived += (sender, e) => DataReceivedHandler(sender, e);
                    // Send the initial command if provided
                    if (!string.IsNullOrEmpty(command))
                    {
                      
                       

                        byte[] commandBytes = command.Split(' ')
                .Select(hex => Convert.ToByte(hex, 16))
                .ToArray();

                        //byte[] commandBytes = Encoding.ASCII.GetBytes(command + "\r\n");
                        serialPort.Write(commandBytes, 0, commandBytes.Length);  //dynamic
                        //serialPort.Write(command);  //dynamic
                    }

                }
                else
                {
                    // If the port is already initialized, perform the logic that should run every second
                    var serialPort = serialPorts[comPort];
                    if (!serialPort.IsOpen)
                    {
                        try
                        {
                            // Attempt to open the serial port if it is not open
                             serialPort.Open();   //dynamic
                        }
                        catch (Exception ex)
                        {
                            // Handle errors during the port opening process
                            Console.WriteLine($"Error reopening serial port {comPort}: {ex.Message}");
                            return;
                        }
                    }
                    try
                    {
                        // Send the command again if provided
                        if (!string.IsNullOrEmpty(command))
                        {
                            byte[] commandBytes = Encoding.ASCII.GetBytes(command + "\r\n");
                             serialPort.Write(commandBytes, 0, commandBytes.Length);  //dynamic
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle errors during the command sending process
                        Console.WriteLine($"Error sending command to serial port {comPort}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Display an error message and handle cleanup
                Console.WriteLine($"Error initializing serial port {comPort}: {ex.Message}");

                // Clean up resources if initialization fails
                if (serialPorts.ContainsKey(comPort))
                {
                    serialPorts[comPort].Close();
                    serialPorts.Remove(comPort);
                }
            }
        }

        // Handles data received from the serial port
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                string commandName = sp.PortName;
                string inData = sp.ReadLine();
                if (!string.IsNullOrEmpty(inData))
                {
                    // Trigger the OnDataReceived event
                    OnDataReceived?.Invoke(inData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while processing data: " + ex.Message);
            }
        }

        // Parse the response data
        public void ParseResponse(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                Console.WriteLine("No data received.");
                return;
            }
        

           


        }
    }
}

