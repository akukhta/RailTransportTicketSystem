using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class UpdatingForm : Form
    {
        List<EmployeesFactoryInfo> employeesFactoryInfos = null;
        EmployeesFactoryInfo currentEmployesFactpry = null;
        private Form1 parent = null;

        public UpdatingForm(List<EmployeesFactoryInfo> employeesFactories, Form1 parent)
        {
            InitializeComponent();

            this.employeesFactoryInfos = employeesFactories;

            if (employeesFactories.Count <= 0)
                return;
         
            for (int i = 0; i < employeesFactoryInfos.Count; i++)
            {
                comboBox1.Items.Add(employeesFactoryInfos[i].UserID);
                comboBox2.Items.Add(employeesFactoryInfos[i].FactoryID);
            }

            this.parent = parent;

        }

        private void UpdatingForm_Load(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BoxesChanged(object sender, EventArgs e)
        {
            comboBox1.SelectedIndexChanged -= BoxesChanged;
            comboBox2.SelectedIndexChanged -= BoxesChanged;

            ComboBox box = (ComboBox)sender;

            currentEmployesFactpry = employeesFactoryInfos[box.SelectedIndex];
            comboBox1.SelectedItem = currentEmployesFactpry.UserID;
            comboBox2.SelectedItem = currentEmployesFactpry.FactoryID;

            comboBox1.SelectedIndexChanged += BoxesChanged;
            comboBox2.SelectedIndexChanged += BoxesChanged;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int factoryID = Convert.ToInt32(comboBox2.Text);
            int userID = Convert.ToInt32(comboBox1.Text);

            FactoryInfo info = new FactoryInfo(factoryID, "", "");
            User user = new User(false, 1, userID);
            parent.DeleteSotrFactory(user, info);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int factoryID = Convert.ToInt32(comboBox2.Text);
            int userID = Convert.ToInt32(comboBox1.Text);

            FactoryInfo info = new FactoryInfo(factoryID, "", "");
            User user = new User(false, 1, userID);
            parent.AddFactoryUser(info, user);
        }
    }
}
