using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Just_Remind
{
    class NotificationList : List<Notification>
    {
        public NotificationList() : base() {}

        /// <summary>
        /// Этот метод вставляет напоминание в список так, что напоминания
        /// оказываются отсортированными от самого раннего (0й) элемент к
        /// самому позднему (последний элемент)
        /// </summary>
        public void Insert(Notification notification)
        {
            int i;
            bool indexFound = false;
            DateTime nearestDateTime = notification.NearestDateTime;
            // Сравниваем дату из notification, который передан ввиде параметра с датами напоминаний в списке.
            // Для этого мы используем интервалы в секундах, которые прошли с 01.01.2019. Разумеется, вместо
            // этой даты можно было взять любую другую дату из прошлого.
            double secondsToDT_1 = nearestDateTime.Subtract(new DateTime(2019, 1, 1)).TotalSeconds;
            for (i = 0; i < this.Count && !indexFound; i++)
            {
                double secondsToDT_2 = this[i].NearestDateTime.Subtract(new DateTime(2019, 1, 1)).TotalSeconds;
                if (secondsToDT_1 - secondsToDT_2 <= 0)
                    indexFound = true;
            }
            if (!indexFound)
                i--;
            this.Insert(i, notification);
        }
    }
}
