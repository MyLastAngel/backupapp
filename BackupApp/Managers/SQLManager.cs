using System;
using System.Collections.Generic;
using System.Data.SQLite;
using BackupApp;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace BackupApp.Managers
{
    public static class SQLManager
    {
        #region Поля
        const string unit = "SQLManager";
        #endregion

        static SQLManager()
        {

        }

        public static bool TryAddData(DBData newData, string pswd, out string eMessage)
        {
            eMessage = "";

            try
            {
                using (var client = new SQLiteConnection(GetConnectionString(pswd, false)))
                {
                    client.Open();

                    Create(client);

                    var cmd = "INSERT INTO DATA ('Url', 'Description', 'Icon') VALUES(@Url, @Description, @Icon)";
                    using (var insertMsgCommand = new SQLiteCommand(cmd, client))
                    {
                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@Url", newData.Url) { DbType = DbType.String });
                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@Description", newData.Description) { DbType = DbType.String });
                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@Icon", newData.Icon) { DbType = DbType.String });
                        insertMsgCommand.ExecuteNonQuery();
                    }

                    newData.Id = client.LastInsertRowId;
                }

                return true;
            }
            catch (Exception ex)
            {
                eMessage = string.Format("Ошибка сохранения данных: {0}", ex.Message);
                return false;
            }
        }
        public static bool TryAddRecord(long dataId, DataRecord rec, string pswd, out string eMessage)
        {
            eMessage = "";

            try
            {
                using (var client = new SQLiteConnection(GetConnectionString(pswd, false)))
                {
                    client.Open();

                    Create(client);

                    var cmd = "INSERT INTO DATA_RECORD ('DataId', 'Name', 'Value', 'Description') VALUES(@DataId, @Name, @Value, @Description)";
                    using (var insertMsgCommand = new SQLiteCommand(cmd, client))
                    {
                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@DataId", (int)dataId) { DbType = DbType.Int32 });
                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@Name", rec.Name) { DbType = DbType.String });
                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@Value", rec.Value) { DbType = DbType.String });
                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@Description", rec.Description) { DbType = DbType.String });

                        insertMsgCommand.ExecuteNonQuery();
                    }

                    rec.Id = client.LastInsertRowId;
                }

                return true;
            }
            catch (Exception ex)
            {
                eMessage = string.Format("Ошибка сохранения новой записи: {0}", ex.Message);
                return false;
            }
        }

        /// <summary>Изменение полей</summary>
        public static bool TrUpdateData(DBData data, string pswd, out string eMessage)
        {
            eMessage = "";

            try
            {
                using (var client = new SQLiteConnection(GetConnectionString(pswd, false)))
                {
                    client.Open();

                    Create(client);

                    var cmd = "UPDATE DATA SET Url=@url, Description=@description, Icon=@icon WHERE Id=@id";
                    using (var insertMsgCommand = new SQLiteCommand(cmd, client))
                    {
                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@url", data.Url) { DbType = DbType.String });
                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@description", data.Description) { DbType = DbType.String });
                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@icon", data.Icon) { DbType = DbType.String });
                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@id", data.Id) { DbType = DbType.Int32 });
                        insertMsgCommand.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                eMessage = string.Format("Ошибка сохранения данных: {0}", ex.Message);
                return false;
            }
        }
        /// <summary>Изменение записи</summary>
        public static bool TrUpdateRecord(DataRecord rec, string pswd, out string eMessage)
        {
            eMessage = "";

            try
            {
                using (var client = new SQLiteConnection(GetConnectionString(pswd, false)))
                {
                    client.Open();

                    Create(client);

                    var cmd = "UPDATE DATA_RECORD SET Name=@name, Value=@value, Description=@description WHERE Id=@id";
                    using (var insertMsgCommand = new SQLiteCommand(cmd, client))
                    {
                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@name", rec.Name) { DbType = DbType.String });
                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@value", rec.Value) { DbType = DbType.String });
                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@description", rec.Description) { DbType = DbType.String });
                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@id", rec.Id) { DbType = DbType.Int32 });
                        insertMsgCommand.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                eMessage = string.Format("Ошибка сохранения данных: {0}", ex.Message);
                return false;
            }
        }

        /// <summary>Удаление полей</summary>
        public static bool TryRemoveData(DBData data, string pswd, out string eMessage)
        {
            eMessage = "";

            try
            {
                using (var client = new SQLiteConnection(GetConnectionString(pswd, false)))
                {
                    client.Open();

                    Create(client);

                    var cmd = "DELETE FROM DATA_RECORD WHERE DataId=@id";
                    using (var insertMsgCommand = new SQLiteCommand(cmd, client))
                    {

                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@id", data.Id) { DbType = DbType.Int32 });
                        insertMsgCommand.ExecuteNonQuery();
                    }

                    cmd = "DELETE FROM DATA WHERE Id=@id";
                    using (var insertMsgCommand = new SQLiteCommand(cmd, client))
                    {

                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@id", data.Id) { DbType = DbType.Int32 });
                        insertMsgCommand.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                eMessage = string.Format("Ошибка сохранения данных: {0}", ex.Message);
                return false;
            }
        }
        /// <summary>Удаление записи</summary>
        public static bool TryRemoveRecord(DataRecord rec, string pswd, out string eMessage)
        {
            eMessage = "";

            try
            {
                using (var client = new SQLiteConnection(GetConnectionString(pswd, false)))
                {
                    client.Open();

                    Create(client);

                    var cmd = "DELETE FROM DATA_RECORD WHERE Id=@id";
                    using (var insertMsgCommand = new SQLiteCommand(cmd, client))
                    {

                        insertMsgCommand.Parameters.Add(new SQLiteParameter("@id", rec.Id) { DbType = DbType.Int32 });
                        insertMsgCommand.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                eMessage = string.Format("Ошибка сохранения данных: {0}", ex.Message);
                return false;
            }
        }

        public static bool TryLoad(string pswd, out List<DBData> result, out string eMessage)
        {
            eMessage = "";
            result = new List<DBData>();

            try
            {
                using (var client = new SQLiteConnection(GetConnectionString(pswd, false)))
                {
                    client.Open();

                    Create(client);

                    var query = string.Format("SELECT * FROM DATA;");
                    using (var command = new SQLiteCommand(query, client))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var newData = new DBData
                                {
                                    Id = reader.GetInt32(0),
                                    Url = reader.GetString(1),
                                    Description = reader.GetString(2),
                                    Icon = reader.GetString(3)
                                };
                                result.Add(newData);


                                // Загружаем список записей
                                query = string.Format("SELECT * FROM DATA_RECORD WHERE DataId={0};", newData.Id);
                                using (var command2 = new SQLiteCommand(query, client))
                                {
                                    using (var reader2 = command2.ExecuteReader())
                                    {
                                        while (reader2.Read())
                                        {
                                            // "Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                                            //"DataId INTEGER, " +
                                            //"Name TEXT, " +
                                            //"Value TEXT, " +
                                            //"Description TEXT, " +

                                            newData.Records.Add(
                                                new DataRecord
                                                {
                                                    Id = reader2.GetInt32(0),
                                                    DataId = reader2.GetInt32(1),
                                                    Name = reader2.GetString(2),
                                                    Value = reader2.GetString(3),
                                                    Description = reader2.GetString(4),
                                                });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                eMessage = string.Format("Ошибка Загрузки данных: {0}", ex.Message);
                return false;
            }
        }

        /// <summary>Создание БД если ее нет</summary>
        static void Create(SQLiteConnection client)
        {
            // Настройки
            var s = "PRAGMA synchronous = OFF";
            var command = new SQLiteCommand(s, client);
            command.ExecuteNonQuery();

            s = "PRAGMA journal_mode=WAL";
            command = new SQLiteCommand(s, client);
            command.ExecuteNonQuery();

            s = "PRAGMA cache_size=8000";
            command = new SQLiteCommand(s, client);
            command.ExecuteNonQuery();

            // Создаем таблицу если требуется
            var sql = "CREATE TABLE IF NOT EXISTS DATA (" +
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                    "Url TEXT," +
                    "Description TEXT," +
                    "Icon TEXT); ";

            command = new SQLiteCommand(sql, client);
            command.ExecuteNonQuery();

            // Создаем таблицу если требуется
            sql = "CREATE TABLE IF NOT EXISTS DATA_RECORD (" +
                    "Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                    "DataId INTEGER, " +
                    "Name TEXT, " +
                    "Value TEXT, " +
                    "Description TEXT, " +
                    "FOREIGN KEY(DataId) REFERENCES DATA(Id)" +
                    ");";

            command = new SQLiteCommand(sql, client);
            command.ExecuteNonQuery();
        }


        /// <summary>Возврат строки соединения</summary>
        static string GetConnectionString(string pswd, bool isReadOnly)
        {
            var path = string.Format("{0}\\cache.db", CommonManager.GetDirectory(DirectoryMode.DB));

            if (!File.Exists(path))
                isReadOnly = false;

            var result = string.Format("Data Source='{0}'; Version=3; Password={1}; {2}", path, pswd, isReadOnly ? "ReadOnly=True;" : "");
            return result;
        }
    }
}
