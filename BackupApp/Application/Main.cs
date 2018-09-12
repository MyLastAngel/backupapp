using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupApp
{
    class Startup
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var wrapper = new SingleInstanceApplicationWrapper();
            wrapper.Run(args);
        }
    }

    public class SingleInstanceApplicationWrapper : WindowsFormsApplicationBase
    {
        #region Поля.
        MainApplication app;
        #endregion

        public SingleInstanceApplicationWrapper()
        {
            IsSingleInstance = true;
        }

        #region Обработчики сигналов.
        protected override bool OnStartup(StartupEventArgs e)
        {
            app = new MainApplication();
            app.Run();
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs e)
        {
            app.Activate(e.CommandLine.ToArray());
        }

        protected override bool OnUnhandledException(Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs e)
        {
            return base.OnUnhandledException(e);
        }
        #endregion
    }
}
