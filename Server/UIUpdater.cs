using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    class UIUpdater
    {
        private Form1 UI = null;

        public UIUpdater(Form1 UI)
        {
            this.UI = UI;
        }

        public void LogMessage(string message)
        {
            UI.richTextBox1.Invoke((new MethodInvoker(() => UI.richTextBox1.Text += message + "\n")));
        }

        public void AddClient(string clientIP)
        {
            UI.listView1.Invoke((new MethodInvoker(() => UI.listView1.Items.Add(clientIP))));
        }
    }
}
