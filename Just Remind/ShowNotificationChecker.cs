using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Just_Remind
{
    class ShowNotificationChecker
    {
        // Флаг, указывающий, должен ли метод Work
        // остановить работу
        private volatile bool stopped;

        // Объект главной формы
        private Form1 mainForm;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ShowNotificationChecker(Form1 mainForm)
        {
            this.mainForm = mainForm;
        }

        private void Work()
        {
            while (!stopped)
            {
                // дописать выполнение работы

                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Команда начать работу
        /// </summary>
        public async void StartAsync()
        {
            stopped = false;
            await Task.Run(() => Work());
        }

        /// <summary>
        /// Команда остановить работу
        /// </summary>
        public void Stop()
        {
            stopped = true;
        }
    }
}
