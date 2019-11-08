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
        private NotificationList importantNotifications = new NotificationList();
        private AddNotificationForm addNotificationForm = new AddNotificationForm();
        private ShowNotificationChecker showNotificationChecker;

        /// <summary>
        /// Зарезервированная дата и время, которую возвращает NearestDateTimeCounter
        /// в том случае, если напоминание больше не нужно повторять
        /// </summary>
        private readonly DateTime DELETE_NOTIFICATION = new DateTime(2200, 1, 1);

        /// <summary>
        /// Напоминание, которое будет показано раньше всех остальных
        /// </summary>
        public Notification NearestNotification { get; set; }

        /// <summary>
        /// Определяет ближайшее напоминание, если хотя бы один из списков 
        /// с напоминаниями является путым
        /// </summary>
        private void DetermineNearestNotif_EmptyLists()
        {
            int personalNotifCount = personalNotifications.Count;
            int birthdayNotifCount = birthdayNotifications.Count;
            int holidayNotifCount = holidayNotifications.Count;
            if (personalNotifCount == 0 && birthdayNotifCount == 0 &&
                holidayNotifCount == 0)
            {
                NearestNotification = new Notification();
                NearestNotification.NearestDateTime = new DateTime(2200, 1, 1);
            }
            else if (personalNotifCount > 0)
            {
                if (birthdayNotifCount > 0)
                {
                    Notification nearestPersonalNotif = personalNotifications[0];
                    Notification nearestBirthdayNotif = birthdayNotifications[0];
                    if (nearestPersonalNotif.NearestDateTime.CompareTo(nearestBirthdayNotif.NearestDateTime) <= 0)
                        NearestNotification = nearestPersonalNotif;
                    else
                        NearestNotification = nearestBirthdayNotif;
                }
                else if (holidayNotifCount > 0)
                {
                    Notification nearestPersonalNotif = personalNotifications[0];
                    Notification nearestHolidayNotif = birthdayNotifications[0];
                    if (nearestPersonalNotif.NearestDateTime.CompareTo(nearestHolidayNotif.NearestDateTime) <= 0)
                        NearestNotification = nearestPersonalNotif;
                    else
                        NearestNotification = nearestHolidayNotif;
                }
                else
                    NearestNotification = personalNotifications[0];
            }
            else if (birthdayNotifCount > 0)
            {
                if (holidayNotifCount > 0)
                {
                    Notification nearestBirthdayNotif = personalNotifications[0];
                    Notification nearestHolidayNotif = birthdayNotifications[0];
                    if (nearestBirthdayNotif.NearestDateTime.CompareTo(nearestHolidayNotif.NearestDateTime) <= 0)
                        NearestNotification = nearestBirthdayNotif;
                    else
                        NearestNotification = nearestHolidayNotif;
                }
                else
                    NearestNotification = birthdayNotifications[0];
            }
            else
                NearestNotification = holidayNotifications[0];
        }

        /// <summary>
        /// Сравнивает ближайшие напоминания во всех списках, выбирает из них
        /// то, которое должно быть показано раньше всего, и сохраняет его в
        /// свойство NearestNotification
        /// </summary>
        private void DetermineNearestNotif()
        {
            Notification nearestNotif;
            if (personalNotifications.Count > 0 && birthdayNotifications.Count > 0 &&
                holidayNotifications.Count > 0)
            {
                Notification nearestPersonalNotif = personalNotifications[0];
                Notification nearestBirthdayNotif = birthdayNotifications[0];
                if (nearestPersonalNotif.NearestDateTime.CompareTo(nearestBirthdayNotif.NearestDateTime) <= 0)
                    nearestNotif = nearestPersonalNotif;
                else
                    nearestNotif = nearestBirthdayNotif;
                Notification nearestHolidayNotif = holidayNotifications[0];
                if (nearestNotif.NearestDateTime.CompareTo(nearestBirthdayNotif.NearestDateTime) <= 0)
                    NearestNotification = nearestNotif;
                else
                    NearestNotification = nearestHolidayNotif;
            }
            else
                DetermineNearestNotif_EmptyLists();
        }

        // Загрузка формы
        #region Load

        /// <summary>
        /// Тестовое уведомление и значения в таблице
        /// </summary>
        private void SetTestOptions()
        {
            // Настраиваем системное уведомление (тестовое)
            notifyIcon.BalloonTipText = "Напоминание";
            notifyIcon.BalloonTipTitle = "Очень важное";
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon.Icon = this.Icon;
        }
            
        /// <summary>
        /// Конструктор формы
        /// </summary>
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
            NotificationWindow.XStartCoord = resolution.Width - 431;
            NotificationWindow.YStartCoord = resolution.Height - 247;
            // Тестовое уведомление и значения в таблице. Потом мы это уберём
            SetTestOptions();
        }

        /// <summary>
        /// Проверяет, существует ли директория пользователя и файлы в ней в 
        /// AppData/Local. Если не существует - создаёт её и файлы. Если
        /// нет какого-то файла - создаёт его
        /// </summary>
        private string CheckUserDirectory()
        {
            string appDataPath = 
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string applicationPath = appDataPath + "\\Just Remind";
            if (Directory.Exists(applicationPath))
            {
                string personalFilePath = applicationPath + "\\Personal.dat";
                if (!File.Exists(personalFilePath))
                    File.Create(personalFilePath).Close();
                string birthdaysFilePath = applicationPath + "\\Birthdays.dat";
                if (!File.Exists(birthdaysFilePath))
                    File.Create(birthdaysFilePath).Close();
                string holidaysFilePath = applicationPath + "\\Holidays.dat";
                if (!File.Exists(holidaysFilePath))
                    File.Create(holidaysFilePath).Close();
            }
            else
            {
                Directory.CreateDirectory(applicationPath);
                File.Create(applicationPath + "\\Personal.dat").Close();
                File.Create(applicationPath + "\\Birthdays.dat").Close();
                File.Create(applicationPath + "\\Holidays.dat").Close();
            }
            return applicationPath;
        }

        /// <summary>
        /// Тут думаю всё понятно
        /// </summary>
        private bool StrToBool(string str)
        {
            if (str == "0")
                return false;
            else
                return true;
        }

        /// <summary>
        /// Загружает данные из "Personal.dat"
        /// </summary>
        private void LoadDataFromPersonal(string appPath)
        {
            try
            {
                string filePath = appPath + "\\Personal.dat";
                long fileLength = new FileInfo(filePath).Length;
                if (fileLength == 0)
                    return;
                string fileText;
                using (StreamReader reader = new StreamReader(filePath))
                {
                    fileText = reader.ReadToEnd();
                }
                using (StringReader reader = new StringReader(fileText))
                {
                    string line;
                    reader.ReadLine();
                    while ((line = reader.ReadLine()) != null)
                    {
                        Notification notification = new Notification();
                        short rowsNum = short.Parse(line);
                        notification.RowsNum = rowsNum;
                        short day = short.Parse(reader.ReadLine());
                        short month = short.Parse(reader.ReadLine());
                        short year = short.Parse(reader.ReadLine());
                        short hour = short.Parse(reader.ReadLine());
                        short minute = short.Parse(reader.ReadLine());
                        DateTime dateTime = new DateTime(year, month, day);
                        dateTime = dateTime.AddHours(hour);
                        dateTime = dateTime.AddMinutes(minute);
                        notification.NearestDateTime = dateTime;
                        bool flag = StrToBool(reader.ReadLine());
                        notification.IsRepeatByDate = flag;
                        if (flag == true)
                        {
                            day = short.Parse(reader.ReadLine());
                            month = short.Parse(reader.ReadLine());
                            notification.RepeatDate = new DateTime(year, month, day);
                        }
                        else
                        {
                            reader.ReadLine();
                            reader.ReadLine();
                        }
                        flag = StrToBool(reader.ReadLine());
                        notification.IsRepeatByDaysOfWeek = flag;
                        if (flag == true)
                        {
                            notification.IsRepeatOnMonday = StrToBool(reader.ReadLine());
                            notification.IsRepeatOnTuesday = StrToBool(reader.ReadLine());
                            notification.IsRepeatOnWednesday = StrToBool(reader.ReadLine());
                            notification.IsRepeatOnThursday = StrToBool(reader.ReadLine());
                            notification.IsRepeatOnFriday = StrToBool(reader.ReadLine());
                            notification.IsRepeatOnSaturday = StrToBool(reader.ReadLine());
                            notification.IsRepeatOnSunday = StrToBool(reader.ReadLine());
                        }
                        else
                        {
                            for (int i = 0; i < 7; i++)
                                reader.ReadLine();
                        }
                        flag = StrToBool(reader.ReadLine());
                        notification.IsRepeatByDay = flag;
                        if (flag == true)
                        {
                            reader.ReadLine();
                            reader.ReadLine();
                            DateTime startTime = new DateTime();
                            startTime = startTime.AddHours(short.Parse(reader.ReadLine()));
                            startTime = startTime.AddMinutes(short.Parse(reader.ReadLine()));
                            notification.StartTime = startTime;
                            DateTime endTime = new DateTime();
                            endTime = endTime.AddHours(short.Parse(reader.ReadLine()));
                            endTime = endTime.AddMinutes(short.Parse(reader.ReadLine()));
                            notification.EndTime = endTime;
                            notification.HoursInterval = short.Parse(reader.ReadLine());
                            notification.MinutesInterval = short.Parse(reader.ReadLine());
                        }
                        else
                        {
                            notification.Hour = short.Parse(reader.ReadLine());
                            notification.Minute = short.Parse(reader.ReadLine());
                            for (int i = 0; i < 6; i++)
                                reader.ReadLine();
                        }
                        notification.IsImportant = StrToBool(reader.ReadLine());
                        string notificationText = string.Empty;
                        for (int i = 0; i < rowsNum; i++)
                            notificationText += reader.ReadLine();
                        notification.Text = notificationText;
                        notification.Category = NotifCategories.Personal;
                        personalNotifications.Insert(notification);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при чтении файла \"Personal.dat\"", "Ошибка");
                return;
            }
        }

        /// <summary>
        /// Загружает данные из "Birthdays.dat" или из "Holidays.dat"
        /// в зависимости от переданных аргументов
        /// </summary>
        private void LoadDataFromBirthdaysOrHolidays(string filePath, 
            NotificationList notificationList)
        {
            NotifCategories notifCategory;
            if (notificationList == birthdayNotifications)
                notifCategory = NotifCategories.Birthdays;
            else
                notifCategory = NotifCategories.Holidays;
            string fileText;
            using (StreamReader reader = new StreamReader(filePath))
            {
                fileText = reader.ReadToEnd();
            }
            using (StringReader reader = new StringReader(fileText))
            {
                string line;
                reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    Notification notification = new Notification();
                    short rowsNum = short.Parse(line);
                    notification.RowsNum = rowsNum;
                    short day = short.Parse(reader.ReadLine());
                    short month = short.Parse(reader.ReadLine());
                    short year = short.Parse(reader.ReadLine());
                    short hour = short.Parse(reader.ReadLine());
                    short minute = short.Parse(reader.ReadLine());
                    DateTime dateTime = new DateTime(year, month, day);
                    dateTime = dateTime.AddHours(hour);
                    dateTime = dateTime.AddMinutes(minute);
                    notification.NearestDateTime = dateTime;
                    day = short.Parse(reader.ReadLine());
                    month = short.Parse(reader.ReadLine());
                    notification.RepeatDate = new DateTime(year, month, day);
                    bool flag = StrToBool(reader.ReadLine());
                    notification.IsRepeatByDay = flag;
                    if (flag == true)
                    {
                        reader.ReadLine();
                        reader.ReadLine();
                        DateTime startTime = new DateTime();
                        startTime = startTime.AddHours(short.Parse(reader.ReadLine()));
                        startTime = startTime.AddMinutes(short.Parse(reader.ReadLine()));
                        notification.StartTime = startTime;
                        DateTime endTime = new DateTime();
                        endTime = endTime.AddHours(short.Parse(reader.ReadLine()));
                        endTime = endTime.AddMinutes(short.Parse(reader.ReadLine()));
                        notification.EndTime = endTime;
                        notification.HoursInterval = short.Parse(reader.ReadLine());
                        notification.MinutesInterval = short.Parse(reader.ReadLine());
                    }
                    else
                    {
                        notification.Hour = short.Parse(reader.ReadLine());
                        notification.Minute = short.Parse(reader.ReadLine());
                        for (int i = 0; i < 6; i++)
                            reader.ReadLine();
                    }
                    notification.IsImportant = StrToBool(reader.ReadLine());
                    string notificationText = string.Empty;
                    for (int i = 0; i < rowsNum; i++)
                        notificationText += reader.ReadLine();
                    notification.Text = notificationText;
                    notification.IsRepeatByDate = true;
                    notification.Category = notifCategory;
                    notificationList.Insert(notification);
                }
            }
        }

        /// <summary>
        /// Загружает данные из "Birthdays.dat"
        /// </summary>
        private void LoadDataFromBirthdays(string appPath)
        {
            try
            {
                string filePath = appPath + "\\Birthdays.dat";
                long fileLength = new FileInfo(filePath).Length;
                if (fileLength == 0)
                    return;
                else
                    LoadDataFromBirthdaysOrHolidays(filePath, birthdayNotifications);
            }
            catch
            {
                MessageBox.Show("Ошибка при чтении файла \"Birthdays.dat\"", "Ошибка");
                return;
            }
        }

        /// <summary>
        /// Загружает данные из "Holidays.dat"
        /// </summary>
        private void LoadDataFromHolidays(string appPath)
        {
            try
            {
                string filePath = appPath + "\\Holidays.dat";
                long fileLength = new FileInfo(filePath).Length;
                if (fileLength == 0)
                    return;
                else
                    LoadDataFromBirthdaysOrHolidays(filePath, holidayNotifications);
            }
            catch
            {
                MessageBox.Show("Ошибка при чтении файла \"Holidays.dat\"", "Ошибка");
                return;
            }
        }

        /// <summary>
        /// После загрузки напоминаний из файлов удаляет напоминания, которые
        /// уже поздно показывать
        /// </summary>
        private void DeleteOutOFDateNotifs()
        {
            for (int i = 0; i < personalNotifications.Count; i++)
            {
                Notification notification = personalNotifications[i];
                DateTime nearestDateTime = NearestDateTimeCounter.CountNearestDateTime(notification);
                personalNotifications.RemoveAt(i);
                if (nearestDateTime.Equals(DELETE_NOTIFICATION))
                    i--;
                else
                {
                    notification.NearestDateTime = nearestDateTime;
                    int insertIndex = personalNotifications.Insert(notification);
                    if (insertIndex > i)
                        i--;
                }
            }
            RewritePersonalFile();
            for (int i = 0; i < birthdayNotifications.Count; i++)
            {
                Notification notification = birthdayNotifications[i];
                DateTime nearestDateTime = NearestDateTimeCounter.CountNearestDateTime(notification);
                birthdayNotifications.RemoveAt(i);
                notification.NearestDateTime = nearestDateTime;
                int insertIndex = birthdayNotifications.Insert(notification);
                if (insertIndex > i)
                    i--;
            }
            RewriteBirthdaysOrHolidaysFile(birthdayNotifications, "Birthdays.dat");
            for (int i = 0; i < holidayNotifications.Count; i++)
            {
                Notification notification = holidayNotifications[i];
                DateTime nearestDateTime = NearestDateTimeCounter.CountNearestDateTime(notification);
                holidayNotifications.RemoveAt(i);
                notification.NearestDateTime = nearestDateTime;
                int insertIndex = holidayNotifications.Insert(notification);
                if (insertIndex > i)
                    i--;
            }
            RewriteBirthdaysOrHolidaysFile(holidayNotifications, "Holidays.dat");
        }

        /// <summary>
        /// Загружает данные из файлов программы в AppData оперативную память
        /// (списки NotificationList) 
        /// </summary>
        private void LoadDataFromFiles(string appPath)
        {
            // Проверять наличие папки Just Remind и файлов не обязательно, так как
            // этот метод вызывается при загрузке программы после этой самой проверки
            this.Cursor = Cursors.WaitCursor;
            LoadDataFromPersonal(appPath);
            LoadDataFromBirthdays(appPath);
            LoadDataFromHolidays(appPath);
            DeleteOutOFDateNotifs();
            UpdateNotifTable(dataGridView2, personalNotifications);
            UpdateNotifTable(dataGridView3, birthdayNotifications);
            UpdateNotifTable(dataGridView4, holidayNotifications);
            CreateImportantNotifsList();
            UpdateImportantNotifTable();
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Формирует список важных напоминаний
        /// </summary>
        private void CreateImportantNotifsList()
        {
            foreach (Notification notification in personalNotifications)
            {
                if (notification.IsImportant)
                    importantNotifications.Insert(notification);
            }
            foreach (Notification notification in birthdayNotifications)
            {
                if (notification.IsImportant)
                    importantNotifications.Insert(notification);
            }
            foreach (Notification notification in holidayNotifications)
            {
                if (notification.IsImportant)
                    importantNotifications.Insert(notification);
            }
        }

        /// <summary>
        /// Вызывается при загрузке формы, после конструктора
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            string appPath = CheckUserDirectory();
            LoadDataFromFiles(appPath);
            showNotificationChecker = new ShowNotificationChecker(this);
            DetermineNearestNotif();
            showNotificationChecker.StartAsync();
        }

        #endregion

        //Показ уведомлений
        #region NotificationsShow

        /// <summary>
        /// Показывает ближайшее уведомление (NearestNotification)
        /// </summary>
        public void ShowNearestNotification()
        {
            NotificationWindow notificationWindow = new NotificationWindow(this, NearestNotification);
            showNotificationChecker.Stop();
            notificationWindow.Show();
            Notification tempNotification;
            switch (NearestNotification.Category)
            {
                case NotifCategories.Personal:
                    // Сохраняем первый (ближайший) элемент списка во временный объект
                    tempNotification = personalNotifications[0];
                    // Считаем для него новую ближайшую дату показа
                    tempNotification.NearestDateTime = NearestDateTimeCounter.CountNearestDateTime(tempNotification);
                    // Удаляем первый элемент списка, так как он только что был показан и больше не нужен
                    personalNotifications.RemoveAt(0);
                    // Если это напоминание ещё нужно будет повторять в будущем, то вставляем его 
                    // (с уже пересчитанной датой/временем показа) в список
                    if (tempNotification.NearestDateTime.CompareTo(DELETE_NOTIFICATION) != 0)
                        personalNotifications.Insert(tempNotification);
                    // Перезаписываем файл и обновляем таблицу, основываясь на уже обновлённом списке
                    RewritePersonalFile();
                    UpdateNotifTable(dataGridView2, personalNotifications);
                    break;
                    // В остальных кейсах делается то же самое, только с другими списками
                case NotifCategories.Birthdays:
                    tempNotification = birthdayNotifications[0];
                    tempNotification.NearestDateTime = NearestDateTimeCounter.CountNearestDateTime(tempNotification);
                    birthdayNotifications.RemoveAt(0);
                    if (tempNotification.NearestDateTime.CompareTo(DELETE_NOTIFICATION) != 0)
                        birthdayNotifications.Insert(tempNotification);
                    RewriteBirthdaysOrHolidaysFile(birthdayNotifications, "Birthdays.dat");
                    UpdateNotifTable(dataGridView3, birthdayNotifications);
                    break;
                case NotifCategories.Holidays:
                    tempNotification = holidayNotifications[0];
                    tempNotification.NearestDateTime = NearestDateTimeCounter.CountNearestDateTime(tempNotification);
                    holidayNotifications.RemoveAt(0);
                    if (tempNotification.NearestDateTime.CompareTo(DELETE_NOTIFICATION) != 0)
                        holidayNotifications.Insert(tempNotification);
                    RewriteBirthdaysOrHolidaysFile(holidayNotifications, "Holidays.dat");
                    UpdateNotifTable(dataGridView4, holidayNotifications);
                    break;
            }
            DetermineNearestNotif();
            showNotificationChecker.StartAsync();
        }

        /// <summary>
        /// Нажатие Отладка -> Показать всплывающее окно
        /// </summary>
        private void ShowPopupWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Так вызывается событие показа уведомления ввиде окна
            NotificationWindow nWindow = new NotificationWindow(this, 
                new Notification() { Text = "Какой-то текст"});
            nWindow.Show();
        }

        /// <summary>
        /// Нажатие Отладка -> Показать простое уведомление
        /// </summary>
        private void ShowSimpleNotifToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Так вызывается событие показа стандартного уведомления Windows
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(5000);
        }

        #endregion

        // То, что происходит при нажатии кнопки "+"
        #region ButtonPlus

        /// <summary>
        /// Создаёт директорию Just Remind в AppData, если она была удалена
        /// и файл в этой директории, имя которого передаётся в fileName.
        /// Возвращает путь к файлу
        /// </summary>
        private string CreateDirAndFile(string fileName)
        {
            string appDataPath =
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string applicationPath = appDataPath + "\\Just Remind";
            string filePath = applicationPath + "\\" + fileName;
            if (!Directory.Exists(applicationPath))
            {
                Directory.CreateDirectory(applicationPath);
                File.Create(filePath).Close();
            }
            return filePath;
        }

        /// <summary>
        /// Обновляет таблицу dataGridView, используя данные из notificationList
        /// </summary>
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
            UpdateImportantNotifTable();
        }

        /// <summary>
        /// Обновляет таблицу с важными уведомлениями
        /// </summary>
        private void UpdateImportantNotifTable()
        {
            dataGridView1.Rows.Clear();
            foreach (Notification notification in importantNotifications)
            {
                string notificationText = notification.Text;
                int indexOfNewLine = notificationText.IndexOf('\n');
                if (indexOfNewLine != -1)
                {
                    string firstRow = notificationText.Substring(0, indexOfNewLine) + "...";
                    dataGridView1.Rows.Add(firstRow);
                }
                else
                    dataGridView1.Rows.Add(notificationText);
            }
        }

        /// <summary>
        /// Перезаписывает файл "Personal.dat"
        /// </summary>
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
                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
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

        /// <summary>
        /// Перезаписывает файл "Birthdays.dat" или "Holidays.dat" в зависимости
        /// от того, какие notificationList и fileName были переданы в качестве аргументов
        /// </summary>
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

        /// <summary>
        /// Нажатие кнопки "+"
        /// </summary>
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
                showNotificationChecker.Stop();
                DetermineNearestNotif();
                showNotificationChecker.StartAsync();
            }
        }

        #endregion

        // Нажатия на ячейки таблиц
        #region DataGridViewClicks

        /// <summary>
        /// Нажатие звёздочки в таблице "Важные"
        /// </summary>
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                int rowNum = e.RowIndex;
                importantNotifications[rowNum].IsImportant = false;
                switch (importantNotifications[rowNum].Category)
                {
                    case NotifCategories.Personal:
                        RewritePersonalFile();
                        break;
                    case NotifCategories.Birthdays:
                        RewriteBirthdaysOrHolidaysFile(birthdayNotifications, "Birthdays.dat");
                        break;
                    case NotifCategories.Holidays:
                        RewriteBirthdaysOrHolidaysFile(holidayNotifications, "Holidays.dat");
                        break;
                }
                importantNotifications.RemoveAt(rowNum);
                UpdateImportantNotifTable();
            }
        }

        /// <summary>
        /// Нажатие звёздочки в таблице "Личные"
        /// </summary>
        private void DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                int rowNum = e.RowIndex;
                if (!personalNotifications[rowNum].IsImportant)
                {
                    personalNotifications[rowNum].IsImportant = true;
                    importantNotifications.Insert(personalNotifications[rowNum]);
                }
                else
                {
                    personalNotifications[rowNum].IsImportant = false;
                    importantNotifications.Remove(personalNotifications[rowNum]);
                }
                RewritePersonalFile();
                UpdateImportantNotifTable();
            }
        }

        /// <summary>
        /// Нажатие звёздочки в таблице "Дни рождения"
        /// </summary>
        private void DataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                int rowNum = e.RowIndex;
                if (!birthdayNotifications[rowNum].IsImportant)
                {
                    birthdayNotifications[rowNum].IsImportant = true;
                    importantNotifications.Insert(birthdayNotifications[rowNum]);
                }
                else
                {
                    birthdayNotifications[rowNum].IsImportant = false;
                    importantNotifications.Remove(birthdayNotifications[rowNum]);
                }
                RewriteBirthdaysOrHolidaysFile(birthdayNotifications, "Birthdays.dat");
                UpdateImportantNotifTable();
            }
        }

        /// <summary>
        /// Нажатие звёздочки в таблице "Праздники"
        /// </summary>
        private void DataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                int rowNum = e.RowIndex;
                if (!holidayNotifications[rowNum].IsImportant)
                {
                    holidayNotifications[rowNum].IsImportant = true;
                    importantNotifications.Insert(holidayNotifications[rowNum]);
                }
                else
                {
                    holidayNotifications[rowNum].IsImportant = false;
                    importantNotifications.Remove(holidayNotifications[rowNum]);
                }
                RewriteBirthdaysOrHolidaysFile(holidayNotifications, "Holidasy.dat");
                UpdateImportantNotifTable();
            }
        }

        #endregion

        /// <summary>
        /// Тут программа выбирает, что показывать после "Ваши задачи"
        /// в зависимости от того, какая вкладка сейчас открыта 
        /// </summary>
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

        /// <summary>
        /// Вставляет отложенное напоминание в необходимый список, пересчитывает
        /// NearestNotification, обновляет таблицы
        /// </summary>
        public void InsertPutedoffNotification(Notification notification)
        {
            switch (notification.Category)
            {
                case NotifCategories.Personal:
                    personalNotifications.Insert(notification);
                    UpdateNotifTable(dataGridView2, personalNotifications);
                    break;
                case NotifCategories.Birthdays:
                    birthdayNotifications.Insert(notification);
                    UpdateNotifTable(dataGridView3, birthdayNotifications);
                    break;
                case NotifCategories.Holidays:
                    holidayNotifications.Insert(notification);
                    UpdateNotifTable(dataGridView4, holidayNotifications);
                    break;
            }
            showNotificationChecker.Stop();
            DetermineNearestNotif();
            showNotificationChecker.StartAsync();
        }
    }
}
