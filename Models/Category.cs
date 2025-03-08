using SQLite;

namespace FrugalFoxBudgetApp.Models;


[SQLite.Table("Categories")]
public class Category
{
   [PrimaryKey, AutoIncrement]
    public int CategoryId { get; set; }
    public string Name { get; set; }= string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    
    [Ignore]
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
}