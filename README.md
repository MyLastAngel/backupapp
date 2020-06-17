### Создает SQLLite DB

```sql
var sql = "CREATE TABLE IF NOT EXISTS DATA (" +
    "Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
    "Url TEXT," +
    "Description TEXT," +
    "Icon TEXT); ";

sql = "CREATE TABLE IF NOT EXISTS DATA_RECORD (" +
    "Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
    "DataId INTEGER, " +
    "Name TEXT, " +
    "Value TEXT, " +
    "Description TEXT, " +
    "FOREIGN KEY(DataId) REFERENCES DATA(Id)" +
    ");";
```
    
### БД хранится в папке директории 

```c#
string.Format(@"{0}\DB", Environment.CurrentDirectory);
```

### Защита заключается в том что пароль для шифра БД известен только создавшему БД

```c#
static string GetConnectionString(string pswd, bool isReadOnly)
{
    var path = string.Format("{0}\\cache.db", CommonManager.GetDirectory(DirectoryMode.DB));

    if (!File.Exists(path))
        isReadOnly = false;

    var result = string.Format("Data Source='{0}'; Version=3; Password={1}; {2}", path, pswd, isReadOnly ? "ReadOnly=True;" : "");
    return result;
}
```
