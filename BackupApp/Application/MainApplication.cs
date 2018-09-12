using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BackupApp
{
    public class MainApplication : Application
    {
        #region Свойства
        public int MenuButtonSize { get { return 36; } }
        #endregion

        public MainApplication()
        {
            ShutdownMode = ShutdownMode.OnLastWindowClose;
        }

        #region Перегрузка стандартных обработчиков
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Activate(e.Args);
        }
        #endregion

        internal void Activate(string[] args)
        {
            if (MainWindow == null)
            {
                MainWindow = new MainWindow();
            }

            MainWindow.Show();
        }
    }
}
