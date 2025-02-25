using FrugalFoxBudgetApp.Models;
using SQLite;

namespace FrugalFoxBudgetApp.Database;

public class FrugalFoxDB
{
    public string CurrentState;
    static SQLiteConnection DatabaseConnection;

    public FrugalFoxDB()
    {
        try
        {
            //make the connection
            DatabaseConnection = new SQLiteConnection(DBConnection.DatabasePath, DBConnection.flags);

            //define the table
            DatabaseConnection.CreateTable<User>();
            Console.WriteLine("Table created successfully");

            //set the status of the DB
            CurrentState = "Initialized";

        }
        catch(SQLiteException e)
        {
            CurrentState = e.Message;
        }
        
        
    }
    //Define Utility Functions
        
    //User functions
        
    //CreateUser
    public int CreateUser(User newUser)
    {
        var insertStatus = DatabaseConnection.Insert(newUser);
        return insertStatus;
    }
}