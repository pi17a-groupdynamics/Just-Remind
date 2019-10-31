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
        /// <summary>
        /// Высота формы по умолчанию
        /// </summary>
        private static readonly int DEFAULT_HEAIGHT = 167;

        /// <summary>
        /// Высота формы после нажатия кнопки "Отложить"
        /// </summary>
        private static readonly int ALT_HEIGHT = 200;

        /// <summary>
        /// Номер элемента в комбобоксе с вариантом "не откладывать"
        /// </summary>
        private static readonly int NO_PUTOFF = 8;

        /// <summary>
        /// Переходит в true при нажатии кнопки "Отложить"
        /// </summary>
        private bool puttoffClicked = false;

        /// <summary>
        /// Основная форма
        /// </summary>
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

        /// <summary>
        /// Конструктор принимает объект главной формы
        /// </summary>
        public NotificationWindow(Form1 mainForm, Notification notification)
        {
            this.TopMost = true;
            InitializeComponent();
            this.mainForm = mainForm;
            this.Notification = notification;
        }

        /// <summary>
        /// Вызывается при загрузке формы (когда форма отображается пользователю)
        /// </summary>
        private void NotificationWindow_Load(object sender, EventArgs e)
        {
            this.Location = new Point(XStartCoord, YStartCoord);
            comboBox1.SelectedIndex = 0;
            this.Height = DEFAULT_HEAIGHT;
            string notifText = string.Empty;
            if (notification.IsImportant)
                notifText += "Важно!\n";
            if (notification.Category == NotifCategories.Birthdays)
                notifText += "День рождения\n";
            notifText += notification.Text;
            label1.Text = notifText;
            this.WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// Нажатие "Ок"
        /// </summary>
        private void Button2_Click(object sender, EventArgs e)
        {
            if (puttoffClicked && comboBox1.SelectedIndex != NO_PUTOFF)
            {
                // Обработка ситуации, если пользователь захотел отложить уведомление
                notification.IsRepeatByDate = false;
                notification.IsRepeatByDay = false;
                notification.IsRepeatByDaysOfWeek = false;
                notification.Category = NotifCategories.Personal;
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

        /// <summary>
        /// Нажатие "Отлолжить"
        /// </summary>
        private void Button1_Click(object sender, EventArgs e)
        {
            this.Height = ALT_HEIGHT;
            puttoffClicked = true;
        }
    }
}
