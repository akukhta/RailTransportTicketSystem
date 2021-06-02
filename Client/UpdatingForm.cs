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

        public UpdatingForm(List<EmployeesFactoryInfo> employeesFactories)
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
    }
}
