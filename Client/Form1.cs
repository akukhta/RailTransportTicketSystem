﻿using System;
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
    public partial class Form1 : Form
    {
        private ClientConnection client;
        private User user;

        private void test()
        {
            ;
        }

        public Form1(ref ClientConnection client, ref User user)
        {
            InitializeComponent();
            test();
            this.client = client;
            this.user = user;
            

            if (this.user.userType == 1)
            {
                string fullName = user.job + " " + user.name + " " + user.surname + " " + user.patronymic;
                comboBox4.Text = fullName;
                comboBox4.Items.Add(fullName);
            }

            else
            {
                string fullName = user.name + " " + user.surname + " " + user.patronymic;
                comboBox1.Text = fullName;
                comboBox1.Items.Add(fullName);
                comboBox2.Text = user.job;
                comboBox2.Items.Add(user.job);
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



            return true;
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

            BussinesTripInfo info = new BussinesTripInfo(fullname[0], fullname[1], fullname[2], comboBox2.Text,
                comboBox3.Text, richTextBox1.Text, comboBox4.Text, dateTimePicker1.Value, dateTimePicker2.Value);

            client.CreateRequest(info);
        }
    }
   
    }
