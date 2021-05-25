using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class Form2 : Form
    {
        private ClientConnection client;
        public ClientConnection.UserType user;

        public Form2(ref ClientConnection client)
        {
            InitializeComponent();
            this.client = client;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.user = client.Login(textBox1.Text);

            if (user == ClientConnection.UserType.Error)
            {
                MessageBox.Show("Неверный идентификатор пользователя!");
                textBox1.Text = "";
            }
            else
            {
                this.Close();
            }
        }
    }
}
