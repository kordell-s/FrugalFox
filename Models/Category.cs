namespace FrugalFoxBudgetApp.Models;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public string Color { get; set; }
    
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
}