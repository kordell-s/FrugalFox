namespace FrugalFoxBudgetApp.Models;

public class Transaction
{
    public int TransactionId { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    
    
    public User User { get; set; } = null!;
    public Category Category { get; set; } = null!;
}