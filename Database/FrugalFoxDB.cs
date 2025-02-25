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
            
            //Initializing the Tables
            DatabaseConnection.CreateTable<User>();
            DatabaseConnection.CreateTable<Budget>();
            DatabaseConnection.CreateTable<Category>();
            DatabaseConnection.CreateTable<Transaction>();

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
    public int CreateUser(User newUser)
    {
        var insertStatus = DatabaseConnection.Insert(newUser);
        return insertStatus;
    }

    public int updateUser(User newUser)
    {
        var updateStatus = DatabaseConnection.Update(newUser);
        return updateStatus;
    }

    public int deleteUser(User newUser)
    {
        var deleteStatus = DatabaseConnection.Delete(newUser);
        return deleteStatus;
    }
    
    //Budget Functions
    public int CreateBudget(Budget budget)
    {
        var insertStatus = DatabaseConnection.Insert(budget);
        return insertStatus;
    }

    public Budget GetBudget(int budgetId)
    {
        var select = DatabaseConnection.Table<Budget>().Where(b => b.BudgetId == budgetId);
        return select.FirstOrDefault();
    }

    public Budget GetCurrentBudget(int userId)
    {
        var select = DatabaseConnection.Table<Budget>().Where(b => b.UserId == userId);
        return select.FirstOrDefault();
        
    }
    
    public int UpdateBudget(Budget budget)
    {
        var updateStatus = DatabaseConnection.Update(budget);
        return updateStatus;
    }
    
    public int DeleteBudget(int budgetId)
    {
        var deleteStatus = DatabaseConnection.Delete(budgetId);
        return deleteStatus;
    }
    
    
    //Category Functions

    public int AddCategory(Category category)
    {
        var insertStatus = DatabaseConnection.Insert(category);
        return insertStatus;
    }

    public int UpdateCategory(Category category)
    {
        var updateStatus = DatabaseConnection.Update(category);
        return updateStatus;
    }

    public int DeleteCategory(int categoryId)
    {
        var deleteStatus = DatabaseConnection.Delete(categoryId);
        return deleteStatus;
    }
    
    //Transaction Functions

    public int AddTransaction(Transaction newTransaction)
    {
        var insertStatus = DatabaseConnection.Insert(newTransaction);
        return insertStatus;
    }

    public int UpdateTransaction(Transaction transaction)
    {
        var updateStatus = DatabaseConnection.Update(transaction);
        return updateStatus;
    }

    public int deleteTransaction(Transaction transaction)
    {
        var deleteStatus = DatabaseConnection.Delete(transaction);
        return deleteStatus;
    }
}