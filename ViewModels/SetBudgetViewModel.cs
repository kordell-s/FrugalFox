using System.ComponentModel;
using System.Windows.Input;
using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Models;

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
        if (MonthlyBudget <= 0)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "You must select a monthly budget.", "OK");
            return;
        }

        var budget = new Budget
        {
            UserId = App.CurrentUser.UserId,
            MonthlyBudget = MonthlyBudget,
            StartDate = StartDate,
            EndDate = EndDate,
            CurrentSpent = 0
        };
        
        int result = Database.CreateBudget(budget);
        if (result > 0)
        {
            await Application.Current.MainPage.DisplayAlert("Success", "Budget added.", "OK");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Budget added.", "OK");
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
