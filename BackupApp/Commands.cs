using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BackupApp
{
    public static class AppCommands
    {
        public static readonly RoutedUICommand RecAdd,
                                                RecEdit,
                                                RecRemove;
        static AppCommands()
        {
            RecAdd = new RoutedUICommand("Добавить новую запись", "RecAdd", typeof(AppCommands));
            RecEdit = new RoutedUICommand("Редактировать выбранную запись", "RecEdit", typeof(AppCommands));
            RecRemove = new RoutedUICommand("Удалить выбранную запись", "RecRemove", typeof(AppCommands));
        }
    }
}
