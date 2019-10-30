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
        // Делегат, необходимый для вызова метода BeginInvoke()
        // в методе Work()
        private delegate void InvokeDelegate();

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
                if (mainForm.NearestNotification.NearestDateTime.CompareTo(DateTime.Now) <= 0)
                    // BeginInvoke() вызывает метод, передаваемый через делегат, из 
                    // того потока, к которому принадлежит компонент, при помощи которого 
                    // был вызван метод BeginInvoke().
                    // В нашем случае это наш основной поток, в котором работает программа
                    mainForm.BeginInvoke(new InvokeDelegate(mainForm.ShowNearestNotification));

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
