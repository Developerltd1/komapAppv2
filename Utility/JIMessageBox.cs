using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utility
{
    public class JIMessageBox
    {
        public static void ErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void InformationMessage(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void AsteriskMessage(string message)
        {
            MessageBox.Show(message, "Asterisk", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        public static void ExclamationMessage(string message)
        {
            MessageBox.Show(message, "Exclamation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        public static void HandMessage(string message)
        {
            MessageBox.Show(message, "Hand", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        public static void QuestionMessage(string message)
        {
            MessageBox.Show(message, "Question", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }
        public static void StopMessage(string message)
        {
            MessageBox.Show(message, "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        public static void WarningMessage(string message)
        {
            MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static void NoneMessage(string message)
        {
            MessageBox.Show(message, "None", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
        public static void CustomMessage(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK);
        }
    }
}
