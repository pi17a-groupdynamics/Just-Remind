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
    public partial class NotificationWindow : Form
    {
        // Высота формы по умолчанию
        private static readonly int DEFAULT_HEAIGHT = 167;
        // Высота формы после нажатия кнопки "Отложить"
        private static readonly int ALT_HEIGHT = 200;
        // Номер элемента в комбобоксе с вариантом "не откладывать"
        private static readonly int NO_PUTOFF = 8;

        // Переходит в true при нажатии кнопки "Отложить"
        private bool puttoffClicked = false;

        // Основная форма
        private Form1 mainForm;

        private Notification notification;

        /// <summary>
        /// Это поле обязательно необходимо инициализировать перед показом
        /// окна уведомления. 
        /// Метод get возвращает напоминание, которое сейчас
        /// хранится в этом окне.
        /// Метод set создаёт локальную копию передаваемого напоминания, и
        /// сохраняет её в этом окне до тех пор, пока в это поле не будет
        /// передано другое напоминание.
        /// </summary>
        public Notification Notification
        {
            get { return notification; }
            set
            {
                notification = new Notification();
                notification = value.Copy();
            }
        }

        // Поля статические, чтобы не пересчитывать их каждый раз
        public static int XStartCoord { get; set; }
        public static int YStartCoord { get; set; }

        // Конструктор принимает объект главной формы
        public NotificationWindow(Form1 mainForm, Notification notification)
        {
            this.TopMost = true;
            InitializeComponent();
            this.mainForm = mainForm;
            this.Notification = notification;
        }

        // Вызывается при загрузке формы (когда форма отображается пользователю)
        private void NotificationWindow_Load(object sender, EventArgs e)
        {
            this.Location = new Point(XStartCoord, YStartCoord);
            comboBox1.SelectedIndex = 0;
            this.Height = DEFAULT_HEAIGHT;
            label1.Text = Notification.Text;
            this.WindowState = FormWindowState.Normal;
        }

        // Нажатие на кнопку "Ок"
        private void Button2_Click(object sender, EventArgs e)
        {
            if (puttoffClicked && comboBox1.SelectedIndex != NO_PUTOFF)
            {
                // Обработка ситуации, если пользователь захотел отложить уведомление
                notification.IsRepeatByDate = false;
                notification.IsRepeatByDay = false;
                notification.IsRepeatByDaysOfWeek = false;
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        notification.NearestDateTime = DateTime.Now.AddMinutes(5);
                        break;
                    case 1:
                        notification.NearestDateTime = DateTime.Now.AddMinutes(10);
                        break;
                    case 2:
                        notification.NearestDateTime = DateTime.Now.AddMinutes(15);
                        break;
                    case 3:
                        notification.NearestDateTime = DateTime.Now.AddMinutes(20);
                        break;
                    case 4:
                        notification.NearestDateTime = DateTime.Now.AddMinutes(30);
                        break;
                    case 5:
                        notification.NearestDateTime = DateTime.Now.AddMinutes(40);
                        break;
                    case 6:
                        notification.NearestDateTime = DateTime.Now.AddMinutes(50);
                        break;
                    case 7:
                        notification.NearestDateTime = DateTime.Now.AddHours(1);
                        break;
                }
                mainForm.InsertPutedoffNotification(notification);
            }
            this.Close();
        }

        // Нажатие на кнопку "Отлолжить"
        private void Button1_Click(object sender, EventArgs e)
        {
            this.Height = ALT_HEIGHT;
            puttoffClicked = true;
        }
    }
}
