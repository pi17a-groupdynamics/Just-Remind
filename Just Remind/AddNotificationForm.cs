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

        Notification notification;
        bool simpleNotification;

        public AddNotificationForm(Notification notification)
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            this.notification = notification;
        }

        private void button4_Click(object sender, EventArgs e)
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
            if (simpleNotification)
            {
                if (radioButton5.Checked)
                {
                    int hours = int.Parse(comboBox8.Text);
                    int minutes = int.Parse(comboBox7.Text);
                    if (hours > 23 || minutes > 59)
                    {
                        MessageBox.Show("Проверьте корректность введённых данных", "Ошибка");
                        return;
                    }
                    // В объект dateTime вносим даннные о дате из календаря.
                    // Свойства dateTime.Hours, dateTime.Minutes и т. д.
                    // равны нулю
                    DateTime dateTime = monthCalendar2.SelectionStart;
                    // Теперь добавим hours и minutes
                    dateTime.AddHours(hours);
                    dateTime.AddMinutes(minutes);
                    // Инициализируем notification, который мы передавали сюда из 
                    // главной формы в конструкторе. Таким образом, метод сейчас
                    // завершится и эта форма закроется, но необходимые данные
                    // будут переданы в главную форму при помощи именно этого
                    // объекта (notification). Дальше мы будем с ним работать уже
                    // в главной форме.
                    notification.Initialize(richTextBox1.Text, dateTime);
                }
                else
                {
                    // Здесь всё аналогично как в коде выше, только инициализация
                    // notification требует больше параметров. Основная логика 
                    // та же
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
                    DateTime dateTime = monthCalendar2.SelectionStart;
                    DateTime begin = new DateTime(0);
                    begin.AddHours(beginHours);
                    begin.AddMinutes(beginMinutes);
                    DateTime end = new DateTime(0);
                    end.AddHours(endHours);
                    end.AddMinutes(endMinutes);
                    notification.Initialize(richTextBox1.Text, dateTime, hoursInterval,
                        minutesInterval, begin, end);
                }
            }
            //else
            //{
            //    нужно дописать
            //}        
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Это обработка радиобаттона на последней (4й) панели
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
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

        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!char.IsControl(c) && (comboBox3.Text.Length == 2 || !char.IsDigit(c)))
                e.Handled = true;
        }

        private void comboBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!char.IsControl(c) && (comboBox4.Text.Length == 2 || !char.IsDigit(c)))
                e.Handled = true;
        }

        private void comboBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!char.IsControl(c) && (comboBox6.Text.Length == 2 || !char.IsDigit(c)))
                e.Handled = true;
        }

        private void comboBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!char.IsControl(c) && (comboBox5.Text.Length == 2 || !char.IsDigit(c)))
                e.Handled = true;
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!char.IsControl(c) && (comboBox1.Text.Length == 2 || !char.IsDigit(c)))
                e.Handled = true;
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!char.IsControl(c) && (comboBox2.Text.Length == 2 || !char.IsDigit(c)))
                e.Handled = true;
        }

        private void comboBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!char.IsControl(c) && (comboBox1.Text.Length == 2 || !char.IsDigit(c)))
                e.Handled = true;
        }

        private void comboBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!char.IsControl(c) && (comboBox1.Text.Length == 2 || !char.IsDigit(c)))
                e.Handled = true;
        }

        private void comboBox8_Leave(object sender, EventArgs e)
        {
            if (comboBox8.Text == "")
                comboBox8.Text = "0";
        }

        private void comboBox7_Leave(object sender, EventArgs e)
        {
            if (comboBox7.Text == "")
                comboBox7.Text = "00";
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
                comboBox1.Text = "0";
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
                comboBox2.Text = "00";
        }

        private void comboBox3_Leave(object sender, EventArgs e)
        {
            if (comboBox3.Text == "")
                comboBox3.Text = "00";
        }

        private void comboBox4_Leave(object sender, EventArgs e)
        {
            if (comboBox4.Text == "")
                comboBox4.Text = "00";
        }

        private void comboBox6_Leave(object sender, EventArgs e)
        {
            if (comboBox6.Text == "")
                comboBox6.Text = "00";
        }

        private void comboBox5_Leave(object sender, EventArgs e)
        {
            if (comboBox5.Text == "")
                comboBox5.Text = "00";
        }
    }
}
