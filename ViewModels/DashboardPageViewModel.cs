using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System;
using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Models;
using SQLite;

namespace FrugalFoxBudgetApp.ViewModels;

public class DashboardPageViewModel:INotifyPropertyChanged
{

    private readonly FrugalFoxDB Database;
    private readonly int userID; //For current user that's logged in
    
    
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
                OnPropertyChanged(nameof(MonthlyBudget));
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
    
    //List of Commands
    
    public ICommand AddTransactionCommand { get; private set; }
    public ICommand ViewReportsCommand { get; private set; }  
    public ICommand SetBudgetCommand { get; private set; }
    
    //constructor

    public DashboardPageViewModel()
    {
        Database = new FrugalFoxDB();
        userID = userID;

        if (App.CurrentUser != null)
        {
            // Retrieve the full user information using GetUserByEmail method.
            var user = Database.GetUserByEmail(App.CurrentUser.Email);
            if (user != null)
            {
                Greeting = $"Hello, {user.FirstName}!";
            }
            else
            {
                Greeting = "User not found.";
            }
        }
        else
        {
            Greeting = "Please Login";
            return;
        }


        LoadCurrentBudget();
        LoadRecentTransactions();

        AddTransactionCommand = new Command(OnAddTransaction);
        ViewReportsCommand = new Command(OnViewReports);
        SetBudgetCommand = new Command(OnSetBudget);

    }

    private void LoadCurrentBudget()
    {
        var today = DateTime.Today;
        currentBudget = Database.GetCurrentBudget(userID);

        if (currentBudget != null)
        {
            MonthlyBudget = currentBudget.MonthlyBudget;
            UpdateBudgetStatus();
        }
        else
        {
            MonthlyBudget = 0;
            BudgetStatus = "No active Budget. Tap 'Set Budget to get started";
        }
    }

    private void UpdateBudgetStatus()
    {
        if (MonthlyBudget <= 0)
        {
            BudgetStatus = "No active Budget. Tap 'Set Budget to get started";
            return;
        }

        if (currentBudget != null)
        {
            decimal balance = currentBudget.MonthlyBudget - currentBudget.CurrentSpent;
            BudgetStatus = $"Spent ${currentBudget.CurrentSpent} of {currentBudget.MonthlyBudget:F2}";
        }
        else
        {
            BudgetStatus = "$Budget: ${MonthlyBudget:F2}";
        }
    }

    private void LoadRecentTransactions()
    {
        var query = @"
        SELECT t.*, c.Name as CategoryName
        FROM Transactions t
        JOIN Categories c ON t.CategoryId = c.CategoryId
        WHERE t.UserId = ?
        ORDER BY t.Date DESC
        LIMIT 5";
    
        // Getting the transactions from the database
        var transactions = Database.Query<Transaction>(query, userID);
    
        // Mapping each transaction to a TransactionViewModel
        var transactionViewModels = transactions.Select(t => new TransactionViewModel {
            TransactionId = t.TransactionId,
            Amount = t.Amount,
            Date = t.Date,
            CategoryName = t.CategoryName
        }).ToList();
    
        RecentTransactions = new ObservableCollection<TransactionViewModel>((IEnumerable<TransactionViewModel>)transactionViewModels);
    }

    private async void OnAddTransaction()
    {
        try
        {
            await Shell.Current.GoToAsync("AddTransactionPage");
        }catch (Exception ex)

        {
            System.Diagnostics.Debug.WriteLine("Error", ex.Message, "Ok");
        }
    }

    private async void OnViewReports()
    {
        try
        { 
            await Shell.Current.GoToAsync("AddTransactionPage");
        }catch (Exception ex)

        {
            System.Diagnostics.Debug.WriteLine("Error", ex.Message, "Ok");
        }
    }

    private async void OnSetBudget()
    {
        try
         {
           await Shell.Current.GoToAsync("AddTransactionPage");
        }catch (Exception ex)

         {
             System.Diagnostics.Debug.WriteLine("Error", ex.Message, "Ok"); }
    }

    
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}