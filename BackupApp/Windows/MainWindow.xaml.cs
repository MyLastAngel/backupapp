using BackupApp.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BackupApp
{
    public partial class MainWindow : Window
    {
        #region Поля
        string currentSessionPassword = "";

        readonly ObservableCollection<DBData> values = new ObservableCollection<DBData>();
        #endregion

        #region Свойства
        public MainApplication App { get { return (MainApplication)Application.Current; } }

        public ObservableCollection<DBData> DataList { get { return values; } }
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            Loaded += MainWindow_Loaded;
        }

        #region Команды

        #region Доступность команд
        void CanExecuteRecAdd(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = listDB != null && listDB.SelectedItem is DBData;
        }
        void CanExecuteRecEdit(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = listRecords != null && listRecords.SelectedItem is DataRecord;
        }
        void CanExecuteRecRemove(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = listRecords != null && listRecords.SelectedItem is DataRecord;
        }
        #endregion

        void ExecuteRecAdd(object sender, ExecutedRoutedEventArgs e)
        {
            var data = listDB.SelectedItem as DBData;
            if (data == null)
                return;

            var win = new NewRecordWindow { Owner = this };
            if (win.ShowDialog() != true || string.IsNullOrEmpty(win.FinalValue))
                return;

            var rec = new DataRecord { Value = win.FinalValue, Name = win.FinalName, Description = win.FinalDescription };

            var eMessage = "";
            if (!SQLManager.TryAddRecord(data.Id, rec, currentSessionPassword, out eMessage))
            {
                MessageBox.Show(eMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            data.Records.Add(rec);
            listRecords.SelectedItem = rec;
        }
        void ExecuteRecEdit(object sender, ExecutedRoutedEventArgs e)
        {
            var rec = listRecords.SelectedItem as DataRecord;
            if (rec == null)
                return;

            var win = new NewRecordWindow { Owner = this, FinalName = rec.Name, FinalValue = rec.Value, FinalDescription = rec.Description };
            if (win.ShowDialog() != true || string.IsNullOrEmpty(win.FinalValue))
                return;

            var tmp = new DataRecord
            {
                Name = win.FinalName,
                Value = win.FinalValue,
                Description = win.FinalDescription,
            };

            var eMessage = "";
            if (!SQLManager.TrUpdateRecord(tmp, currentSessionPassword, out eMessage))
            {
                MessageBox.Show(eMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            rec.Name = tmp.Name;
            rec.Value = tmp.Value;
            rec.Description = tmp.Description;
        }
        void ExecuteRecRemove(object sender, ExecutedRoutedEventArgs e)
        {
            var rec = listRecords.SelectedItem as DataRecord;
            if (rec == null)
                return;

            var data = listDB.SelectedItem as DBData;
            if (data == null)
                return;

            var rez = MessageBox.Show("Подтвердите удаление записи", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Stop);
            if (rez != MessageBoxResult.Yes)
                return;

            var eMessage = "";
            if (!SQLManager.TryRemoveRecord(rec, currentSessionPassword, out eMessage))
            {
                MessageBox.Show(eMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            data.Records.Remove(rec);
        }
        #endregion

        void Login()
        {
            List<DBData> result = null;
            var eMessage = "";
            currentSessionPassword = "";

            values.Clear();

            while (true)
            {
                // проверка
                var win = new LoginWindow() { Owner = this };
                if (win.ShowDialog() != true || !SQLManager.TryLoad(win.Password, out result, out eMessage))
                {
                    if (string.IsNullOrEmpty(eMessage))
                        eMessage = "Ошибка открытия БД";


                    var res = MessageBox.Show(string.Format("{0}\nПовторить попытку регистрации?", eMessage), "Ошибка", MessageBoxButton.YesNo, MessageBoxImage.Stop);
                    if (res == MessageBoxResult.No)
                    {
                        App.Shutdown();
                        return;
                    }
                }
                else
                {
                    currentSessionPassword = win.Password;
                    break;
                }
            }

            DataList.Clear();

            foreach (var v in result)
                values.Add(v);
        }

        #region Обработчики событий
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Login();
        }
        void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        // Текст изменился по данным
        void txtDataFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDataFilter();
        }

        // Добавляем данные
        void btnAddData_Click(object sender, RoutedEventArgs e)
        {
            var win = new NewDataWindow { Owner = this };
            if (win.ShowDialog() != true || string.IsNullOrEmpty(win.FinalUrl))
                return;

            var sIcon = "";
            if (!string.IsNullOrWhiteSpace(win.FinalIconName))
                sIcon = win.FinalIconName;

            var data = new DBData { Url = win.FinalUrl, Description = win.FinalDescription, Icon = sIcon };

            var eMessage = "";
            if (!SQLManager.TryAddData(data, currentSessionPassword, out eMessage))
            {
                MessageBox.Show(eMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            values.Add(data);
            listDB.SelectedItem = data;
        }
        // редактировать данные
        void btnEditData_Click(object sender, RoutedEventArgs e)
        {
            var data = listDB.SelectedItem as DBData;
            if (data == null)
                return;

            var win = new NewDataWindow { Owner = this, FinalDescription = data.Description, FinalUrl = data.Url, FinalIconName = data.Icon };
            if (win.ShowDialog() != true || string.IsNullOrEmpty(win.FinalUrl))
                return;

            var sIcon = "";
            if (!string.IsNullOrWhiteSpace(win.FinalIconName))
                sIcon = win.FinalIconName;

            var tmp = new DBData
            {
                Id = data.Id,
                Url = win.FinalUrl,
                Description = win.FinalDescription,
                Icon = sIcon,
            };

            var eMessage = "";
            if (!SQLManager.TrUpdateData(tmp, currentSessionPassword, out eMessage))
            {
                MessageBox.Show(eMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            data.Url = tmp.Url;
            data.Description = tmp.Description;
            data.Icon = tmp.Icon;
        }
        // Удаление данных
        void btnDeleteData_Click(object sender, RoutedEventArgs e)
        {
            var data = listDB.SelectedItem as DBData;
            if (data == null)
                return;

            var rez = MessageBox.Show("Подтвердите удаление данных", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Stop);
            if (rez != MessageBoxResult.Yes)
                return;

            var eMessage = "";
            if (!SQLManager.TryRemoveData(data, currentSessionPassword, out eMessage))
            {
                MessageBox.Show(eMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            values.Remove(data);
        }

        // Копирование записи в буфер обмена
        void mnuRecValueCopy_Click(object sender, RoutedEventArgs e)
        {
            var rec = listRecords.SelectedItem as DataRecord;
            if (rec == null)
                return;


            Clipboard.Clear();
            Clipboard.SetText(rec.Value);

        }
        #endregion

        // Фильтр данных
        void UpdateDataFilter()
        {
            var collection = (ListCollectionView)CollectionViewSource.GetDefaultView(listDB.ItemsSource);
            if (collection != null)
            {
                if (string.IsNullOrEmpty(txtDataFilter.Text))
                {
                    if (collection.Filter != null)
                        collection.Filter = null;
                }
                else
                    collection.Filter = DataFilter;
            }
        }
        bool DataFilter(object d)
        {
            var data = (DBData)d;
            return data.Url.Contains(txtDataFilter.Text) || data.Id.ToString().Contains(txtDataFilter.Text);
        }
    }
}
