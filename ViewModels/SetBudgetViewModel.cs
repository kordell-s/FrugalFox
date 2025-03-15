using System.ComponentModel;
using System.Windows.Input;
using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Models;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using FrugalFoxBudgetApp.Messages;

namespace FrugalFoxBudgetApp.ViewModels;

public class SetBudgetViewModel : INotifyPropertyChanged
{
    
    private readonly FrugalFoxDB Database;

    public SetBudgetViewModel()
    {
        Database = new FrugalFoxDB();
        SaveBudgetCommand = new Command(OnSaveBudget);
        StartDate = DateTime.Today;
        EndDate = DateTime.Today.AddMonths(1);
    }

    private decimal monthlyBudget;

    public decimal MonthlyBudget
    {
        get => monthlyBudget;
        set
        {
            if (monthlyBudget != value)
            {
                monthlyBudget = value;
                OnPropertyChanged(nameof(MonthlyBudget));
            }
        }
    }
    private DateTime startDate;

    public DateTime StartDate
    {
        get => startDate;
        set
        {
            if (startDate != value)
            {
                startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
    }
    
    private DateTime endDate;

    public DateTime EndDate
    {
        get => endDate;
        set
        {
            if (endDate != value)
            {
                endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
    }
    
    public ICommand SaveBudgetCommand { get; }
    
   private async void OnSaveBudget()
{
    try
    {
        if (MonthlyBudget <= 0)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You must select a monthly budget.", "OK");
            return;
        }

        Console.WriteLine($"[DEBUG] Checking existing budget for User ID: {App.CurrentUser.UserId}");

        var existingBudget = Database.GetCurrentBudget(App.CurrentUser.UserId);

        if (existingBudget != null)
        {
            // Update existing budget
            Console.WriteLine("[DEBUG] Updating existing budget...");

            existingBudget.MonthlyBudget = MonthlyBudget;
            existingBudget.StartDate = StartDate;
            existingBudget.EndDate = EndDate;

            int result = Database.UpdateBudget(existingBudget);

            if (result > 0)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Budget updated.", "OK");
                WeakReferenceMessenger.Default.Send(new BudgetUpdatedMessage(true));

            }
            else
            {
                throw new Exception("Database update operation failed.");
            }
        }
        else
        {
            // Create new budget
            Console.WriteLine("[DEBUG] Creating new budget...");

            var newBudget = new Budget
            {
                UserId = App.CurrentUser.UserId,
                MonthlyBudget = MonthlyBudget,
                StartDate = StartDate,
                EndDate = EndDate,
                CurrentSpent = 0
            };

            int result = Database.CreateBudget(newBudget);

            if (result > 0)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Budget added.", "OK");
                WeakReferenceMessenger.Default.Send(new BudgetUpdatedMessage(true));

            }
            else
            {
                throw new Exception("Database insert operation failed.");
            }
        }
        
        Console.WriteLine("[DEBUG] Budget saved successfully. Navigating back...");
        await Application.Current.MainPage.Navigation.PopAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] Exception occurred: {ex.Message}");
        await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
    }
}
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
