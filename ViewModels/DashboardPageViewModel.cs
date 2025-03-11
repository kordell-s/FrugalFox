using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System;
using CommunityToolkit.Mvvm.Messaging;
using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Models;
using FrugalFoxBudgetApp.Views;
using SQLite;

namespace FrugalFoxBudgetApp.ViewModels;

public class DashboardPageViewModel:INotifyPropertyChanged
{

    private readonly FrugalFoxDB Database;
    private readonly int userID; //For current user that's logged in
    
    private decimal totalBudget;
    private decimal monthlyBudget;
    private string budgetStatus;
    private ObservableCollection<TransactionViewModel> recentTransactions;
    private string greeting;
    private Budget currentBudget;
    
    public string Greeting
    {
        get => greeting;
        set
        {
            if (greeting != value)
            {
                greeting = value;
                OnPropertyChanged(nameof(Greeting));
            }
        }
    }

    public string RemainingBalance
    {
        get
        {
            if (currentBudget == null)
            
                return string.Empty;
            decimal remaining = currentBudget.MonthlyBudget - currentBudget.CurrentSpent;
            return $"Remaining Balance: {remaining:C}";

        }
    }
    
    public decimal MonthlyBudget
    {
        get => monthlyBudget;
        set
        {
            if (monthlyBudget != value)
            {
                monthlyBudget = value;
                OnPropertyChanged(nameof(MonthlyBudget));
                UpdateBudgetStatus();
            }
        }
    }
    
    public string BudgetStatus
    {
        get => budgetStatus;
        set
        {
            if (budgetStatus != value)
            {
                budgetStatus = value;
                OnPropertyChanged(nameof(BudgetStatus));
            }
        }
    }

    public ObservableCollection<TransactionViewModel>RecentTransactions{
        get => recentTransactions;
        set
        {
            if (recentTransactions != value)
            {
                recentTransactions = value;
                OnPropertyChanged(nameof(RecentTransactions));
            }
        }
        
    }
    private ObservableCollection<BudgetChart> chartData = new ObservableCollection<BudgetChart>();

    public ObservableCollection<BudgetChart> ChartData
    {
        get => chartData;
        set
        {
            if (chartData != value)
            {
                chartData = value;
                OnPropertyChanged(nameof(ChartData));
            }
        }
    }
    
    //List of Commands
    
    public ICommand AddTransactionCommand { get; private set; }
    public ICommand ViewReportsCommand { get; private set; }  
    public ICommand SetBudgetCommand { get; private set; }
    
    public ICommand ViewTransactionsCommand { get; private set; }
    //constructor

    public  DashboardPageViewModel()
    {
        Database = new FrugalFoxDB();

        if (App.CurrentUser != null)
        {


            userID = App.CurrentUser.UserId;
            var user = Database.GetUserByEmail(App.CurrentUser.Email);
            Greeting = user != null ? $"Hello, {user.FirstName}!" : "User not found.";
            LoadCurrentBudget();
            LoadRecentTransactions();
        }
        else
        {
            Greeting = "Please log in";
            MonthlyBudget = 0;
            BudgetStatus = "No active user";
            RecentTransactions = new ObservableCollection<TransactionViewModel>();
        }

        
        AddTransactionCommand = new Command(OnAddTransaction);
        ViewReportsCommand = new Command(OnViewReports);
        SetBudgetCommand = new Command(OnSetBudget);
        ViewTransactionsCommand = new Command(OnViewTransactions);
        


    }

    private void LoadCurrentBudget()
    {
        if (App.CurrentUser == null) return;
    
        var today = DateTime.Today;
        currentBudget = Database.GetCurrentBudget(userID);

        if (currentBudget != null)
        {
            // Update current spent from the sum of transactions
            currentBudget.CurrentSpent = Database.CalculateCurrentSpent(userID);
        
            // Now update the MonthlyBudget (if needed) and refresh status
            MonthlyBudget = currentBudget.MonthlyBudget;
            UpdateBudgetStatus();
            UpdateChartData();
            OnPropertyChanged(nameof(RemainingBalance));

        }
        else
        {
            MonthlyBudget = 0;
            BudgetStatus = "No active Budget. Tap 'Set Budget' to get started";
        }
    }
    
