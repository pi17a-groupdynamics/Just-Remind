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
                notification.Category = value.Category;
                notification.EndTime = value.EndTime;
                notification.Hour = value.Hour;
                notification.HoursInterval = value.HoursInterval;
                notification.IsImportant = value.IsImportant;
                notification.IsRepeatByDate = value.IsRepeatByDate;
                notification.IsRepeatByDay = value.IsRepeatByDay;
                notification.IsRepeatByDaysOfWeek = value.IsRepeatByDaysOfWeek;
                notification.IsRepeatOnMonday = value.IsRepeatOnMonday;
                notification.IsRepeatOnTuesday = value.IsRepeatOnTuesday;
                notification.IsRepeatOnWednesday = value.IsRepeatOnWednesday;
                notification.IsRepeatOnThursday = value.IsRepeatOnThursday;
                notification.IsRepeatOnFriday = value.IsRepeatOnFriday;
                notification.IsRepeatOnSaturday = value.IsRepeatOnSaturday;
                notification.IsRepeatOnSunday = value.IsRepeatOnSunday;
                notification.Minute = value.Minute;
                notification.MinutesInterval = value.MinutesInterval;
                notification.NearestDateTime = value.NearestDateTime;
                notification.RepeatDate = value.RepeatDate;
                notification.RowsNum = value.RowsNum;
                notification.StartTime = value.StartTime;
                notification.Text = value.Text;
            }
        }

        //поля статические, чтобы не пересчитывать их каждый раз
        public static int xStartCoord;
        public static int yStartCoord;

        //в конструктор обязательно передавать текст по понятным причинам
        public NotificationWindow()
        {
            InitializeComponent();
            Location = new Point(xStartCoord, yStartCoord);
        }
    }
}
