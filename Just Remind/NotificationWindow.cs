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
        // Основная форма
        private Form mainForm;

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
        public static int xStartCoord;
        public static int yStartCoord;

        // Конструктор принимает объект главной формы
        public NotificationWindow(Form mainForm, Notification notification)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.Notification = notification;
        }

        // Вызывается при загрузке формы (когда форма отображается пользователю)
        private void NotificationWindow_Load(object sender, EventArgs e)
        {
            this.Location = new Point(xStartCoord, yStartCoord);
        }
    }
}
