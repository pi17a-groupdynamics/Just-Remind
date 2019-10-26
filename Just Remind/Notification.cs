using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Just_Remind
{
    public enum NotifCategories { Important, Personal, Birthdays, Holidays}

    public class Notification
    {
        // Флаг, указывающий, нужно ли повторять напоминание в течении дня
        public bool Repeat { get; private set; } = false;
        // Текст напоминания
        public string Text { get; private set; }
        // Время напоминания
        public DateTime DateTime { get; set; }
        // Интервал, через который должно повторяться напоминание в часах
        // (в течении дня), если это необходимо
        private int hoursInterval;
        // Интервал, через который должно повторяться напоминание в минутах
        // (в течении дня), если это необходимо
        private int minutesInterval;
        // Начало повторения напоминания в течении дня
        private DateTime startTime;
        // Конец повторения напоминания в течении дня
        private DateTime endTime;
        // Категория напоминания (важные, личные, дни рождения, праздники)
        public NotifCategories Category { get; set; } 

        public Notification()
        {

        }

        // Конструктор для напоминания, которое не нужно повторять в течении дня
        public Notification(string text, DateTime dateTime)
        {
            Initialize(text, dateTime);
        }

        // Конструктор для напоминания, которое нужно повторять в течении дня
        public Notification(string text, DateTime dateTime, int hoursInterval,
            int minutesInterval, DateTime startTime, DateTime endTime)
        {
            Initialize(text, dateTime, hoursInterval, minutesInterval,
                startTime, endTime);
        }

        public void Initialize(string text, DateTime dateTime)
        {
            Text = text;
            DateTime = dateTime;
            Repeat = false;
        }

        public void Initialize(string text, DateTime dateTime, int hoursInterval,
            int minutesInterval, DateTime startTime, DateTime endTime)
        {
            Text = text;
            DateTime = dateTime;
            Repeat = true;
            this.hoursInterval = hoursInterval;
            this.minutesInterval = minutesInterval;
            this.startTime = startTime;
            this.endTime = endTime;
            DateTime.AddHours(startTime.Hour);
            DateTime.AddMinutes(startTime.Minute);
        }
    }
}
