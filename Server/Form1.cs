using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        private Server server;

        private void test()
        {
            listView1.Items.Add("g776");
            listView1.Items.Add("g54");
            listView1.Items.Add("g43");          
        }
        public Form1()
        {
            InitializeComponent();
            listView1.View = System.Windows.Forms.View.List;
            test();
            server = new Server(new UIUpdater(this));
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            server.start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            server.stop();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
