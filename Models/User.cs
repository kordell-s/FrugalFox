using System.ComponentModel.DataAnnotations;
using System.Transactions;
using SQLite;

namespace FrugalFoxBudgetApp.Models;


[SQLite.Table("Users")]
public class User
{
    [PrimaryKey]
    public int UserId { get; set; }
  
    [Required, SQLite.MaxLength(100)]
    public string Email { get; set; }
    public string Password { get; set; }
    
    [Required, SQLite.MaxLength(50)]
    public string FirstName { get; set; }
    
    [Required, SQLite.MaxLength(50)]
    public string LastName { get; set; }
    
    //Password we're storing
    [Required]
    public string PasswordHash { get; set; }
    public DateTime CreatedDate { get; set; }
    
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    public List<Budget> Budgets { get; set; } = new List<Budget>();
    
    public string FullName => $"{FirstName} {LastName}";
    
}