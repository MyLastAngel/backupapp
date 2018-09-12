using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupApp
{
    public class DBData : INotifyPropertyChanged
    {
        #region Поля
        string url = "", description = "", icon = "";

        readonly ObservableCollection<DataRecord> records = new ObservableCollection<DataRecord>();

        #endregion

        #region Свойства
        public long Id { get; set; }

        public string Url
        {
            get { return url; }
            set
            {
                if (url == value)
                    return;

                url = value;
                DoPropertyChanged("Url");
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                if (description == value)
                    return;

                description = value;
                DoPropertyChanged("Description");
            }
        }
        public string Icon
        {
            get { return icon; }
            set
            {
                if (icon == value)
                    return;

                icon = value;
                DoPropertyChanged("Icon");
            }
        }

        /// <summary>Записи</summary>
        public ObservableCollection<DataRecord> Records { get { return records; } }
        #endregion

        public DBData()
        {

        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Url, Description);
        }

        #region События
        public event PropertyChangedEventHandler PropertyChanged;
        private void DoPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }

    public class DataRecord
    {
        #region Свойства
        public long Id { get; set; }
        public int DataId { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        #endregion

        public DataRecord()
        {

        }
    }
}
