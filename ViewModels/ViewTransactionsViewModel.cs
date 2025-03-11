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
        LoadCategories();
        SearchCommand = new Command(OnSearch);
        RefreshCommand = new Command(RefreshTransactions);
        DateRangeChangedCommand = new Command <object>(OnDateRangeChanged);
        TransactionSelectedCommand = new Command<IList>(OnTransactionSelected);
        AddTransactionCommand = new Command(OnAddTransaction);
        ViewTransactionDetailsCommand = new Command<object>(OnViewTransactionDetails);

    }
    
    public ObservableCollection<TransactionViewModel> Transactions { get; set; } = new ObservableCollection<TransactionViewModel>();
    public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
    private ObservableCollection<string> dateRangeOptions = new ObservableCollection<string> { "Today", "This Week", "This Month", "This Year" };
    public ObservableCollection<string> DateRangeOptions
    {
        get => dateRangeOptions;
        set { dateRangeOptions = value; OnPropertyChanged(); }
    }

  
    public Category SelectedCategory
    {
        get => selectedCategory;
        set
        {
            if (selectedCategory != value)
            {
                selectedCategory = value;
                OnPropertyChanged();
                //To automatically refresh transactions when a new category is selected.
                LoadTransactions();
            }
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
    public ICommand ViewTransactionDetailsCommand { get; }
    
    private string selectedDateRange = "This Month";
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
    
    private bool isRefreshing;
    public bool IsRefreshing
    {
        get => isRefreshing;
        set
        {
            isRefreshing = value;
            OnPropertyChanged(nameof(IsRefreshing));
        }
    }

    private Category selectedCategory;

    private TransactionViewModel selectedTransaction;
    public TransactionViewModel SelectedTransaction
    {
        get => selectedTransaction;
        set
        {
            if (selectedTransaction != value)
            {
                selectedTransaction = value;
                OnPropertyChanged();
            
                if (selectedTransaction != null)
                {
                    OnViewTransactionDetails(selectedTransaction);
                }
            }
        }
    }


    
    
    private void RefreshTransactions()
    {
        IsRefreshing = true;
        LoadTransactions(); 
        IsRefreshing = false;
    }
    private async void OnViewTransactionDetails(object obj)
    {
        if (obj is TransactionViewModel transactionVM)
        {
            // Get the full Transaction from the database if needed
            var transaction = Database.Query<Transaction>($"SELECT * FROM Transactions WHERE TransactionId = {transactionVM.TransactionId}").FirstOrDefault();
            if (transaction != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new TransactionsDetailsPage(transaction));
            }
        }
    }

    private void OnDateRangeChanged(object parameter)
    {
        LoadTransactions();
    }
    
    private void LoadCategories()
    {
        // Query the database for all categories.
        var categoryList = Database.Query<Category>("SELECT * FROM Categories");
        Categories = new ObservableCollection<Category>(categoryList);
        OnPropertyChanged(nameof(Categories));
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

        if (SelectedCategory != null)
        {
            results = results.Where(t => t.CategoryName.Equals(SelectedCategory.Name, StringComparison.OrdinalIgnoreCase)).ToList();
           
        }
        
        // Filter by date range if a selection is made.
        if (!string.IsNullOrEmpty(SelectedDateRange))
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;

            switch (SelectedDateRange)
            {
                case "Today":
                    startDate = DateTime.Today;
                    endDate = DateTime.Today.AddDays(1).AddTicks(-1);
                    break;
                case "This Week":
                    // Assuming Monday is the start of the week.
                    int diff = (7 + (DateTime.Today.DayOfWeek - DayOfWeek.Monday)) % 7;
                    startDate = DateTime.Today.AddDays(-diff);
                    endDate = startDate.AddDays(7).AddTicks(-1);
                    break;
                case "This Month":
                    startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    endDate = startDate.AddMonths(1).AddTicks(-1);
                    break;
                case "This Year":
                    startDate = new DateTime(DateTime.Today.Year, 1, 1);
                    endDate = startDate.AddYears(1).AddTicks(-1);
                    break;
            }
            results = results.Where(t => t.Date >= startDate && t.Date <= endDate).ToList();
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
    
    
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}