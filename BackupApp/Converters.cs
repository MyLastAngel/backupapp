using BackupApp.Managers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BackupApp
{
    public class IconToBitmapSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(value as string))
                return null;

            var dir = CommonManager.GetDirectory(DirectoryMode.Icons);
            var destFile = string.Format(@"{0}\{1}", dir, (string)value);

            if (!System.IO.File.Exists(destFile))
                return null;

            var oUri = new Uri(destFile);
            var source = BitmapFrame.Create(oUri);
            source.Freeze();

            return source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value;
        }
    }
}
