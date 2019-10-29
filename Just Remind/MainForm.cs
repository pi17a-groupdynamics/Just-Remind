using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        // Конструктор формы
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

        // Проверяет, существует ли директория пользователя и файлы в ней в 
        // AppData/Local. Если не существует - создаёт её и файлы. Если
        // нет какого-то файла - создаёт его
        private void CheckUserDirectory()
        {
            string appDataPath = 
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string applicationPath = appDataPath + "\\Just Remind";
            if (Directory.Exists(applicationPath))
            {
                string personalFilePath = applicationPath + "\\Personal.dat";
                if (!File.Exists(personalFilePath))
                    File.Create(personalFilePath);
                string birthdaysFilePath = applicationPath + "\\Birthdays.dat";
                if (!File.Exists(birthdaysFilePath))
                    File.Create(birthdaysFilePath);
                string holidaysFilePath = applicationPath + "\\Holidays.dat";
                if (!File.Exists(holidaysFilePath))
                    File.Create(holidaysFilePath);
            }
            else
            {
                Directory.CreateDirectory(applicationPath);
                File.Create(applicationPath + "\\Personal.dat");
                File.Create(applicationPath + "\\Birthdays.dat");
                File.Create(applicationPath + "\\Holidays.dat");
            }
        }

        // Вызывается при загрузке формы, после конструктора
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckUserDirectory();
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

        // Создаёт директорию Just Remind в AppData, если она была удалена
        // и файл в этой директории, имя которого передаётся в fileName.
        // Возвращает путь к файлу
        private string CreateDirAndFile(string fileName)
        {
            string appDataPath =
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string applicationPath = appDataPath + "\\Just Remind";
            string filePath = applicationPath + "\\" + fileName;
            if (!Directory.Exists(applicationPath))
            {
                Directory.CreateDirectory(applicationPath);
                File.Create(filePath);
            }
            return filePath;
        }

        // Обновляет таблицу dataGridView, используя данные из notificationList
        private void UpdateNotifTable(DataGridView dataGridView, 
            NotificationList notificationList)
        {
            dataGridView.Rows.Clear();
            foreach (Notification notification in notificationList)
            {
                string notificationText = notification.Text;
                int indexOfNewLine = notificationText.IndexOf('\n');
                if (indexOfNewLine != -1)
                {
                    string firstRow = notificationText.Substring(0, indexOfNewLine) + "...";
                    dataGridView.Rows.Add(firstRow);
                }
                else
                    dataGridView.Rows.Add(notificationText);
            }
        }

        // Обновляет таблицу с важными уведомлениями
        private void UpdateImportantNotifTable()
        {
            //дописать
        }

        // Перезаписывает файл "Personal.dat"
        private void RewritePersonalFile()
        {
            try
            {
                string filePath = CreateDirAndFile("Personal.dat");
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (Notification notification in personalNotifications)
                    {
                        writer.WriteLine();
                        writer.WriteLine(notification.RowsNum);
                        DateTime nearestDateTime = notification.NearestDateTime;
                        writer.WriteLine(nearestDateTime.Day);
                        writer.WriteLine(nearestDateTime.Month);
                        writer.WriteLine(nearestDateTime.Year);
                        writer.WriteLine(nearestDateTime.Hour);
                        writer.WriteLine(nearestDateTime.Minute);
                        if (notification.IsRepeatByDate)
                        {
                            writer.WriteLine(1);
                            DateTime repeatDate = notification.RepeatDate;
                            writer.WriteLine(repeatDate.Day);
                            writer.WriteLine(repeatDate.Month);
                            writer.WriteLine(repeatDate.Year);
                            writer.WriteLine(repeatDate.Hour);
                            writer.WriteLine(repeatDate.Minute);
                        }
                        else
                        {
                            writer.WriteLine(0);
                            for (int i = 0; i < 5; i++)
                                writer.WriteLine(0);
                        }
                        if (notification.IsRepeatByDaysOfWeek)
                        {
                            writer.WriteLine(1);
                            if (notification.IsRepeatOnMonday)
                                writer.WriteLine(1);
                            else
                                writer.WriteLine(0);
                            if (notification.IsRepeatOnTuesday)
                                writer.WriteLine(1);
                            else
                                writer.WriteLine(0);
                            if (notification.IsRepeatOnWednesday)
                                writer.WriteLine(1);
                            else
                                writer.WriteLine(0);
                            if (notification.IsRepeatOnThursday)
                                writer.WriteLine(1);
                            else
                                writer.WriteLine(0);
                            if (notification.IsRepeatOnFriday)
                                writer.WriteLine(1);
                            else
                                writer.WriteLine(0);
                            if (notification.IsRepeatOnSaturday)
                                writer.WriteLine(1);
                            else
                                writer.WriteLine(0);
                            if (notification.IsRepeatOnSunday)
                                writer.WriteLine(1);
                            else
                                writer.WriteLine(0);
                        }
                        else
                        {
                            writer.WriteLine(0);
                            for (int i = 0; i < 7; i++)
                                writer.WriteLine(0);
                        }
                        if (notification.IsRepeatByDay)
                        {
                            writer.WriteLine(1);
                            writer.WriteLine(0);
                            writer.WriteLine(0);
                            DateTime startTime = notification.StartTime;
                            writer.WriteLine(startTime.Hour);
                            writer.WriteLine(startTime.Minute);
                            DateTime endTime = notification.EndTime;
                            writer.WriteLine(endTime.Hour);
                            writer.WriteLine(endTime.Minute);
                            writer.WriteLine(notification.HoursInterval);
                            writer.WriteLine(notification.MinutesInterval);
                        }
                        else
                        {
                            writer.WriteLine(0);
                            writer.WriteLine(notification.Hour);
                            writer.WriteLine(notification.Minute);
                            for (int i = 0; i < 6; i++)
                                writer.WriteLine(0);
                        }
                        if (notification.IsImportant)
                            writer.WriteLine(1);
                        else
                            writer.WriteLine(0);
                        writer.Write(notification.Text);
                        writer.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при записи файла Personal.dat");
            }
        }

        // Перезаписывает файл "Birthdays.dat" или "Holidays.dat" в зависимости
        // от того, какие notificationList и fileName были переданы в качестве аргументов
        private void RewriteBirthdaysOrHolidaysFile(NotificationList notificationList,
            string fileName)
        {
            try
            {
                string filePath = CreateDirAndFile(fileName);
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (Notification notification in notificationList)
                    {
                        writer.WriteLine();
                        writer.WriteLine(notification.RowsNum);
                        DateTime nearestDateTime = notification.NearestDateTime;
                        writer.WriteLine(nearestDateTime.Day);
                        writer.WriteLine(nearestDateTime.Month);
                        writer.WriteLine(nearestDateTime.Year);
                        writer.WriteLine(nearestDateTime.Hour);
                        writer.WriteLine(nearestDateTime.Minute);
                        DateTime repeatDate = notification.RepeatDate;
                        writer.WriteLine(repeatDate.Day);
                        writer.WriteLine(repeatDate.Month);
                        writer.WriteLine(repeatDate.Year);
                        writer.WriteLine(repeatDate.Hour);
                        writer.WriteLine(repeatDate.Minute);
                        if (notification.IsRepeatByDay)
                        {
                            writer.WriteLine(1);
                            writer.WriteLine(0);
                            writer.WriteLine(0);
                            DateTime startTime = notification.StartTime;
                            writer.WriteLine(startTime.Hour);
                            writer.WriteLine(startTime.Minute);
                            DateTime endTime = notification.EndTime;
                            writer.WriteLine(endTime.Hour);
                            writer.WriteLine(endTime.Minute);
                            writer.WriteLine(notification.HoursInterval);
                            writer.WriteLine(notification.MinutesInterval);
                        }
                        else
                        {
                            writer.WriteLine(0);
                            writer.WriteLine(notification.Hour);
                            writer.WriteLine(notification.Minute);
                            for (int i = 0; i < 6; i++)
                                writer.WriteLine(0);
                        }
                        if (notification.IsImportant)
                            writer.WriteLine(1);
                        else
                            writer.WriteLine(0);
                        writer.Write(notification.Text);
                        writer.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при записи файла " + fileName);
            }
        }

        // Нажатие кнопки "+"
        private void Button1_Click(object sender, EventArgs e)
        {
            Notification notification = new Notification();
            addNotificationForm.Notification = notification;
            addNotificationForm.ShowDialog();
            if (addNotificationForm.DialogResult == DialogResult.OK)
            {
                switch (notification.Category)
                {
                    case NotifCategories.Personal:
                        personalNotifications.Insert(notification);
                        RewritePersonalFile();
                        UpdateNotifTable(dataGridView2, personalNotifications);
                        break;
                    case NotifCategories.Birthdays:
                        birthdayNotifications.Insert(notification);
                        RewriteBirthdaysOrHolidaysFile(birthdayNotifications, "Birthdays.dat");
                        UpdateNotifTable(dataGridView3, birthdayNotifications);
                        break;
                    case NotifCategories.Holidays:
                        holidayNotifications.Insert(notification);
                        RewriteBirthdaysOrHolidaysFile(holidayNotifications, "Holidays.dat");
                        UpdateNotifTable(dataGridView4, holidayNotifications);
                        break;
                }
                UpdateImportantNotifTable();
            }
        }
    }
}
