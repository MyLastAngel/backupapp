using BackupApp.Managers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class NewDataWindow : Window
    {
        #region Свойства
        public string FinalUrl
        {
            get { return txtUrl.Text; }
            set { txtUrl.Text = value; }
        }
        public string FinalDescription
        {
            get { return txtDescription.Text; }
            set { txtDescription.Text = value; }
        }
        public string FinalIconName
        {
            get { return System.IO.Path.GetFileName(imgIcon.Tag as string); }
            set
            {
                var dir = CommonManager.GetDirectory(DirectoryMode.Icons);

                var destFile = string.Format(@"{0}\{1}", dir, value);
                SetIcon(destFile);
            }
        }
        #endregion

        public NewDataWindow()
        {
            InitializeComponent();
        }

        void SetIcon(string fileName)
        {
            var dir = CommonManager.GetDirectory(DirectoryMode.Icons);

            var destFile = string.Format(@"{0}\{1}", dir, System.IO.Path.GetFileName(fileName));
            if (!File.Exists(destFile))
                File.Copy(fileName, destFile, true);

            var oUri = new Uri(destFile);
            var source = BitmapFrame.Create(oUri);
            source.Freeze();

            //BitmapImage b = new BitmapImage();
            //b.BeginInit();
            //b.UriSource = new Uri("c:\\plus.png");
            //b.EndInit();

            imgIcon.Source = source;
            imgIcon.Tag = destFile;
        }

        #region Обработчики событий
        void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUrl.Text))
            {
                MessageBox.Show("Введите имя", "Внимание", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            DialogResult = true;
            Close();
        }

        void imgIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var openFileDialog = new OpenFileDialog { Multiselect = false, Filter = "Файлы рисунков (*.png, *.jpg)|*.png;*.jpg;*.jpeg;" };
            if (openFileDialog.ShowDialog() != true)
                return;

            SetIcon(openFileDialog.FileName);
        }
        void mnuIconRemove_Click(object sender, RoutedEventArgs e)
        {
            imgIcon.Tag = null;

            var oUri = new Uri("pack://application:,,,/Images/question.png", UriKind.RelativeOrAbsolute);
            var source = BitmapFrame.Create(oUri);
            source.Freeze();

            imgIcon.Source = source;
        }
        #endregion


    }
}