    private void UpdateChartData()
    {
        {
            if (currentBudget == null || currentBudget.MonthlyBudget <= 0)
                return;

            // Suppose you have loaded all transactions for the current budget period.
            // For example, you might query all transactions for the month:
            var startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var endDate = startDate.AddMonths(1).AddTicks(-1);
            var transactions = Database.Query<Transaction>(
                "SELECT * FROM Transactions WHERE UserId = ? AND Date BETWEEN ? AND ?",
                App.CurrentUser.UserId, startDate, endDate);

            // Group transactions by category name and sum the amounts.
            var groups = transactions.GroupBy(t => t.CategoryName)
                .Select(g => new BudgetChart()
                {
                    Category = g.Key,
                    Value = g.Sum(t => (double)t.Amount)
                }).ToList();

            // Calculate remaining budget.
            double totalBudget = (double)currentBudget.MonthlyBudget;
            double totalSpent = (double)currentBudget.CurrentSpent;
            double remaining = totalBudget - totalSpent;

            // If no remaining budget, show one full segment in orange.
            if (remaining <= 0)
            {
                groups.Clear();
                groups.Add(new BudgetChart { Category = "Spent", Value = totalBudget });
            }
            else
            {
                groups.Add(new BudgetChart { Category = "Remaining", Value = remaining });
            }

            // Update the collection.
            ChartData.Clear();
            foreach (var item in groups)
            {
                ChartData.Add(item);
            }
        }
    }
    private void UpdateBudgetStatus()
    {
        if (App.CurrentUser == null) return;
        if (MonthlyBudget <= 0)
        {
            BudgetStatus = "No active Budget. Tap 'Set Budget to get started";
            return;
        }

        if (currentBudget != null)
        {
            decimal spent = currentBudget.CurrentSpent;
            decimal total = currentBudget.MonthlyBudget;
            decimal remaining = total - spent;
            BudgetStatus = $"Spent: {spent:C}  |  Remaining: {remaining:C}";
        }
        else
        {
            BudgetStatus = $"Budget: {MonthlyBudget:C}";
        }
    }
    
    public void LoadRecentTransactions()
       {
           var query = @" 
SELECT t.*, c.Name as CategoryName, c.Icon as CategoryIcon
    FROM Transactions t
    LEFT JOIN Categories c ON t.CategoryId = c.CategoryId
    WHERE t.UserId = ?
    ORDER BY t.Date DESC
    LIMIT 5";
       
           var transactions = Database.Query<Transaction>(query, userID);
           System.Diagnostics.Debug.WriteLine($"Join query (with LEFT JOIN) returned {transactions.Count} transactions.");
       
           var transactionViewModels = transactions.Select(t => new TransactionViewModel {
               TransactionId = t.TransactionId,
               Title = t.Title, 
               Amount = t.Amount,
               Date = t.Date,
               CategoryName = t.CategoryName, 
               CategoryIcon = t.CategoryIcon 
           }).ToList();
       
           RecentTransactions = new ObservableCollection<TransactionViewModel>(transactionViewModels);
           System.Diagnostics.Debug.WriteLine($"LoadRecentTransactions: RecentTransactions count is {RecentTransactions.Count}.");
       }
   

    private async void OnAddTransaction()
    {
        try
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddTransactionPage());
        }catch (Exception ex)

        {
            System.Diagnostics.Debug.WriteLine("Error", ex.Message, "Ok");
        }
    }

    private async void OnViewReports()
    {
        try
        { 
            await Application.Current.MainPage.Navigation.PushAsync(new ViewReportsPage());
        }catch (Exception ex)

        {
            System.Diagnostics.Debug.WriteLine("Error", ex.Message, "Ok");
        }
    }

    private async void OnSetBudget()
    {
        try
         {
           await Application.Current.MainPage.Navigation.PushAsync(new SetBudgetPage());
        }catch (Exception ex)

         {
             System.Diagnostics.Debug.WriteLine("Error", ex.Message, "Ok"); }
    }

    public async void OnViewTransactions()
    {
        try
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ViewTransactionsPage());
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine("Error", e.Message, "Ok"); 

        }
    }

    
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}