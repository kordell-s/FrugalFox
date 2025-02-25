using SQLite;

namespace FrugalFoxBudgetApp.Database;

public class DBConnection
{
    //defining the database name
    public const string DatabaseName = "FrugalFox.db";
    
    //db initialisations
    public const SQLite.SQLiteOpenFlags flags = SQLite.SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create;

    public static string DatabasePath
    {
        get
        {
            //get the base path
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
           
            //Appending the base and filename base/filename
            var dbFilePath = Path.Combine(basePath, DatabaseName);
            
            //returning the full db file path
            return dbFilePath;
        }
    }
}