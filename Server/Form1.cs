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
        private void test()
        {
            listView1.Items.Add("g776");
            listView1.Items.Add("g54");
            listView1.Items.Add("g43");
            Database database = new Database();
        }
        public Form1()
        {
            InitializeComponent();

            try
            {
                db = new Database();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                db = null;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private Database db;
    }
}
