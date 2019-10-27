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
        private bool simpleNotification;
        private bool formClosedOk = false;

        // Цвет кнопки по умолчанию
        private Color defButtonColor = Color.FromKnownColor(KnownColor.ButtonFace);
        // Цвет кнопки при нажатии
        private Color altButtonColor = Color.FromKnownColor(KnownColor.ControlDark);

        public AddNotificationForm()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        // Вызывается при загрузке формы, после конструктора
        private void AddNotificationForm_Load(object sender, EventArgs e)
        {
            Button1_Click(sender, e);
        }

        // Нажатие кнопки "Далее" на 1й панели
        private void Button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            if (radioButton1.Checked)
            {
                panel3.Visible = true;
                simpleNotification = true;
            }
            else
            {
                panel2.Visible = true;
                simpleNotification = false;
            }
        }

        // Нажатие кнопки "Далее" на 2й панели
        private void Button5_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel4.Visible = true;
        }

        // Нажатие кнопки "Далее" на 3й панели
        private void Button26_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel4.Visible = true;
        }

        // Нажатие кнопки "Назад" на 2й панели
        private void Button14_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = true;
        }

        // Нажатие кнопки "Назад" на 3й панели
        private void Button15_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            panel1.Visible = true;
        }

        // Нажатие кнопки "Назад" на 4й панели
        private void Button16_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            if (radioButton1.Checked)
                panel3.Visible = true;
            else
                panel2.Visible = true;
        }

        // Нажатие кнопки "Личные" на панели 1
        private void Button1_Click(object sender, EventArgs e)
        {
            button1.BackColor = altButtonColor;
            button2.BackColor = defButtonColor;
            button3.BackColor = defButtonColor;
            button2.UseVisualStyleBackColor = true;
            button3.UseVisualStyleBackColor = true;
            Notification.Category = NotifCategories.Personal;
        }

        // Нажатие кнопки "Дни рождения" на панели 1
        private void Button2_Click(object sender, EventArgs e)
        {
            button1.BackColor = defButtonColor;
            button2.BackColor = altButtonColor;
            button3.BackColor = defButtonColor;
            button1.UseVisualStyleBackColor = true;
            button3.UseVisualStyleBackColor = true;
            Notification.Category = NotifCategories.Birthdays;
        }

        // Нажатие кнопки "Праздники" на панели 1
        private void Button3_Click(object sender, EventArgs e)
        {
            button1.BackColor = defButtonColor;
            button2.BackColor = defButtonColor;
            button3.BackColor = altButtonColor;
            button1.UseVisualStyleBackColor = true;
            button2.UseVisualStyleBackColor = true;
            Notification.Category = NotifCategories.Holidays;
        }

        // Выбраны варианты "Напоминать один день" и "Не повторять в течении дня"
        private void SimpleNotifNoRepeat(DateTime dateTime)
        {
            int hours = int.Parse(comboBox8.Text);
            int minutes = int.Parse(comboBox7.Text);
            if (hours > 23 || minutes > 59)
            {
                MessageBox.Show("Проверьте корректность введённых данных", "Ошибка");
                return;
            }
            // Добавим часы и минуты к dateTime, взятому из календаря на 3й панели
            dateTime.AddHours(hours);
            dateTime.AddMinutes(minutes);
            // Инициализируем notification, который мы передавали сюда из 
            // главной формы в конструкторе. Таким образом, метод сейчас
            // завершится и эта форма закроется, но необходимые данные
            // будут переданы в главную форму при помощи именно этого
            // объекта (notification). Дальше мы будем с ним работать уже
            // в главной форме.
            Notification.Initialize(richTextBox1.Text, dateTime);
        }

        // Выбраны варианты "Напоминать один день" и "Повторять в течении дня"
        private void SimpleNotifRepeat(DateTime dateTime)
        {
            // Здесь всё аналогично как в SimpleNotifNoRepeat, только инициализация
            // notification требует больше параметров. Основная логика та же
            int hoursInterval = int.Parse(comboBox1.Text);
            int minutesInterval = int.Parse(comboBox2.Text);
            int beginHours = int.Parse(comboBox3.Text);
            int beginMinutes = int.Parse(comboBox4.Text);
            int endHours = int.Parse(comboBox6.Text);
            int endMinutes = int.Parse(comboBox5.Text);
            if (hoursInterval > 23 || minutesInterval > 59 || beginHours > 23 ||
                beginMinutes > 59 || endHours > 23 || endMinutes > 59)
            {
                MessageBox.Show("Проверьте корректность введённых данных", "Ошибка");
                return;
            }
            DateTime begin = new DateTime(0);
            begin.AddHours(beginHours);
            begin.AddMinutes(beginMinutes);
            DateTime end = new DateTime(0);
            end.AddHours(endHours);
            end.AddMinutes(endMinutes);
            Notification.Initialize(richTextBox1.Text, dateTime, hoursInterval,
                minutesInterval, begin, end);
        }

        // Нажатие кнопки "Сохранить" на 4й панели
        private void Button17_Click(object sender, EventArgs e)
        {
            if (simpleNotification)
            {
                // В объект dateTime вносим даннные о дате из календаря.
                // Свойства dateTime.Hours, dateTime.Minutes и т. д.
                // равны нулю
                DateTime dateTime = monthCalendar2.SelectionStart;

                if (radioButton5.Checked)
                    SimpleNotifNoRepeat(dateTime);
                else
                    SimpleNotifRepeat(dateTime);
            }
            //else
            //{
            //    нужно дописать
            //}        
            formClosedOk = true;
            this.Close();
        }

        // Обработка радиобаттона на последней (4й) панели
        private void RadioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                panel5.Visible = true;
                panel6.Visible = false;
            }
            else
            {
                panel5.Visible = false;
                panel6.Visible = true;
            }
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

        // Возвращение формы в первозданный вид
        private void ClearForm()
        {
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel1.Visible = true;

            panel5.Visible = false;
            panel6.Visible = true;

            richTextBox1.Text = string.Empty;
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

        // Вызывается при закрытии формы. Возвращаем всё, как было при открытии
        // и создаём DialogResult
        private void AddNotificationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClearForm();
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
