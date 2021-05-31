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

        public UpdatingFactoriesForm(List<FactoryInfo> factories)
        {
            InitializeComponent();
            this.factories = factories;
            
            for (int i = 0; i < factories.Count; i++)
            {
                comboBox1.Items.Add(factories[i].predprID);
                comboBox2.Items.Add(factories[i].name);
                comboBox3.Items.Add(factories[i].address);
            }
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
    }
}
