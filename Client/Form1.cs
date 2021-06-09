using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        private ClientConnection client;
        private User user;
        private List<User> loadedUsers = null;
        private List<FactoryInfo> LoadedFactories = null;
        private List<EmployeesFactoryInfo> LoadedFactoriesEmployees = null;
        private List<BussinesTripInfo> loadedDocuments = null;
        private FactoryInfo currentDestinationPlace = null;
        private Action currentFunc = null;
        private Form subForm = null;

        private void test()
        {
            
            //TODO:
            //1. Регистрация пользователя с паролем + фикс гендера (+- 1 час) ++++
            //2. Скачивание уже созданных документов (+- 2 часа)
        }

        public Form1(ref ClientConnection client, ref User user)
        {
            InitializeComponent();
            test();
            fake();
            this.client = client;
            this.user = user;

            getFactories();
            getUsers();
            getFactoriesEmployees();
            getDocuments();

            UpdateSubPanel(0);

            comboBox5.SelectedIndex = 0;

            if (this.user.userType == 1)
            {
                string fullName = user.job + " " + user.name + " " + user.surname + " " + user.patronymic;
                textBox2.Text = fullName;
            }

            else
            {
                string fullName = user.name + " " + user.surname + " " + user.patronymic;
                comboBox1.Text = fullName;
                comboBox1.Items.Add(fullName);
                textBox1.Text = user.job;
                tabControl1.TabPages.Remove(tabControl1.TabPages[3]);
                tabControl1.TabPages.Remove(tabControl1.TabPages[2]);
            }

        }

        /// <summary>
        /// Проверяет введены ли все данные. Володя, доделай это.
        /// </summary>
        /// <returns></returns>
        private bool CheckUserInput()
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Не введено имя");
                return false;
            }
            
            if (textBox1.Text == "")
            {
                MessageBox.Show("Отсутствует должность сотрудника");
                return false;
            }
            
            if (comboBox3.Text == "")
            {
                MessageBox.Show("Отсутствует путь командировки");
                return false;
            }
            
            if (richTextBox1.Text == "")
            {
                MessageBox.Show("Отсутствует причина командировки");
                return false;
            }

            if (textBox2.Text == "")
            {
                MessageBox.Show("Отсутствует ФИО отправителя");
                return false;
            }


            return true;
        }

        private void UpdateSubPanel(int type)
        {
            if (subForm != null)
            {
                this.panel3.Controls.Remove(subForm);
                subForm.Close();
            }

            switch (type)
            {
                case 0:
                    subForm = new UpdatingFormEmployees(loadedUsers, this);
                    break;

                case 1:
                    subForm = new UpdatingFactoriesForm(ref LoadedFactories, this);
                    break;

                case 2:
                    subForm = new UpdatingForm(LoadedFactoriesEmployees, this);
                    break;
                case 3:
                    subForm = new DocumentsFilter(loadedDocuments, this);
                    break;
                default:
                    break;
            }

            subForm.TopLevel = false;
            subForm.AutoScroll = true;
            this.panel3.Size = subForm.Size;
            subForm.FormBorderStyle = FormBorderStyle.None;
            this.panel3.Controls.Add(subForm);
            subForm.Show();
        }

        private string findUsersJobs(string name)
        {
            for (int i = 0; i < loadedUsers.Count; i++)
            {
                if (loadedUsers[i].name + " " + loadedUsers[i].surname + " " + loadedUsers[i].patronymic
                    == name)
                    return loadedUsers[i].job;
            }
            return "";
        }

        private void getUsers()
        {
            List<User> users = client.GetUsers();
            LoadUsers(users);
            loadedUsers = users;
            currentFunc = this.getUsers;
        }

        private void getFactories()
        {
            List<FactoryInfo> factories = client.GetFactories();
            LoadFactories(factories);
            LoadedFactories = factories;
            currentFunc = this.getFactories;
        }

        private void getFactoriesEmployees()
        {
            List<EmployeesFactoryInfo> employeesFactories = client.GetEmployeesFactories();
            LoadFactoriesEmployees(employeesFactories);
            LoadedFactoriesEmployees = employeesFactories;
            currentFunc = this.getFactoriesEmployees;
        }

        private void getDocuments()
        {
            List<BussinesTripInfo> documents = client.GetDocuments();
            LoadDocuments(documents);
            loadedDocuments = documents;
            currentFunc = this.getDocuments;
        }

        public void LoadDocuments(List<BussinesTripInfo> documents)
        {
            button2.Visible = true;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
            dataGridView1.ColumnCount = 7;

            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "ФИО";
            dataGridView1.Columns[2].HeaderText = "Дата прибытия";
            dataGridView1.Columns[3].HeaderText = "Дата отправления";
            dataGridView1.Columns[4].HeaderText = "Место отправления";
            dataGridView1.Columns[5].HeaderText = "Причина";
            dataGridView1.Columns[6].HeaderText = "Отправитель";

            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.HeaderText = "Скачать документ";
            checkColumn.ReadOnly = false;
            checkColumn.FillWeight = 10; //if the datagridview is resized (on form resize) the checkbox won't take up too much; value is relative to the other columns' fill values
            dataGridView1.Columns.Add(checkColumn);


            if (documents.Count == 0)
                return;

            dataGridView1.RowCount = documents.Count;

            for (int i = 0; i < documents.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = documents[i].documentID;
                dataGridView1.Rows[i].Cells[1].Value = documents[i].name + " " + documents[i].surname + " " + documents[i].patronymic;
                dataGridView1.Rows[i].Cells[2].Value = documents[i].from.ToString("dd-MM-yyyy");
                dataGridView1.Rows[i].Cells[3].Value = documents[i].to.ToString("dd-MM-yyyy");
                dataGridView1.Rows[i].Cells[4].Value = documents[i].destinationPlace;
                dataGridView1.Rows[i].Cells[5].Value = documents[i].reason;
                dataGridView1.Rows[i].Cells[6].Value = documents[i].fullNameOfSender;
            }


        }

        public void AddFactory(FactoryInfo info)
        {
            client.AddFactory(user, info);
        }

        public void DeleteFactory(FactoryInfo info)
        {
            client.DeleteFactory(user, info);
        }

        public void DeleteSotrFactory(User user, FactoryInfo info)
        {
            client.DeleteUserFactory(user, info);            
        }

        public void DeleteUser(User user)
        {
            client.DeleteUser(user);
        }


        public void AddUser(User user, string password)
        {
            client.AddUser(user, password);
        }


        public void AddFactoryUser(FactoryInfo info, User user)
        {
            client.AddFactoryUser(user, info);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
             
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!CheckUserInput())
                return;
                
            string[] fullname = comboBox1.Text.Split(' ');

            BussinesTripInfo info = new BussinesTripInfo(fullname[0], fullname[1], fullname[2], textBox1.Text,
                currentDestinationPlace.name + " " + currentDestinationPlace.address, richTextBox1.Text, textBox2.Text, dateTimePicker1.Value, dateTimePicker2.Value, 0);

            var file = client.CreateRequest(info);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Word document|*.docx";
            saveFileDialog.Title = "Сохранить командировочное удостоверение";
            
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(saveFileDialog.FileName, file.ToArray());
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = findUsersJobs(comboBox1.SelectedItem.ToString());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void LoadUsers(List<User> users)
        {
            button2.Visible = false;
            comboBox1.Items.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
            dataGridView1.ColumnCount = 10;
            dataGridView1.RowCount = users.Count;
            dataGridView1.Columns[0].HeaderText = "ID сотрудника";
            dataGridView1.Columns[1].HeaderText = "Имя";
            dataGridView1.Columns[2].HeaderText = "Фамилия";
            dataGridView1.Columns[3].HeaderText = "Отчество";
            dataGridView1.Columns[4].HeaderText = "Серия паспорта";
            dataGridView1.Columns[5].HeaderText = "Номер паспорта";
            dataGridView1.Columns[6].HeaderText = "Пол";
            dataGridView1.Columns[7].HeaderText = "Дата рождения";
            dataGridView1.Columns[8].HeaderText = "Тип пользователя";
            dataGridView1.Columns[9].HeaderText = "Должность";

            for (int i = 0; i < users.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = users[i].userID;
                dataGridView1.Rows[i].Cells[1].Value = users[i].name;
                dataGridView1.Rows[i].Cells[2].Value = users[i].surname;
                dataGridView1.Rows[i].Cells[3].Value = users[i].patronymic;
                dataGridView1.Rows[i].Cells[4].Value = users[i].passportSeries;
                dataGridView1.Rows[i].Cells[5].Value = users[i].passportNumber;
                dataGridView1.Rows[i].Cells[6].Value = users[i].gender;
                dataGridView1.Rows[i].Cells[7].Value = users[i].birthday.ToString("yyyy-mm-dd");
                dataGridView1.Rows[i].Cells[8].Value = (users[i].userType == 0 ? "Пользователь" : "Администратор");
                dataGridView1.Rows[i].Cells[9].Value = users[i].job;

                string fullname = users[i].name + " " + users[i].surname + " " + users[i].patronymic;
                comboBox1.Items.Add(fullname);
            }
        }

        private void LoadFactories(List<FactoryInfo> factories)
        {
            button2.Visible = false;
            comboBox3.Items.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
            dataGridView1.ColumnCount = 3;
            dataGridView1.RowCount = factories.Count;
            dataGridView1.Columns[0].HeaderText = "ID предприятия";
            dataGridView1.Columns[1].HeaderText = "Название предприятия";
            dataGridView1.Columns[2].HeaderText = "Адрес";

            for (int i = 0; i < factories.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = factories[i].predprID;
                dataGridView1.Rows[i].Cells[1].Value = factories[i].name;
                dataGridView1.Rows[i].Cells[2].Value = factories[i].address;

                comboBox3.Items.Add(factories[i].name);
            }

        }

        private void LoadFactoriesEmployees(List<EmployeesFactoryInfo> employeesFactoryInfos)
        {
            button2.Visible = false;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
            dataGridView1.ColumnCount = 2;
            dataGridView1.RowCount = employeesFactoryInfos.Count;
            dataGridView1.Columns[0].HeaderText = "ID сотрудника";
            dataGridView1.Columns[1].HeaderText = "ID предприятия";

            for (int i = 0; i < employeesFactoryInfos.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = employeesFactoryInfos[i].UserID;
                dataGridView1.Rows[i].Cells[1].Value = employeesFactoryInfos[i].FactoryID;
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSubPanel(comboBox5.SelectedIndex);

            switch(comboBox5.SelectedIndex)
            {
                case 0:
                    getUsers();
                    break;
                case 1:
                    getFactories();
                    break;
                case 2:
                    getFactoriesEmployees();
                    break;
                case 3:
                    getDocuments();
                    break;
                default:
                    break;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentDestinationPlace = LoadedFactories[comboBox3.SelectedIndex];
        }

        private void fake()
        {
            dataGridView4.ColumnCount = 3;
            dataGridView4.Columns[0].HeaderText = "ID";
            dataGridView4.Columns[1].HeaderText = "Имя таблицы";
            dataGridView4.Columns[2].HeaderText = "Тип";

            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.Name = "X";
            checkColumn.HeaderText = "Подтвердить";
            checkColumn.Width = 50;
            checkColumn.ReadOnly = false;
            checkColumn.FillWeight = 10; //if the datagridview is resized (on form resize) the checkbox won't take up too much; value is relative to the other columns' fill values
            dataGridView4.Columns.Add(checkColumn);

            dataGridView4.RowCount = 1;
            
            dataGridView4[0, 0].Value = "1";
            dataGridView4[1, 0].Value = "Предприятия";
            dataGridView4[2, 0].Value = "Добавить";

            dataGridView3.ColumnCount = 2;
            dataGridView3.Columns[0].HeaderText = "ID";
            dataGridView3.Columns[1].HeaderText = "Имя таблицы";
            dataGridView3.RowCount = 3;
            dataGridView3[0, 0].Value = "ID";
            dataGridView3[1, 0].Value = "4";
            dataGridView3[0, 1].Value = "Название";
            dataGridView3[1, 1].Value = "Брестский завод легкопромышленности";
            dataGridView3[0, 2].Value = "Адрес";
            dataGridView3[1, 2].Value = "Г. Брест, ул. Фонарёва 40";

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            currentFunc();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1[7,i].Value != null && (bool)dataGridView1[7, i].Value)
                {
                    var file = client.DownloadDocument(Convert.ToInt32(dataGridView1[0, i].Value));
                    
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Word document|*.docx";
                    saveFileDialog.Title = "Сохранить командировочное удостоверение";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(saveFileDialog.FileName, file.ToArray());
                    }

                }
            }
        }
    }
   
}
