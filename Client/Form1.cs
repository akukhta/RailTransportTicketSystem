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
        private void test()
        {
            comboBox1.Items.Add("afewd");
            comboBox1.Items.Add("adsrffad");
            comboBox1.Items.Add("adsrffad");
            comboBox2.Items.Add("adsrffad");
            comboBox2.Items.Add("adsrffad");
            comboBox2.Items.Add("adsrffad");
        }

        public Form1()
        {
            InitializeComponent();
            test();
        }

        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
             
        }
    }
   
    }
