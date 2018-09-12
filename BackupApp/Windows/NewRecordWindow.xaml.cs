using System;
using System.Collections.Generic;
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
    public partial class NewRecordWindow : Window
    {
        #region Свойства
        public string FinalName
        {
            get { return txtName.Text; }
            set { txtName.Text = value; }
        }
        public string FinalValue
        {
            get { return txtValue.Text; }
            set { txtValue.Text = value; }
        }
        public string FinalDescription
        {
            get { return txtDescription.Text; }
            set { txtDescription.Text = value; }
        }
        #endregion

        public NewRecordWindow()
        {
            InitializeComponent();
        }

        #region Обработчики событий
        void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FinalValue))
            {
                MessageBox.Show("Введите значение", "Внимание", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            DialogResult = true;
            Close();
        }
        #endregion


    }
}
