using System.Collections.ObjectModel;
using System.ComponentModel;
using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Models;

namespace FrugalFoxBudgetApp.ViewModels;

public class ViewReportsViewModel : INotifyPropertyChanged
{
    
    private readonly FrugalFoxDB Database;

    public ViewReportsViewModel()
    {
        Database = new FrugalFoxDB();
        LoadReports(); 
    }

    private ObservableCollection<ReportItem> reportItems;

    public ObservableCollection<ReportItem> ReportItems
    {
        get => reportItems;
        set
        {
            if (reportItems != value)
            {
                reportItems = value;
                OnPropertyChanged(nameof(ReportItems));
            }
        }
    }

    private void LoadReports()
    {
        var transactions = Database.Query<Transaction>("SELECT * FROM Transactions WHERE UserId = ?", App.CurrentUser.UserId);
        var items = transactions.Select(t => new ReportItem

        {
            Title = $"${t.Amount:F2}",
            Detail = t.Date.ToString("dd/MM/yyyy"),
        }).ToList();
        
        ReportItems = new ObservableCollection<ReportItem>(items);
    }
    
    
    
    
    
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}