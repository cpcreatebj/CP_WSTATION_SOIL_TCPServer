using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CP_WSMonitor
{
    public partial class Form_data_jishi : Form
    {
        public Form_data_jishi()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form_data_jishi form1 = new Form_data_jishi();
            form1.Close();
        }
    }
}
