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
        private NotificationList personalNotifications = new NotificationList();
        private NotificationList birthdayNotifications = new NotificationList();
        private NotificationList holidayNotifications = new NotificationList();
        private AddNotificationForm addNotificationForm = new AddNotificationForm();

        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            tabControl1.SelectedIndex = 1;
            label2.Visible = false;
            label3.Visible = false;
            // Тут мы определяем разрешение экрана и выставляем начальные
            // координаты окна уведомления, чтобы оно отображалось в
            // нижнем правом углу экрана
            Size resolution = Screen.PrimaryScreen.Bounds.Size;
            NotificationWindow.xStartCoord = resolution.Width - 431;
            NotificationWindow.yStartCoord = resolution.Height - 207;
            // Тестовое уведомление и значения в таблице. Потом мы это уберём
            SetTestOptions();
        }

        // Тестовое уведомление и значения в таблице
        private void SetTestOptions()
        {
            // Настраиваем системное уведомление (тестовое)
            notifyIcon.BalloonTipText = "Напоминание";
            notifyIcon.BalloonTipTitle = "Очень важное";
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon.Icon = this.Icon;
            // Заполняем таблицы 
            personalNotifications.Add(new Notification("Позвонить Илону Маску",
                new DateTime(2019, 07, 15, 23, 00, 00)));
            personalNotifications.Add(new Notification("Купить майонез",
                new DateTime(2019, 08, 16, 09, 00, 00)));
            dataGridView2.Rows.Add("Позвонить Илону Маску");
            dataGridView2.Rows.Add("Купить майонез");
            dataGridView3.Rows.Add("Надя - 26.02.19");
            dataGridView3.Rows.Add("Катя - 03.03.19");
            dataGridView4.Rows.Add("8 марта - 08.03.19");
            dataGridView4.Rows.Add("Пасха - 28.04.19");
        }

        // Тут программа выбирает, что показывать после "Ваши задачи"
        // в зависимости от того, какая вкладка сейчас открыта
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        // Нажатие Отладка -> Показать всплывающее окно
        private void ShowPopupWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Так вызывается событие показа уведомления ввиде окна
            NotificationWindow nWindow = new NotificationWindow("Некий текст");
            nWindow.Show();
        }

        // Нажатие Отладка -> Показать простое уведомление
        private void ShowSimpleNotifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Так вызывается событие показа стандартного уведомления Windows
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(5000);
        }

        // Нажатие кнопки "+"
        private void Button1_Click(object sender, EventArgs e)
        {
            Notification notification = new Notification();
            addNotificationForm.Notification = notification;
            addNotificationForm.ShowDialog();
            if (addNotificationForm.DialogResult == DialogResult.OK)
            {
                personalNotifications.Insert(notification);
                UpdatePersonalNotifTable();
            }
        }

        // Обновить таблицу на вкладке "Личные"
        private void UpdatePersonalNotifTable()
        {
            dataGridView2.Rows.Clear();
            foreach (Notification notification in personalNotifications)
                dataGridView2.Rows.Add(notification.Text);
        }
    }
}
