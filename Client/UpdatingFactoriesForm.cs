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
    public partial class UpdatingFactoriesForm : Form
    {
        private List<FactoryInfo> factories = null;
        private FactoryInfo currentFactory = null;
        private Form1 parent = null;

        public UpdatingFactoriesForm(List<FactoryInfo> factories, Form1 parent)
        {
            InitializeComponent();
            this.factories = factories;

            if (factories.Count <= 0)
                return;

            for (int i = 0; i < factories.Count; i++)
            {
                comboBox1.Items.Add(factories[i].predprID);
                comboBox2.Items.Add(factories[i].name);
                comboBox3.Items.Add(factories[i].address);
            }

            this.parent = parent;
        }

        private void comboBoxesSelected(object sender, EventArgs e)
        {
            comboBox1.SelectedIndexChanged -= comboBoxesSelected;
            comboBox2.SelectedIndexChanged -= comboBoxesSelected; 
            comboBox3.SelectedIndexChanged -= comboBoxesSelected;
            ComboBox box = (ComboBox) sender;

            currentFactory = factories[box.SelectedIndex];
            comboBox1.SelectedItem = currentFactory.predprID;
            comboBox2.SelectedItem = currentFactory.name;
            comboBox3.SelectedItem = currentFactory.address;

            comboBox1.SelectedIndexChanged += comboBoxesSelected;
            comboBox2.SelectedIndexChanged += comboBoxesSelected;
            comboBox3.SelectedIndexChanged += comboBoxesSelected;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int factoryID = Convert.ToInt32(comboBox1.Text);
            string name = comboBox2.Text;
            string address = comboBox3.Text;
            FactoryInfo newFactory = new FactoryInfo(factoryID, name, address);
            parent.AddFactory(newFactory);
        }
    }
}
