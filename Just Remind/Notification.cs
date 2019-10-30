using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Just_Remind
{
    public enum NotifCategories { Personal, Birthdays, Holidays }

    public class Notification
    {
        /// <summary>
        /// Текст напоминания
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Количество строк в поле Text
        /// </summary>
        public short RowsNum { get; set; }

        /// <summary>
        /// Ближайшее время напоминания
        /// </summary>
        public DateTime NearestDateTime { get; set; }

        /// <summary>
        /// Час, в который должно быть показано напоминание
        /// </summary>
        public int Hour { get; set; }

        /// <summary>
        /// Минута, вкоторую должно быть показано напоминание
        /// </summary>
        public int Minute { get; set; }

        /// <summary>
        /// Флаг, указывающий, нужно ли повторять напоминание в течении дня
        /// </summary>
        public bool IsRepeatByDay { get; set; } = false;

        /// <summary>
        /// Интервал, через который должно повторяться напоминание в часах
        /// (в течении дня), если это необходимо
        /// </summary>
        public int HoursInterval { get; set; }

        /// <summary>
        /// Интервал, через который должно повторяться напоминание в минутах
        /// (в течении дня), если это необходимо
        /// </summary>
        public int MinutesInterval { get; set; }

        /// <summary>
        /// Начало повторения напоминания в течении дня
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Конец повторения напоминания в течении дня
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Является ли напоминание важным
        /// </summary>
        public bool IsImportant { get; set; } = false;

        /// <summary>
        /// Флаг, указывающий, нужно ли повторять напоминание по дням недели
        /// </summary>
        public bool IsRepeatByDaysOfWeek { get; set; } = false;

        /// <summary>
        /// Флаг, указывающий, нужно ли отображать напоминание в понедельник
        /// </summary>
        public bool IsRepeatOnMonday { get; set; } = false;

        /// <summary>
        /// Флаг, указывающий, нужно ли отображать напоминание во вторник
        /// </summary>
        public bool IsRepeatOnTuesday { get; set; } = false;

        /// <summary>
        /// Флаг, указывающий, нужно ли отображать напоминание в среду
        /// </summary>
        public bool IsRepeatOnWednesday { get; set; } = false;

        /// <summary>
        /// Флаг, указывающий, нужно ли отображать напоминание в четверг
        /// </summary>
        public bool IsRepeatOnThursday { get; set; } = false;

        /// <summary>
        /// Флаг, указывающий, нужно ли отображать напоминание в пятницу
        /// </summary>
        public bool IsRepeatOnFriday { get; set; } = false;

        /// <summary>
        /// Флаг, указывающий, нужно ли отображать напоминание в субботу
        /// </summary>
        public bool IsRepeatOnSaturday { get; set; } = false;

        /// <summary>
        /// Флаг, указывающий, нужно ли отображать напоминание в воскресенье
        /// </summary>
        public bool IsRepeatOnSunday { get; set; } = false;

        /// <summary>
        /// Флаг, указывающий, нужно ли повторять напоминание в определённую дату
        /// </summary>
        public bool IsRepeatByDate { get; set; } = false;

        /// <summary>
        /// Дата, когда напоминание должно быть показано
        /// </summary>
        public DateTime RepeatDate { get; set; }

        /// <summary>
        /// Категория напоминания (личные, дни рождения, праздники)
        /// </summary>
        public NotifCategories Category { get; set; }


        public Notification()
        {

        }

        /// <summary>
        /// Конструктор для напоминания, которое не нужно повторять в течении дня
        /// </summary>
        public Notification(string text, DateTime nearestDateTime)
        {
            Initialize(text, nearestDateTime);
        }

        /// <summary>
        /// Конструктор для напоминания, которое нужно повторять в течении дня
        /// </summary>
        public Notification(string text, DateTime nearestDateTime, int hoursInterval,
            int minutesInterval, DateTime startTime, DateTime endTime)
        {
            Initialize(text, nearestDateTime, hoursInterval, minutesInterval,
                startTime, endTime);
        }

        public void Initialize(string text, DateTime nearestDateTime)
        {
            Text = text;
            NearestDateTime = nearestDateTime;
            IsRepeatByDay = false;
        }

        public void Initialize(string text, DateTime nearestDateTime, int hoursInterval,
            int minutesInterval, DateTime startTime, DateTime endTime)
        {
            Text = text;
            NearestDateTime = nearestDateTime;
            IsRepeatByDay = true;
            HoursInterval = hoursInterval;
            MinutesInterval = minutesInterval;
            StartTime = startTime;
            EndTime = endTime;
            NearestDateTime = NearestDateTime.AddHours(startTime.Hour);
            NearestDateTime = NearestDateTime.AddMinutes(startTime.Minute);
        }

        /// <summary>
        /// Возвращает копию данного объекта Notification
        /// </summary>
        public Notification Copy()
        {
            Notification notification = new Notification
            {
                Category = this.Category,
                EndTime = this.EndTime,
                Hour = this.Hour,
                HoursInterval = this.HoursInterval,
                IsImportant = this.IsImportant,
                IsRepeatByDate = this.IsRepeatByDate,
                IsRepeatByDay = this.IsRepeatByDay,
                IsRepeatByDaysOfWeek = this.IsRepeatByDaysOfWeek,
                IsRepeatOnMonday = this.IsRepeatOnMonday,
                IsRepeatOnTuesday = this.IsRepeatOnTuesday,
                IsRepeatOnWednesday = this.IsRepeatOnWednesday,
                IsRepeatOnThursday = this.IsRepeatOnThursday,
                IsRepeatOnFriday = this.IsRepeatOnFriday,
                IsRepeatOnSaturday = this.IsRepeatOnSaturday,
                IsRepeatOnSunday = this.IsRepeatOnSunday,
                Minute = this.Minute,
                MinutesInterval = this.MinutesInterval,
                NearestDateTime = this.NearestDateTime,
                RepeatDate = this.RepeatDate,
                RowsNum = this.RowsNum,
                StartTime = this.StartTime,
                Text = this.Text
            };
            return notification;
        }
    }
}
