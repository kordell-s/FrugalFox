namespace FrugalFoxBudgetApp.ViewModels;

public class TransactionViewModel
{
    public int TransactionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    
    public string FormattedDate => Date.ToString("MMM dd, yyyy");
    
    public string CategoryName { get; set; }
    
    public string CategoryIcon { get; set; }
}
