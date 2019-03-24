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
    public partial class Notification_window : Form
    {
        //поля статические, чтобы не пересчитывать их каждый раз
        public static int xStartCoord;
        public static int yStartCoord;

        //в конструктор обязательно передавать текст по понятным причинам
        public Notification_window(string text)
        {
            InitializeComponent();
            Location = new Point(xStartCoord, yStartCoord);
            label1.Text = text;
        }
    }
}
