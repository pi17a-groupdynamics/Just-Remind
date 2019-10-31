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
        // На этой форме для удобства все элементы расположены на панелях.
        // Я так сделал потому, что форма показывает пользователю как бы
        // 4 разных "экрана". Номера панелей соответствуют номерам скетчей,
        // Которые сбрасывал в конфу Антон. В нескольких последующих методах
        // Прописана обработка событий нажатия кнопок, осовная функция 
        // которых - переключения "экранов" формы.
        // Я развернул экраны формы на одно большое окно. Разумеется, когда
        // мы закончим работу - мы свернём это дело обратно в одну небольшую 
        // красивую формочку. 
        // Пока что так нужно для удобства.
        // Если что, изначальный размер окна был 553 x 396.

        public Notification Notification { get; set; }
        private bool formClosedOk = false;
        private bool catchMonthCalendar1_SelectionChange = false;

        /// <summary>
        /// Цвет кнопки по умолчанию
        /// </summary>
        private Color defButtonColor = Color.FromKnownColor(KnownColor.ButtonFace);

        /// <summary>
        /// Цвет кнопки при нажатии
        /// </summary>
        private Color altButtonColor = Color.FromKnownColor(KnownColor.ControlDark);

        public AddNotificationForm()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        /// <summary>
        /// Возвращение формы в первозданный вид
        /// </summary>
        private void ClearForm()
        {
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel1.Visible = true;

            panel5.Visible = false;
            panel6.Visible = true;

            richTextBox1.Text = string.Empty;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            radioButton1.Checked = true;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            monthCalendar1.SelectionStart = DateTime.Now;
            monthCalendar1.SelectionEnd = DateTime.Now;
            monthCalendar2.SelectionStart = DateTime.Now;
            monthCalendar2.SelectionEnd = DateTime.Now;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
            comboBox7.SelectedIndex = 0;
            comboBox8.SelectedIndex = 0;
            radioButton5.Checked = true;
        }

        /// <summary>
        /// Очищает все флаги в напоминании
        /// </summary>
        private void ClearNotification()
        {
            Notification.IsRepeatByDay = false;
            Notification.IsRepeatByDate = false;
            Notification.IsRepeatByDaysOfWeek = false;
            Notification.IsRepeatOnMonday = false;
            Notification.IsRepeatOnTuesday = false;
            Notification.IsRepeatOnWednesday = false;
            Notification.IsRepeatOnThursday = false;
            Notification.IsRepeatOnFriday = false;
            Notification.IsRepeatOnSaturday = false;
            Notification.IsRepeatOnSunday = false;
        }

        /// <summary>
        /// Вызывается при загрузке формы, после конструктора
        /// </summary>
        private void AddNotificationForm_Load(object sender, EventArgs e)
        {
            Button1_Click(sender, e);
            ClearNotification();
            ClearForm();
        }

        #region Panel1

        /// <summary>
        /// Нажатие кнопки "Личные" на панели 1
        /// </summary>
        private void Button1_Click(object sender, EventArgs e)
        {
            button1.BackColor = altButtonColor;
            button2.BackColor = defButtonColor;
            button3.BackColor = defButtonColor;
            button2.UseVisualStyleBackColor = true;
            button3.UseVisualStyleBackColor = true;
            Notification.Category = NotifCategories.Personal;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
        }

        /// <summary>
        /// Нажатие кнопки "Дни рождения" на панели 1
        /// </summary>
        private void Button2_Click(object sender, EventArgs e)
        {
            button1.BackColor = defButtonColor;
            button2.BackColor = altButtonColor;
            button3.BackColor = defButtonColor;
            button1.UseVisualStyleBackColor = true;
            button3.UseVisualStyleBackColor = true;
            Notification.Category = NotifCategories.Birthdays;
            radioButton2.Checked = true;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
        }

        /// <summary>
        /// Нажатие кнопки "Праздники" на панели 1
        /// </summary>
        private void Button3_Click(object sender, EventArgs e)
        {
            button1.BackColor = defButtonColor;
            button2.BackColor = defButtonColor;
            button3.BackColor = altButtonColor;
            button1.UseVisualStyleBackColor = true;
            button2.UseVisualStyleBackColor = true;
            Notification.Category = NotifCategories.Holidays;
            radioButton2.Checked = true;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
        }

        /// <summary>
        /// Нажатие кнопки "Далее" на 1й панели
        /// </summary>
        private void Button4_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == string.Empty)
            {
                MessageBox.Show("Пожалуйста, введите текст напоминания");
                return;
            }
            panel1.Visible = false;
            if (radioButton2.Checked && Notification.Category == NotifCategories.Personal)
                panel2.Visible = true;
            else
                panel3.Visible = true;
        }

        #endregion

        #region Panel2

        /// <summary>
        /// Проверяет, нужно ли ставить в Notification флаг IsRepeatByDaysOfWeek.
        /// Если напоминание должно повторяться хотя бы в один из дней недели, то
        /// флаг будет true.
        /// </summary>
        private void CheckRepeatByDayOfWeek()
        {
            if (Notification.IsRepeatOnMonday || Notification.IsRepeatOnTuesday ||
                Notification.IsRepeatOnWednesday || Notification.IsRepeatOnThursday ||
                Notification.IsRepeatOnFriday || Notification.IsRepeatOnSaturday ||
                Notification.IsRepeatOnSunday)
            {
                Notification.IsRepeatByDaysOfWeek = true;
            }
            else
                Notification.IsRepeatByDaysOfWeek = false;
        }

        /// <summary>
        /// Сбрасывает все изменения в повторении по дате
        /// </summary>
        private void DischargeRepeatByDate()
        {
            Notification.IsRepeatByDate = false;
            // Изменение этого флага нужно из-за того, что 
            // при изменении monthCalendar1 сбрасываются дни недели
            catchMonthCalendar1_SelectionChange = true;
            monthCalendar1.SelectionStart = DateTime.Now;
            catchMonthCalendar1_SelectionChange = false;
        }

        /// <summary>
        /// Нажатие кнопки "ПН" на 2й панели
        /// </summary>
        private void Button6_Click(object sender, EventArgs e)
        {
            DischargeRepeatByDate();
            if (!Notification.IsRepeatOnMonday)
            {
                button6.BackColor = altButtonColor;
                Notification.IsRepeatOnMonday = true;
            }
            else
            {
                button6.BackColor = defButtonColor;
                button6.UseVisualStyleBackColor = true;
                Notification.IsRepeatOnMonday = false;
                checkBox1.Checked = false;
            }
            CheckRepeatByDayOfWeek();
        }

        /// <summary>
        /// Нажатие кнопки "ВТ" на 2й панели
        /// </summary>
        private void Button8_Click(object sender, EventArgs e)
        {
            DischargeRepeatByDate();
            if (!Notification.IsRepeatOnTuesday)
            {
                button8.BackColor = altButtonColor;
                Notification.IsRepeatOnTuesday = true;
            }
            else
            {
                button8.BackColor = defButtonColor;
                button8.UseVisualStyleBackColor = true;
                Notification.IsRepeatOnTuesday = false;
                checkBox1.Checked = false;
            }
            CheckRepeatByDayOfWeek();
        }

        /// <summary>
        /// Нажатие кнопки "СР" на 2й панели
        /// </summary>
        private void Button10_Click(object sender, EventArgs e)
        {
            DischargeRepeatByDate();
            if (!Notification.IsRepeatOnWednesday)
            {
                button10.BackColor = altButtonColor;
                Notification.IsRepeatOnWednesday = true;
            }
            else
            {
                button10.BackColor = defButtonColor;
                button10.UseVisualStyleBackColor = true;
                Notification.IsRepeatOnWednesday = false;
                checkBox1.Checked = false;
            }
            CheckRepeatByDayOfWeek();
        }

        /// <summary>
        /// Нажатие кнопки "ЧТ" на 2й панели
        /// </summary>
        private void Button12_Click(object sender, EventArgs e)
        {
            DischargeRepeatByDate();
            if (!Notification.IsRepeatOnThursday)
            {
                button12.BackColor = altButtonColor;
                Notification.IsRepeatOnThursday = true;
            }
            else
            {
                button12.BackColor = defButtonColor;
                button12.UseVisualStyleBackColor = true;
                Notification.IsRepeatOnThursday = false;
                checkBox1.Checked = false;
            }
            CheckRepeatByDayOfWeek();
        }

        /// <summary>
        /// Нажатие кнопки "ПТ" на 2й панели
        /// </summary>
        private void Button7_Click(object sender, EventArgs e)
        {
            DischargeRepeatByDate();
            if (!Notification.IsRepeatOnFriday)
            {
                button7.BackColor = altButtonColor;
                Notification.IsRepeatOnFriday = true;
            }
            else
            {
                button7.BackColor = defButtonColor;
                button7.UseVisualStyleBackColor = true;
                Notification.IsRepeatOnFriday = false;
                checkBox1.Checked = false;
            }
            CheckRepeatByDayOfWeek();
        }

        /// <summary>
        /// Нажатие кнопки "СБ" на 2й панели
        /// </summary>
        private void Button9_Click(object sender, EventArgs e)
        {
            DischargeRepeatByDate();
            if (!Notification.IsRepeatOnSaturday)
            {
                button9.BackColor = altButtonColor;
                Notification.IsRepeatOnSaturday = true;
            }
            else
            {
                button9.BackColor = defButtonColor;
                button9.UseVisualStyleBackColor = true;
                Notification.IsRepeatOnSaturday = false;
                checkBox2.Checked = false;
            }
            CheckRepeatByDayOfWeek();
        }

        /// <summary>
        /// Нажатие кнопки "ВС" на 2й панели
        /// </summary>
        private void Button11_Click(object sender, EventArgs e)
        {
            DischargeRepeatByDate();
            if (!Notification.IsRepeatOnSunday)
            {
                button11.BackColor = altButtonColor;
                Notification.IsRepeatOnSunday = true;
            }
            else
            {
                button11.BackColor = defButtonColor;
                button11.UseVisualStyleBackColor = true;
                Notification.IsRepeatOnSunday = false;
                checkBox2.Checked = false;
            }
            CheckRepeatByDayOfWeek();
        }

        /// <summary>
        /// Нажатие чекбокса "Будние дни"
        /// </summary>
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                DischargeRepeatByDate();
                button6.BackColor = altButtonColor;
                button8.BackColor = altButtonColor;
                button10.BackColor = altButtonColor;
                button12.BackColor = altButtonColor;
                button7.BackColor = altButtonColor;
                Notification.IsRepeatOnMonday = true;
                Notification.IsRepeatOnTuesday = true;
                Notification.IsRepeatOnWednesday = true;
                Notification.IsRepeatOnThursday = true;
                Notification.IsRepeatOnFriday = true;
                Notification.IsRepeatByDaysOfWeek = true;
            }
        }

        /// <summary>
        /// Нажатие чекбокса "Выходные дни"
        /// </summary>
        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                DischargeRepeatByDate();
                button9.BackColor = altButtonColor;
                button11.BackColor = altButtonColor;
                Notification.IsRepeatOnSaturday = true;
                Notification.IsRepeatOnSunday = true;
                Notification.IsRepeatByDaysOfWeek = true;
            }
        }

        /// <summary>
        /// Сбрасывает все изменения в днях недели
        /// </summary>
        private void DischargeDaysOfWeek()
        {
            Notification.IsRepeatByDaysOfWeek = false;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            button6.BackColor = defButtonColor;
            button6.UseVisualStyleBackColor = true;
            Notification.IsRepeatOnMonday = false;
            button7.BackColor = defButtonColor;
            button7.UseVisualStyleBackColor = true;
            Notification.IsRepeatOnTuesday = false;
            button8.BackColor = defButtonColor;
            button8.UseVisualStyleBackColor = true;
            Notification.IsRepeatOnWednesday = false;
            button9.BackColor = defButtonColor;
            button9.UseVisualStyleBackColor = true;
            Notification.IsRepeatOnThursday = false;
            button10.BackColor = defButtonColor;
            button10.UseVisualStyleBackColor = true;
            Notification.IsRepeatOnFriday = false;
            button11.BackColor = defButtonColor;
            button11.UseVisualStyleBackColor = true;
            Notification.IsRepeatOnSaturday = false;
            button12.BackColor = defButtonColor;
            button12.UseVisualStyleBackColor = true;
            Notification.IsRepeatOnSunday = false;
        }

        /// <summary>
        /// Выбрана новая дата на календаре 2й панели
        /// </summary>
        private void MonthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            // Иногда нужно вернуть дату на место, а дни недели сбрасывать не нужно.
            // Поэтому нужна эта проверка
            if (!catchMonthCalendar1_SelectionChange)
            {
                DischargeDaysOfWeek();
                Notification.IsRepeatByDate = true;
                DateTime monthCalendarDateTime = monthCalendar1.SelectionStart;
                Notification.RepeatDate = new DateTime(monthCalendarDateTime.Year,
                    monthCalendarDateTime.Month, monthCalendarDateTime.Day);
            }
        }

        /// <summary>
        /// Нажатие кнопки "Назад" на 2й панели
        /// </summary>
        private void Button14_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = true;
            DischargeRepeatByDate();
            DischargeDaysOfWeek();
        }

        /// <summary>
        /// Нажатие кнопки "Далее" на 2й панели
        /// </summary>
        private void Button5_Click(object sender, EventArgs e)
        {
            if (!Notification.IsRepeatByDate && !Notification.IsRepeatByDaysOfWeek)
            {
                MessageBox.Show("Вы не выбрали повторение ни по дате, ни по дням недели. " +
                    "Пожалуйста, выберите, когда напоминание должно повторяться, чтобы продолжить.",
                    "Ошибка");
                return;
            }
            panel2.Visible = false;
            panel4.Visible = true;
        }

        #endregion

        #region Panel3

        /// <summary>
        /// Выбрана новая дата на календаре 3й панели
        /// </summary>
        private void MonthCalendar2_DateChanged(object sender, DateRangeEventArgs e)
        {
            Notification.IsRepeatByDate = true;
            DateTime monthCalendarDateTime = monthCalendar2.SelectionStart;
            Notification.RepeatDate = new DateTime(monthCalendarDateTime.Year,
                monthCalendarDateTime.Month, monthCalendarDateTime.Day);
        }

        /// <summary>
        /// Нажатие кнопки "Назад" на 3й панели
        /// </summary>
        private void Button15_Click(object sender, EventArgs e)
        {
            Notification.IsRepeatByDate = false;
            monthCalendar2.SelectionStart = DateTime.Now;
            panel3.Visible = false;
            panel1.Visible = true;
        }

        /// <summary>
        /// Нажатие кнопки "Далее" на 3й панели
        /// </summary>
        private void Button26_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel4.Visible = true;
        }

        #endregion

        #region Panel4

        /// <summary>
        /// Нажатие кнопки "Назад" на 4й панели
        /// </summary>
        private void Button16_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            if (radioButton1.Checked)
                panel3.Visible = true;
            else
                panel2.Visible = true;
        }

        /// <summary>
        /// Проверяет, введены ли в комбобоксах корректные цифры
        /// </summary>
        private bool CheckRepeatComboboxesCorrect()
        {
            int startHours = int.Parse(comboBox3.Text);
            int startMinutes = int.Parse(comboBox4.Text);
            int endHours = int.Parse(comboBox6.Text);
            int endMinutes = int.Parse(comboBox5.Text);
            int intervalHours = int.Parse(comboBox1.Text);
            int intervalMinutes = int.Parse(comboBox2.Text);
            if (startHours > endHours)
            {
                MessageBox.Show("Начало повторения не может быть позже конца повторения",
                    "Ошибка");
                return false;
            }
            else if (startHours == endHours)
            {
                if (startMinutes > endMinutes)
                {
                    MessageBox.Show("Начало повторения не может быть позже конца повторения",
                    "Ошибка");
                    return false;
                }
            }
            if (intervalHours + intervalMinutes == 0)
            {
                MessageBox.Show("Интервал между повторениями не может быть равен 0 часов 00 минут",
                    "Ошибка");
                return false;
            }

            return true;
        }

        /// <summary>
        ///Считает количество строк в тексте
        /// </summary>
        private short CountRowsNum(string text)
        {
            short rowsNum = 1;
            int textLength = text.Length;
            for (int i = 0; i < textLength; i++)
            {
                if (text[i] == '\n')
                    rowsNum++;
            }
            return rowsNum;
        }

        /// <summary>
        /// Заполняет напоминание с повторением в течении дня 
        /// информацией из комбобоксов 5й панели
        /// </summary>
        private bool FillNotificationRepeatByDay()
        {
            if (!CheckRepeatComboboxesCorrect())
                return false;
            int hoursInterval = int.Parse(comboBox1.Text);
            int minutesInterval = int.Parse(comboBox2.Text);
            int startHours = int.Parse(comboBox3.Text);
            int startMinutes = int.Parse(comboBox4.Text);
            int endHours = int.Parse(comboBox6.Text);
            int endMinutes = int.Parse(comboBox5.Text);
            DateTime startTime = new DateTime(2019, 1, 1);
            startTime = startTime.AddHours(startHours);
            startTime = startTime.AddMinutes(startMinutes);
            DateTime endTime = new DateTime(2019, 1, 1);
            endTime = endTime.AddHours(endHours);
            endTime = endTime.AddMinutes(endMinutes);
            // нужно исправить везде, где есть Add
            Notification.HoursInterval = hoursInterval;
            Notification.MinutesInterval = minutesInterval;
            Notification.StartTime = startTime;
            Notification.EndTime = endTime;
            return true;
        }

        /// <summary>
        /// Проверяет, не создаётся ли уведомление без каких-либо повторений в прошлом
        /// </summary>
        private bool CheckNotifNoRepeats(DateTime dateTimeNow)
        {
            if (Notification.NearestDateTime.CompareTo(dateTimeNow) <= 0)
            {
                MessageBox.Show("Нельзя создать уведомление, которое будет показано в прошлом",
                    "Ошибка");
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Проверяет, не создаётся ли уведомление с повторением в течении дня (и только) в прошлом
        /// </summary>
        private bool CheckNotifRepeatByDay(DateTime dateTimeNow)
        {
            if (Notification.NearestDateTime.CompareTo(dateTimeNow) <= 0)
            {
                MessageBox.Show("Начало показа уведомления должно быть позже текущего времени",
                    "Ошибка");
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Выбраны варианты "Напоминать один день" и "Не повторять в течении дня"
        /// </summary>
        private bool CreateNotification(DateTime dateTimeNow)
        {
            Notification.Text = richTextBox1.Text;
            Notification.RowsNum = CountRowsNum(Notification.Text);
            bool notifIsRepeatByDay;
            if (radioButton5.Checked)
            {
                int hour = int.Parse(comboBox8.Text);
                int minute = int.Parse(comboBox7.Text);
                Notification.Hour = hour;
                Notification.Minute = minute;
                notifIsRepeatByDay = false;
            }
            else
            {
                if (!FillNotificationRepeatByDay())
                    return false;
                notifIsRepeatByDay = true;
            }
            if (Notification.IsRepeatByDate || Notification.IsRepeatByDaysOfWeek)
                Notification.NearestDateTime = NearestDateTimeCounter.CountNearestDateTime(Notification);
            else
            {
                if (!notifIsRepeatByDay)
                {
                    DateTime monthCalendarDateTime = monthCalendar2.SelectionStart;
                    Notification.NearestDateTime = new DateTime(monthCalendarDateTime.Year,
                        monthCalendarDateTime.Month, monthCalendarDateTime.Day);
                    Notification.NearestDateTime = Notification.NearestDateTime.AddHours(Notification.Hour);
                    Notification.NearestDateTime = Notification.NearestDateTime.AddMinutes(Notification.Minute);
                    if (!CheckNotifNoRepeats(dateTimeNow))
                        return false;
                }
                else
                {
                    DateTime monthCalendarDateTime = monthCalendar2.SelectionStart;
                    Notification.NearestDateTime = new DateTime(monthCalendarDateTime.Year,
                        monthCalendarDateTime.Month, monthCalendarDateTime.Day);
                    Notification.NearestDateTime = Notification.NearestDateTime.AddHours(Notification.StartTime.Hour);
                    Notification.NearestDateTime = Notification.NearestDateTime.AddMinutes(Notification.StartTime.Minute);
                    if (!CheckNotifRepeatByDay(dateTimeNow))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Нажатие кнопки "Сохранить" на 4й панели
        /// </summary>
        private void Button17_Click(object sender, EventArgs e)
        {
            if (!CreateNotification(DateTime.Now))
                return;

            formClosedOk = true;
            this.Close();
        }

        // Запрет на ввод в комбобоксы 4й панели некорректных данных
        #region ComboBoxesPanel4_BanIncorrectData

        private void ComboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!char.IsControl(c) && (comboBox3.Text.Length == 2 || !char.IsDigit(c)))
                e.Handled = true;
        }

        private void ComboBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!char.IsControl(c) && (comboBox4.Text.Length == 2 || !char.IsDigit(c)))
                e.Handled = true;
        }

        private void ComboBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!char.IsControl(c) && (comboBox6.Text.Length == 2 || !char.IsDigit(c)))
                e.Handled = true;
        }

        private void ComboBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!char.IsControl(c) && (comboBox5.Text.Length == 2 || !char.IsDigit(c)))
                e.Handled = true;
        }

        private void ComboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!char.IsControl(c) && (comboBox1.Text.Length == 2 || !char.IsDigit(c)))
                e.Handled = true;
        }

        private void ComboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!char.IsControl(c) && (comboBox2.Text.Length == 2 || !char.IsDigit(c)))
                e.Handled = true;
        }

        private void ComboBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!char.IsControl(c) && (comboBox1.Text.Length == 2 || !char.IsDigit(c)))
                e.Handled = true;
        }

        private void ComboBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!char.IsControl(c) && (comboBox1.Text.Length == 2 || !char.IsDigit(c)))
                e.Handled = true;
        }

        private void ComboBox8_Leave(object sender, EventArgs e)
        {
            if (comboBox8.Text == "")
                comboBox8.Text = "0";
        }

        private void ComboBox7_Leave(object sender, EventArgs e)
        {
            if (comboBox7.Text == "")
                comboBox7.Text = "00";
        }

        private void ComboBox1_Leave(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
                comboBox1.Text = "0";
        }

        private void ComboBox2_Leave(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
                comboBox2.Text = "00";
        }

        private void ComboBox3_Leave(object sender, EventArgs e)
        {
            if (comboBox3.Text == "")
                comboBox3.Text = "00";
        }

        private void ComboBox4_Leave(object sender, EventArgs e)
        {
            if (comboBox4.Text == "")
                comboBox4.Text = "00";
        }

        private void ComboBox6_Leave(object sender, EventArgs e)
        {
            if (comboBox6.Text == "")
                comboBox6.Text = "00";
        }

        private void ComboBox5_Leave(object sender, EventArgs e)
        {
            if (comboBox5.Text == "")
                comboBox5.Text = "00";
        }

        #endregion

        /// <summary>
        /// Обработка радиобаттона на 4й панели 
        /// </summary>
        private void RadioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                panel5.Visible = true;
                panel6.Visible = false;
                Notification.IsRepeatByDay = true;
            }
            else
            {
                panel5.Visible = false;
                panel6.Visible = true;
                Notification.IsRepeatByDay = false;
            }
        }

        #endregion

        /// <summary>
        /// Вызывается при закрытии формы. Возвращаем всё, как было при открытии
        /// и создаём DialogResult
        /// </summary>
        private void AddNotificationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (formClosedOk)
            {
                formClosedOk = false;
                this.DialogResult = DialogResult.OK;
            }
            else
                this.DialogResult = DialogResult.Cancel;
        }
    }
}
