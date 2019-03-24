using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Just_Remind
{
    public partial class AddNotificationForm : Form
    {
        public AddNotificationForm()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            if (radioButton1.Checked)
                panel3.Visible = true;
            else
                panel2.Visible = true;
        }

        private void button26_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel4.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel4.Visible = true;
        }


        private void button15_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel1.Visible = true;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = true;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            if (radioButton1.Checked)
                panel3.Visible = true;
            else
                panel2.Visible = true;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
