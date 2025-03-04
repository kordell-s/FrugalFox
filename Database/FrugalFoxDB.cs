using FrugalFoxBudgetApp.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;

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
        SeedOrUpdateDefaultCategories();
    }
    
    private void SeedOrUpdateDefaultCategories()
    {
        // Retrieve existing categories from the database.
        var existingCategories = DatabaseConnection.Table<Category>().ToList();

        // Define default categories.
        var defaultCategories = new List<Category>
        {
            new Category { Name = "Food", Icon = "food.png", Color = "#F39C12" },
            new Category { Name = "Transport", Icon = "transportation.png", Color = "#27AE60" },
            new Category { Name = "Entertainment", Icon = "entertainment.png", Color = "#8E44AD" },
            new Category { Name = "Utilities", Icon = "utilities.png", Color = "#2980B9" }
        };

        // For each default category, insert if missing or update if it exists.
        foreach (var defaultCat in defaultCategories)
        {
            if (!existingCategories.Any(c => c.Name == defaultCat.Name))
            {
                DatabaseConnection.Insert(defaultCat);
            }
            else
            {
                var existingCat = existingCategories.First(c => c.Name == defaultCat.Name);
                existingCat.Icon = defaultCat.Icon;
                existingCat.Color = defaultCat.Color;
                DatabaseConnection.Update(existingCat);
            }
        }
        
        
    }
    
    
    //to get updated amount spent each time
    
    public decimal CalculateCurrentSpent(int userId)
    {
        var transactions = DatabaseConnection.Table<Transaction>().Where(t => t.UserId == userId).ToList();
        return transactions.Sum(t => t.Amount);
    }
    
    
    //Define Utility Functions
    public List<T> Query<T>(string query, params object[] args) where T : new()
    {
        return DatabaseConnection.Query<T>(query, args);
    }
        
    //User functions
    public int CreateUser(User newUser)
    {
        var insertStatus = DatabaseConnection.Insert(newUser);
        return insertStatus;
    }

    public int UpdateUser(User newUser)
    {
        var updateStatus = DatabaseConnection.Update(newUser);
        return updateStatus;
    }

    public int DeleteUser(User newUser)
    {
        var deleteStatus = DatabaseConnection.Delete(newUser);
        return deleteStatus;
    }

    public User GetUserByEmail(string email)
    {
        return DatabaseConnection.Table<User>().FirstOrDefault(u => u.Email == email);
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
        var today = DateTime.Today;
        return DatabaseConnection.Table<Budget>().Where(b => b.UserId == userId && b.StartDate <= today && b.EndDate >= today).FirstOrDefault();
        
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

    public Category GetCategoryById(int categoryId)
    {
        var select = DatabaseConnection.Table<Category>().Where(c => c.CategoryId == categoryId);
        return select.FirstOrDefault();
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

    public int DeleteTransaction(Transaction transaction)
    {
        var deleteStatus = DatabaseConnection.Delete(transaction);
        return deleteStatus;
    }
}