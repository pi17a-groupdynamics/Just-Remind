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
    public partial class Form1 : Form
    {
        private NotifyIcon notifyIcon = new NotifyIcon();

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            tabControl1.SelectedIndex = 1;
            label2.Visible = false;
            label3.Visible = false;
            //тут мы определяем разрешение экрана и выставляем начальные
            //координаты окна уведомления, чтобы оно отображалось в
            //нижнем правом углу экрана
            Size resolution = Screen.PrimaryScreen.Bounds.Size;
            Notification_window.xStartCoord = resolution.Width - 431;
            Notification_window.yStartCoord = resolution.Height - 207;
            //Настраиваем обычное уведомление
            notifyIcon.BalloonTipText = "Напоминание";
            notifyIcon.BalloonTipTitle = "Очень важное";
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon.Icon = this.Icon;
            //Заполняем таблицы
            dataGridView2.Rows.Add("Купить майонез");
            dataGridView2.Rows.Add("Позвонить Илону Маску");
            dataGridView3.Rows.Add("Надя - 26.02.19");
            dataGridView3.Rows.Add("Катя - 03.03.19");
            dataGridView4.Rows.Add("8 марта - 08.03.19");
            dataGridView4.Rows.Add("Пасха - 28.04.19");
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //тут программа выбирает, что показывать после "Ваши задачи"
            //в зависимости от того, какая вкладка сейчас открыта
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    comboBox1.Visible = false;
                    label2.Visible = false;
                    label3.Visible = false;
                    break;
                case 1:
                    comboBox1.Visible = true;
                    label2.Visible = false;
                    label3.Visible = false;
                    break;
                case 2:
                    comboBox1.Visible = false;
                    label2.Visible = true;
                    label3.Visible = false;
                    break;
                case 3:
                    comboBox1.Visible = false;
                    label2.Visible = false;
                    label3.Visible = true;
                    break;
            }
        }

        private void показатьВсплывающееОкноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Так вызывается событие показа уведомления ввиде окна
            Notification_window nWindow = new Notification_window("Некий текст");
            nWindow.Show();
        }

        private void показатьПростоеУведомлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Так вызывается событие показа стандартного уведомления Windows
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(5000);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddNotificationForm addNotificationForm = new AddNotificationForm();
            addNotificationForm.Show();
        }
    }
}
