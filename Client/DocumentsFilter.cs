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
    public partial class DocumentsFilter : Form
    {
        private List<BussinesTripInfo> documents = null;
        private List<BussinesTripInfo> sortedDocuments = null;
        private Form1 parent = null;

        public DocumentsFilter(List<BussinesTripInfo> documents, Form1 form1)
        {
            InitializeComponent();

            this.parent = form1;

            comboBox1.SelectedIndexChanged -= ChangeBoxesChanged;
            comboBox2.SelectedIndexChanged -= ChangeBoxesChanged;

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;


            comboBox1.SelectedIndexChanged += ChangeBoxesChanged;
            comboBox2.SelectedIndexChanged += ChangeBoxesChanged;

            if (documents.Count <= 0)
                return;
            
            this.documents = documents;

            ChangeBoxesChanged(null, null);

            foreach (BussinesTripInfo info in documents)
            {
                comboBox1.Items.Add(info.destinationPlace);
                string fullname = info.name + " " + info.surname + " " + info.patronymic;
                comboBox2.Items.Add(fullname);
            }
        }

        private List<BussinesTripInfo> filter()
        {
            sortedDocuments = new List<BussinesTripInfo>(documents);

            for (int i = 0; i < documents.Count; i++)
            {
                if (comboBox1.SelectedIndex != 0)
                {
                    if (documents[i].destinationPlace != comboBox1.SelectedItem.ToString())
                    {
                        sortedDocuments.Remove(documents[i]);
                    }
                }
                
                if (comboBox2.SelectedIndex != 0)
                {
                    string fullName = documents[i].name + " " + documents[i].surname + " " + documents[i].patronymic;

                    if (fullName != comboBox2.SelectedItem.ToString())
                    {
                        sortedDocuments.Remove(documents[i]);
                    }
                }

                if (documents[i].from.Date.Ticks < dateTimePicker1.Value.Date.Ticks)
                {
                    sortedDocuments.Remove(documents[i]);
                }

                if (documents[i].to.Date.Ticks > dateTimePicker2.Value.Date.Ticks)
                {
                    sortedDocuments.Remove(documents[i]);
                }

            }

            return sortedDocuments;
        }

        private void ChangeBoxesChanged(object sender, EventArgs e)
        {
            parent.LoadDocuments(filter());
        }
    }
}
