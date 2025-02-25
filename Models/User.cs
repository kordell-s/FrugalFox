using System.Transactions;
using SQLite;

namespace FrugalFoxBudgetApp.Models;


[SQLite.Table("Users")]
public class User
{
    [PrimaryKey]
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    public List<Budget> Budgets { get; set; } = new List<Budget>();
    
    // Model methods for business logic
    public bool IsValidForRegistration(string confirmPassword, out string errorMessage)
    {
        if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || 
            string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
        {
            errorMessage = "Please fill all fields";
            return false;
        }
        
        if (Password != confirmPassword)
        {
            errorMessage = "Passwords do not match";
            return false;
        }
        
        errorMessage = string.Empty;
        return true;
    }
    
    // This would handle the actual account creation in a real app
    public bool CreateAccount()
    {
        // In a real app, this would save to a database
        // For now, we'll just simulate success
        return true;
    }
}