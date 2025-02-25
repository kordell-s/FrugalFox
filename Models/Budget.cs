namespace FrugalFoxBudgetApp.Models;

[SQLite.Table("Budgets")]
public class Budget
{
    public int BudgetId { get; set; }
    public int UserId { get; set; }
    public decimal MonthlyBudget { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal CurrentSpent { get; set; }
    public User User { get; set; } = null!;
}