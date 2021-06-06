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
    public partial class UpdatingFormEmployees : Form
    {
        private List<User> users = null;
        private User currentUser = null;
        private Form1 parent = null;

        public UpdatingFormEmployees(List<User> users, Form1 parent)
        {
            InitializeComponent();
            this.users = users;

            if (users.Count <= 0)
                return;

            for (int i = 0; i < users.Count; i++)
            {
                comboBox1.Items.Add(users[i].userID);
                comboBox2.Items.Add(users[i].name);
                comboBox3.Items.Add(users[i].surname);
                comboBox4.Items.Add(users[i].patronymic);
                comboBox5.Items.Add(users[i].passportSeries);
                comboBox6.Items.Add(users[i].passportNumber);
                comboBox8.Items.Add(users[i].userType == 0 ? "Пользователь" : "Администратор");
                comboBox9.Items.Add(users[i].job);
            }

            this.parent = parent;
        }

        private bool CheckUserInput()
        {
            return false;
        }

        private void BoxesChanged(object sender, EventArgs e)
        {
            comboBox1.SelectedIndexChanged -= BoxesChanged;
            comboBox2.SelectedIndexChanged -= BoxesChanged;
            comboBox3.SelectedIndexChanged -= BoxesChanged;
            comboBox4.SelectedIndexChanged -= BoxesChanged;
            comboBox5.SelectedIndexChanged -= BoxesChanged;
            comboBox6.SelectedIndexChanged -= BoxesChanged;
            comboBox8.SelectedIndexChanged -= BoxesChanged;
            comboBox9.SelectedIndexChanged -= BoxesChanged;

            ComboBox box = (ComboBox)sender;
            currentUser = users[box.SelectedIndex];

            comboBox1.SelectedItem = currentUser.userID;
            comboBox2.SelectedItem = currentUser.name;
            comboBox3.SelectedItem = currentUser.surname;
            comboBox4.SelectedItem = currentUser.patronymic;
            comboBox5.SelectedItem = currentUser.passportSeries;
            comboBox6.SelectedItem = currentUser.passportNumber;
            dateTimePicker1.Value = currentUser.birthday;
            comboBox8.SelectedItem = currentUser.userType == 0 ? "Пользователь" : "Администратор";
            comboBox9.SelectedItem = currentUser.job;

            comboBox1.SelectedIndexChanged += BoxesChanged;
            comboBox2.SelectedIndexChanged += BoxesChanged;
            comboBox3.SelectedIndexChanged += BoxesChanged;
            comboBox4.SelectedIndexChanged += BoxesChanged;
            comboBox5.SelectedIndexChanged += BoxesChanged;
            comboBox6.SelectedIndexChanged += BoxesChanged;
            comboBox8.SelectedIndexChanged += BoxesChanged;
            comboBox9.SelectedIndexChanged += BoxesChanged;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(comboBox1.Text);
            string name = comboBox2.Text;
            string surname = comboBox3.Text;
            string patronymic = comboBox4.Text;
            string passS = comboBox5.Text;
            string passN = comboBox6.Text;
            DateTime date = dateTimePicker1.Value.Date;
            int userType = comboBox8.Text == "Пользователь" ? 0 : 1;
            string job = comboBox9.Text;
            string gender = comboBox7.Text;
            User user = new User(false, userType, userID, name, surname, patronymic, passS, passN, job, gender, date, 0);

            parent.AddUser(user);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(comboBox1.Text);

            User user = new User(false, 0, userID);
            parent.DeleteUser(user);
        }
    }
}
