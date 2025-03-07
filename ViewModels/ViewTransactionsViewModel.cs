using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Models;
using FrugalFoxBudgetApp.Views;

namespace FrugalFoxBudgetApp.ViewModels;

public class ViewTransactionsViewModel: INotifyPropertyChanged
{
    private readonly FrugalFoxDB Database;
    private string searchText;

    public ViewTransactionsViewModel()
    {
        Database = new FrugalFoxDB();
        LoadTransactions();
        SearchCommand = new Command(OnSearch);
        RefreshCommand = new Command(OnRefresh);
        DateRangeChangedCommand = new Command <object>(OnDateRangeChanged);
        TransactionSelectedCommand = new Command<IList>(OnTransactionSelected);
        AddTransactionCommand = new Command(OnAddTransaction);

    }
    
    public ObservableCollection<TransactionViewModel> Transactions { get; set; } = new ObservableCollection<TransactionViewModel>();
    private ObservableCollection<string> dateRangeOptions = new ObservableCollection<string> { "Today", "This Week", "This Month", "This Year" };
    public ObservableCollection<string> DateRangeOptions
    {
        get => dateRangeOptions;
        set { dateRangeOptions = value; OnPropertyChanged(); }
    }

    private string selectedDateRange;
    public string SelectedDateRange
    {
        get => selectedDateRange;
        set 
        { 
            selectedDateRange = value; 
            OnPropertyChanged();
            LoadTransactions();
        }
    }

    public string SearchText
    {
        get => searchText;
        set
        {
            searchText = value;
            OnPropertyChanged();
        }
    }
    
    public ICommand SearchCommand { get; }
    public ICommand RefreshCommand { get; }
    
    public ICommand DateRangeChangedCommand { get; }
    public ICommand TransactionSelectedCommand { get; }
    public ICommand AddTransactionCommand { get; }

    private void OnDateRangeChanged(object parameter)
    {
        LoadTransactions();
    }
    
    private async void OnAddTransaction()
    {
        // Option A: Using Shell navigation
        //await Shell.Current.GoToAsync("//AddTransactionPage");

        // Option B: Using NavigationPage (uncomment if you prefer this method)
        await Application.Current.MainPage.Navigation.PushAsync(new AddTransactionPage());
    }

    private void OnTransactionSelected(IList t)
    {
        if (t != null && t.Count > 0 && t[0] is TransactionViewModel transaction)
        {
            System.Diagnostics.Debug.WriteLine("Selected Transaction: " + transaction.Title);

        }
    }

    public void LoadTransactions()
    {
        var query = 
            @"
                SELECT t.*, c.Name as CategoryName, c.Icon as CategoryIcon
                FROM Transactions t
                LEFT JOIN Categories c ON t.CategoryId = c.CategoryId
                WHERE t.UserId = ?
                ORDER BY t.Date DESC";
        
        var results = Database.Query<Transaction>(query, App.CurrentUser.UserId);
        
        //filter using SearchText in-memory if not empty
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            results = results.Where(t => t.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                         t.CategoryName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        
        var tVMs = results.Select(t => new TransactionViewModel
        {
            TransactionId = t.TransactionId,
            Title = t.Title,
            Amount = t.Amount,
            Date = t.Date,
            CategoryName = t.CategoryName,
            CategoryIcon = t.CategoryIcon
        }).ToList();
        
        Transactions = new ObservableCollection<TransactionViewModel>(tVMs);
        OnPropertyChanged(nameof(Transactions));
        
    }

    private void OnSearch()
    {
        LoadTransactions();
    }

    private void OnRefresh()
    {
        LoadTransactions();
    }
    
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}