using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Just_Remind
{
    class NearestDateTimeCounter
    {
        /// <summary>
        /// Возвращает 1, если date1 позже, чем date2.
        /// Возращает 0, если date1 совпадает с date2.
        /// Возвращает -1, если date1 меньше, чем date2.
        /// </summary>
        private static int CompareDates(DateTime date1, DateTime date2)
        {
            // Если это значение больше 0, то дата наступит в этом году.
            // Если = 0, то месяц совпадает.
            // Если меньше 0, то в этом году дата не наступит.
            int monthComparison = date1.Month - date2.Month;

            if (monthComparison > 0)
                return 1;
            else if (monthComparison == 0)
            {
                // По аналогии с месяцем
                int dayComparison = date1.Day - date2.Day;
                if (dayComparison > 0)
                    return 1;
                else if (dayComparison == 0)
                    return 0;
                else
                    return -1;
            }
            else
                return -1;
        }

        /// <summary>
        /// Возвращает 1, если time1 позже, чем time2.
        /// Возращает 0, если time1 совпадает с time2.
        /// Возвращает -1, если time1 меньше, чем time2.
        /// </summary>
        private static int CompareTimes(DateTime time1, DateTime time2)
        {
            int hoursComparison = time1.Hour - time2.Hour;

            if (hoursComparison > 0)
                return 1;
            else if (hoursComparison == 0)
            {
                int minutesComparison = time1.Minute - time2.Minute;
                if (minutesComparison > 0)
                    return 1;
                else if (minutesComparison == 0)
                    return 0;
                else
                    return -1;
            }
            else
                return -1;
        }

        // Добавляет время к уже готовой дате, если эта дата - не сегодня
        private static DateTime CountTimeNotToday(Notification notification,
            DateTime notificationDate)
        {
            if (!notification.IsRepeatByDay)
                return notificationDate.AddHours(notification.Hour).AddMinutes(notification.Minute);
            else
            {
                DateTime startTime = notification.StartTime;
                return notificationDate.AddHours(startTime.Hour).AddMinutes(startTime.Minute);
            }
        }

        #region RepeatByDaysOfWeek

        // Отвечает, является ли day подходящим днём недели для показа уведомления
        private static bool IsSuitableDay(Notification notification, DateTime day)
        {
            DayOfWeek dayOfWeek = day.DayOfWeek;
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    if (notification.IsRepeatOnMonday)
                        return true;
                    break;
                case DayOfWeek.Tuesday:
                    if (notification.IsRepeatOnTuesday)
                        return true;
                    break;
                case DayOfWeek.Wednesday:
                    if (notification.IsRepeatOnWednesday)
                        return true;
                    break;
                case DayOfWeek.Thursday:
                    if (notification.IsRepeatOnThursday)
                        return true;
                    break;
                case DayOfWeek.Friday:
                    if (notification.IsRepeatOnFriday)
                        return true;
                    break;
                case DayOfWeek.Saturday:
                    if (notification.IsRepeatOnSaturday)
                        return true;
                    break;
                case DayOfWeek.Sunday:
                    if (notification.IsRepeatOnSunday)
                        return true;
                    break;
            }
            return false;
        }

        // Считает, на склоько дней нужно переместиться вперёд относительно day,
        // чтобы напоминание было показано в нужный день
        private static int CountDaysToCarryover(Notification notification, DateTime day)
        {
            DayOfWeek dayOfWeek = day.DayOfWeek;
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    if (notification.IsRepeatOnTuesday)
                        return 1;
                    else if (notification.IsRepeatOnWednesday)
                        return 2;
                    else if (notification.IsRepeatOnThursday)
                        return 3;
                    else if (notification.IsRepeatOnFriday)
                        return 4;
                    else if (notification.IsRepeatOnSaturday)
                        return 5;
                    else if (notification.IsRepeatOnSunday)
                        return 6;
                    else if (notification.IsRepeatOnMonday)
                        return 7;
                    break;
                case DayOfWeek.Tuesday:
                    if (notification.IsRepeatOnWednesday)
                        return 1;
                    else if (notification.IsRepeatOnThursday)
                        return 2;
                    else if (notification.IsRepeatOnFriday)
                        return 3;
                    else if (notification.IsRepeatOnSaturday)
                        return 4;
                    else if (notification.IsRepeatOnSunday)
                        return 5;
                    else if (notification.IsRepeatOnMonday)
                        return 6;
                    else if (notification.IsRepeatOnTuesday)
                        return 7;
                    break;
                case DayOfWeek.Wednesday:
                    if (notification.IsRepeatOnThursday)
                        return 1;
                    else if (notification.IsRepeatOnFriday)
                        return 2;
                    else if (notification.IsRepeatOnSaturday)
                        return 3;
                    else if (notification.IsRepeatOnSunday)
                        return 4;
                    else if (notification.IsRepeatOnMonday)
                        return 5;
                    else if (notification.IsRepeatOnWednesday)
                        return 6;
                    else if (notification.IsRepeatOnWednesday)
                        return 7;
                    break;
                case DayOfWeek.Thursday:
                    if (notification.IsRepeatOnFriday)
                        return 1;
                    else if (notification.IsRepeatOnSaturday)
                        return 2;
                    else if (notification.IsRepeatOnSunday)
                        return 3;
                    else if (notification.IsRepeatOnMonday)
                        return 4;
                    else if (notification.IsRepeatOnWednesday)
                        return 5;
                    else if (notification.IsRepeatOnWednesday)
                        return 6;
                    else if (notification.IsRepeatOnThursday)
                        return 7;
                    break;
                case DayOfWeek.Friday:
                    if (notification.IsRepeatOnSaturday)
                        return 1;
                    else if (notification.IsRepeatOnSunday)
                        return 2;
                    else if (notification.IsRepeatOnMonday)
                        return 3;
                    else if (notification.IsRepeatOnWednesday)
                        return 4;
                    else if (notification.IsRepeatOnWednesday)
                        return 5;
                    else if (notification.IsRepeatOnThursday)
                        return 6;
                    else if (notification.IsRepeatOnFriday)
                        return 7;
                    break;
                case DayOfWeek.Saturday:
                    if (notification.IsRepeatOnSunday)
                        return 1;
                    else if (notification.IsRepeatOnMonday)
                        return 2;
                    else if (notification.IsRepeatOnWednesday)
                        return 3;
                    else if (notification.IsRepeatOnWednesday)
                        return 4;
                    else if (notification.IsRepeatOnThursday)
                        return 5;
                    else if (notification.IsRepeatOnFriday)
                        return 6;
                    else if (notification.IsRepeatOnSaturday)
                        return 7;
                    break;
                case DayOfWeek.Sunday:
                    if (notification.IsRepeatOnMonday)
                        return 1;
                    else if (notification.IsRepeatOnWednesday)
                        return 2;
                    else if (notification.IsRepeatOnWednesday)
                        return 3;
                    else if (notification.IsRepeatOnThursday)
                        return 4;
                    else if (notification.IsRepeatOnFriday)
                        return 5;
                    else if (notification.IsRepeatOnSaturday)
                        return 6;
                    else if (notification.IsRepeatOnSunday)
                        return 7;
                    break;
            }
            return 0;
        }

        // Фрагмент RepeatByDaysOfWeek_CountTimeToday. Считает, как добавлять время, если
        // напоминание не нужно повторять в течении дня
        private static DateTime RepeatByDaysOfWeek_CountTimeToday_NotRepeatByDay(Notification notification,
            DateTime dateTimeNow)
        {
            //весь метод нужно переписать
            DateTime time = new DateTime(2019, 1, 1);
            time = time.AddHours(notification.Hour);
            time = time.AddMinutes(notification.Minute);
            int timeComparison = CompareTimes(time, dateTimeNow);
            DateTime nearestDateTime;
            if (timeComparison > 0)
                nearestDateTime = new DateTime(dateTimeNow.Year,
                    dateTimeNow.Month, dateTimeNow.Day);
            else
            {
                nearestDateTime = new DateTime(dateTimeNow.Year,
                    dateTimeNow.Month, dateTimeNow.Day);
                int daysToCarryOver = CountDaysToCarryover(notification, dateTimeNow);
                nearestDateTime = nearestDateTime.AddDays(daysToCarryOver);
            }
            nearestDateTime = nearestDateTime.AddHours(notification.Hour);
            nearestDateTime = nearestDateTime.AddMinutes(notification.Minute);
            return nearestDateTime;
        }

        // Фрагмент RepeatByDaysOfWeek_CountTimeToday_RepeatByDay. Считает дату и время,
        // если текущая дата совпадает с датой напоминания, а текущее время
        // находится в интервале повторения напоминания
        private static DateTime RepeatByDaysOfWeek_TimeInInterval(Notification notification,
            DateTime dateTimeNow, DateTime endTime)
        {
            DateTime notificationTime = new DateTime(2019, 1, 1);
            notificationTime = notificationTime.AddHours(notification.StartTime.Hour);
            notificationTime = notificationTime.AddMinutes(notification.StartTime.Minute);
            bool outOfInterval = false;
            int comparisonResult = CompareTimes(notificationTime, dateTimeNow);
            while (comparisonResult <= 0 && !outOfInterval)
            {
                notificationTime = notificationTime.AddHours(notification.HoursInterval);
                notificationTime = notificationTime.AddMinutes(notification.MinutesInterval);
                comparisonResult = CompareTimes(notificationTime, dateTimeNow);
                int outOfIntervalComparison = CompareTimes(endTime, notificationTime);
                if (outOfIntervalComparison < 0)
                    outOfInterval = true;
            }
            if (outOfInterval)
            {
                DateTime nearestDateTime = new DateTime(dateTimeNow.Year,
                    dateTimeNow.Month, dateTimeNow.Day);
                int daysToCarryover = CountDaysToCarryover(notification, dateTimeNow);
                nearestDateTime = nearestDateTime.AddDays(daysToCarryover);
                nearestDateTime = nearestDateTime.AddHours(notification.StartTime.Hour);
                nearestDateTime = nearestDateTime.AddMinutes(notification.StartTime.Minute);
                return nearestDateTime;
            }
            else
            {
                DateTime nearestDateTime = new DateTime(dateTimeNow.Year,
                    dateTimeNow.Month, dateTimeNow.Day);
                nearestDateTime = nearestDateTime.AddHours(notificationTime.Hour);
                nearestDateTime = nearestDateTime.AddMinutes(notificationTime.Minute);
                return nearestDateTime;
            }
        }

        // Фрагмент RepeatByDaysOfWeek_CountTimeToday. Считает, как добавлять время, если
        // напоминание нужно повторять в течении дня
        private static DateTime RepeatByDaysOfWeek_CountTimeToday_RepeatByDay(Notification notification,
            DateTime dateTimeNow)
        {
            DateTime startTime = notification.StartTime;
            DateTime endTime = notification.EndTime;
            int comparisonStart = CompareTimes(startTime, dateTimeNow);
            int comparisonEnd = CompareTimes(endTime, dateTimeNow);
            // Текущее время раньше, чем интервал, в который напоминание повторяется
            if (comparisonStart > 0 && comparisonEnd > 0)
            {
                DateTime nearestDateTime = new DateTime(dateTimeNow.Year,
                    dateTimeNow.Month, dateTimeNow.Day);
                nearestDateTime = nearestDateTime.AddHours(notification.StartTime.Hour);
                nearestDateTime = nearestDateTime.AddMinutes(notification.StartTime.Minute);
                return nearestDateTime;
            }
            // Текущее время позже, чем интерал, в который напоминание повторяется, либо
            // мы находимся в последней минуте интервала
            else if (comparisonStart < 0 && comparisonEnd <= 0)
            {
                DateTime nearestDateTime = new DateTime(dateTimeNow.Year,
                    dateTimeNow.Month, dateTimeNow.Day);
                int daysToCarryOver = CountDaysToCarryover(notification, dateTimeNow);
                nearestDateTime = nearestDateTime.AddDays(daysToCarryOver);
                nearestDateTime = nearestDateTime.AddHours(notification.StartTime.Hour);
                nearestDateTime = nearestDateTime.AddMinutes(notification.StartTime.Minute);
                return nearestDateTime;
            }
            // Текущее время попадает в интервал
            else
                return RepeatByDaysOfWeek_TimeInInterval(notification, dateTimeNow, endTime);
        }

        // Добавляет время к дате, если эта дата - сегодня.
        // Если текущее время больше времени, когда должно быть показано уведомление - 
        // переносит дату до следующего дня недедели, когда должно быть показано уведомление.
        // Если напоминание должно показываться в течении интервала - высчитывает, когда
        // должно быть показано напоминание (сегодня ли, или в другой день недели, и в какое время)
        private static DateTime RepeatByDaysOfWeek_CountTimeToday(Notification notification,
            DateTime dateTimeNow, DateTime notificationDate)
        {
            if (!notification.IsRepeatByDay)
                return RepeatByDaysOfWeek_CountTimeToday_NotRepeatByDay(notification,
                    dateTimeNow);
            else
                return RepeatByDaysOfWeek_CountTimeToday_RepeatByDay(notification,
                        dateTimeNow);
        }

        // Если повтор по дням недели
        private static DateTime CountIfRepeatByDaysOfWeek(Notification notification,
            DateTime dateTimeNow)
        {
            // Начинаем считать дату, когда будет показано уведомление.
            // Для начала берём сегодняшний день и прибавляем к нему столько дней,
            // сколько нужно, чтобы был тот день недели, когда уведомление должно быть показано
            DateTime notificationDate = new DateTime(dateTimeNow.Year,
                dateTimeNow.Month, dateTimeNow.Day);
            if (!IsSuitableDay(notification, notificationDate))
            {
                int daysToCarryOver = CountDaysToCarryover(notification, notificationDate);
                notificationDate = notificationDate.AddDays(daysToCarryOver);
            }

            int comparison = CompareDates(notificationDate, dateTimeNow);
            // Если дата оказалась позже текущей
            if (comparison > 0)
                // Считаем время и добавляем к дате
                return CountTimeNotToday(notification, notificationDate);
            // Если эта дата - сегодня
            else 
                return RepeatByDaysOfWeek_CountTimeToday(notification, dateTimeNow, notificationDate);
        }

        #endregion

        #region RepeatByDate

        // Фрагмент RepeatByDate_CountTimeToday. Считает, как добавлять время, если
        // напоминание не нужно повторять в течении дня
        private static DateTime RepeatByDate_CountTimeToday_NotRepeatByDay(Notification notification,
            DateTime dateTimeNow)
        {
            DateTime time = new DateTime(2019, 1, 1);
            time = time.AddHours(notification.Hour);
            time = time.AddMinutes(notification.Minute);
            int timeComparison = CompareTimes(time, dateTimeNow);
            DateTime nearestDateTime;
            if (timeComparison > 0)
                nearestDateTime = new DateTime(dateTimeNow.Year,
                    dateTimeNow.Month, dateTimeNow.Day);
            else
                nearestDateTime = new DateTime(dateTimeNow.Year + 1,
                    dateTimeNow.Month, dateTimeNow.Day);
            nearestDateTime = nearestDateTime.AddHours(notification.Hour);
            nearestDateTime = nearestDateTime.AddMinutes(notification.Minute);
            return nearestDateTime;
        }

        // Фрагмент RepeatByDate_CountTimeToday_RepeatByDay. Считает дату и время,
        // если текущая дата совпадает с датой напоминания, а текущее время
        // находится в интервале повторения напоминания
        private static DateTime RepeatByDate_TimeInInterval(Notification notification,
            DateTime dateTimeNow, DateTime endTime)
        {
            DateTime notificationTime = new DateTime(2019, 1, 1);
            notificationTime = notificationTime.AddHours(notification.StartTime.Hour);
            notificationTime = notificationTime.AddMinutes(notification.StartTime.Minute);
            bool outOfInterval = false;
            int comparisonResult = CompareTimes(notificationTime, dateTimeNow);
            while (comparisonResult <= 0 && !outOfInterval)
            {
                notificationTime = notificationTime.AddHours(notification.HoursInterval);
                notificationTime = notificationTime.AddMinutes(notification.MinutesInterval);
                comparisonResult = CompareTimes(notificationTime, dateTimeNow);
                int outOfIntervalComparison = CompareTimes(endTime, notificationTime);
                if (outOfIntervalComparison < 0)
                    outOfInterval = true;
            }
            if (outOfInterval)
            {
                DateTime nearestDateTime = new DateTime(dateTimeNow.Year + 1,
                    dateTimeNow.Month, dateTimeNow.Day);
                nearestDateTime = nearestDateTime.AddHours(notification.StartTime.Hour);
                nearestDateTime = nearestDateTime.AddMinutes(notification.StartTime.Minute);
                return nearestDateTime;
            }
            else
            {
                DateTime nearestDateTime = new DateTime(dateTimeNow.Year,
                    dateTimeNow.Month, dateTimeNow.Day);
                nearestDateTime = nearestDateTime.AddHours(notificationTime.Hour);
                nearestDateTime = nearestDateTime.AddMinutes(notificationTime.Minute);
                return nearestDateTime;
            }
        }

        // Фрагмент RepeatByDate_CountTimeToday. Считает, как добавлять время, если
        // напоминание нужно повторять в течении дня
        private static DateTime RepeatByDate_CountTimeToday_RepeatByDay(Notification notification,
            DateTime dateTimeNow)
        {
            DateTime startTime = notification.StartTime;
            DateTime endTime = notification.EndTime;
            int comparisonStart = CompareTimes(startTime, dateTimeNow);
            int comparisonEnd = CompareTimes(endTime, dateTimeNow);
            // Текущее время раньше, чем интервал, в который напоминание повторяется
            if (comparisonStart > 0 && comparisonEnd > 0)
            {
                DateTime nearestDateTime = new DateTime(dateTimeNow.Year,
                    dateTimeNow.Month, dateTimeNow.Day);
                nearestDateTime = nearestDateTime.AddHours(notification.StartTime.Hour);
                nearestDateTime = nearestDateTime.AddMinutes(notification.StartTime.Minute);
                return nearestDateTime;
            }
            // Текущее время позже, чем интерал, в который напоминание повторяется, либо
            // мы находимся в последней минуте интервала
            else if (comparisonStart < 0 && comparisonEnd <= 0)
            {
                DateTime nearestDateTime = new DateTime(dateTimeNow.Year + 1,
                    dateTimeNow.Month, dateTimeNow.Day);
                nearestDateTime = nearestDateTime.AddHours(notification.StartTime.Hour);
                nearestDateTime = nearestDateTime.AddMinutes(notification.StartTime.Minute);
                return nearestDateTime;
            }
            // Текущее время попадает в интервал
            else
                return RepeatByDate_TimeInInterval(notification, dateTimeNow, endTime);
        }

        // Добавляет время к дате, если эта дата - сегодня.
        // Если текущее время больше времени, когда должно быть показано уведомление - 
        // переносит дату на год.
        // Если напоминание должно показываться в течении интервала - высчитывает, когда
        // должно быть показано напоминание (сегодня ли, или через год, и в какое время)
        private static DateTime RepeatByDate_CountTimeToday(Notification notification,
            DateTime dateTimeNow)
        {
            if (!notification.IsRepeatByDay)
                return RepeatByDate_CountTimeToday_NotRepeatByDay(notification,
                    dateTimeNow);
            else
                return RepeatByDate_CountTimeToday_RepeatByDay(notification,
                        dateTimeNow);
        }

        // Если повтор по определённой дате в году
        private static DateTime CountIfRepeatByDate(Notification notification,
            DateTime dateTimeNow)
        {
            // Начинаем считать дату, когда будет показано уведомление.
            // Для начала берём день и месяц из notification.RepeatDate,
            // а год берём текущий.
            DateTime notificationDate = new DateTime(dateTimeNow.Year,
                notification.RepeatDate.Month, notification.RepeatDate.Day);

            int comparison = CompareDates(notificationDate, dateTimeNow);
            // Если дата оказалась позже текущей
            if (comparison > 0)
                // Считаем время и добавляем к дате
                return CountTimeNotToday(notification, notificationDate);
            // Если эта дата - сегодня
            else if (comparison == 0)
                return RepeatByDate_CountTimeToday(notification, dateTimeNow);
            // Если дата оказалась раньше текущей
            else
            {
                // Добавляем годик
                notificationDate = notificationDate.AddYears(1);
                // И считаем время
                return CountTimeNotToday(notification, notificationDate);
            }
        }

        #endregion

        /// <summary>
        /// Позволяет посчитать ближайшую дату и время показа уведомления. 
        /// Для этого в уведомлении должна быть инициализирована вся информация,
        /// касающаяся даты и времени его показа (по дням недели, датам, повтор
        /// в течении дня и т. д.). Этот метод считает ближайшую дату только для 
        /// уведомления, которое повторяется по датам в году либо по дням недели. 
        /// Для другого уведомления он вернёт дату и время,
        /// которые УЖЕ находятся у этого уведомления в NearestDateTime.
        /// </summary>
        public static DateTime CountNearestDateTime(Notification notification)
        {
            DateTime dateTimeNow = DateTime.Now;
            if (notification.IsRepeatByDaysOfWeek)
                return CountIfRepeatByDaysOfWeek(notification, dateTimeNow);
            else if (notification.IsRepeatByDate)
                return CountIfRepeatByDate(notification, dateTimeNow);
            else
                return notification.NearestDateTime;
        }
    }
}
