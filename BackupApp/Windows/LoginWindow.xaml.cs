using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BackupApp
{
    public partial class LoginWindow : Window, INotifyPropertyChanged
    {
        #region Свойства
        /// <summary>Название теущей раскладки</summary>
        public string InputLanguage { get { return InputLanguageManager.Current.CurrentInputLanguage.TwoLetterISOLanguageName; } }

        public string Password
        {
            get
            {
                if (password != null)
                    return password.Text;

                return "";
            }
        }
        #endregion

        public LoginWindow()
        {
            InitializeComponent();

            DataContext = this;
            password.Focus();

            InputLanguageManager.Current.InputLanguageChanged += Current_InputLanguageChanged;
        }

        #region Обработчики событий
        void btnReg_Click(object sender, RoutedEventArgs e)
        {
            //if (!Login())
            //{
            //    MessageBox.Show(this, "Введите правильный пароль", "Внимание", MessageBoxButton.OK, MessageBoxImage.Stop);
            //    return;
            //}

            DialogResult = true;
            Close();
        }

        /// <summary>Изменилась раскладка клавиатуры</summary>
        void Current_InputLanguageChanged(object sender, InputLanguageEventArgs e)
        {
            DoPropertyChanged("InputLanguage");
        }
        #endregion

        #region События.
        public event PropertyChangedEventHandler PropertyChanged;

        void DoPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
