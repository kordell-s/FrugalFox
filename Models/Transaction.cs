using SQLite;

namespace FrugalFoxBudgetApp.Models;


[SQLite.Table("Transactions")]
public class Transaction
{
    [PrimaryKey, AutoIncrement]
    public int TransactionId { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string CategoryName { get; set; } = null!;
    
    [Ignore]
    public User User { get; set; } = null!;
    [Ignore]
    public Category Category { get; set; } = null!;
    public string CategoryIcon { get; set; } = null!;
  
}
