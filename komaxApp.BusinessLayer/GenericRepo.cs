using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;
using System.Drawing;

namespace komaxApp.BusinessLayer
{
    public class GenericRepo
    {
        public void RefreshPortList(ComboBox targetComboBox, Label infoMessage, string noItemsText = "No COM")
        {
            try
            {
                targetComboBox.Items.Clear();

                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    targetComboBox.Items.Add(port);
                }

                // Select the first item if the list is not empty
                if (targetComboBox.Items.Count > 0)
                {
                    targetComboBox.SelectedIndex = targetComboBox.Items.Count - 1;
                }
                else
                {
                    targetComboBox.Text = noItemsText;
                }
            }
            catch (Exception ex)
            {
                infoMessage.Text  = ex.Message;
                targetComboBox.Text = noItemsText;
            }
        }
    }
}
