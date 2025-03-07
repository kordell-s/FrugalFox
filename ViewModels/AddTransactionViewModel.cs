using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Models;

namespace FrugalFoxBudgetApp.ViewModels;

public class AddTransactionViewModel : INotifyPropertyChanged
{
    private readonly FrugalFoxDB Database;

    public AddTransactionViewModel()
    {
        Database = new FrugalFoxDB();
        SaveTransactionCommand = new Command(OnSaveTransaction);
        TransactionDate = DateTime.Today;
        LoadCategories();
    }

    private string title;

    public string Title
    {
       get => title;
       set
       {
           if (title != value)
           {
               title = value;
               OnPropertyChanged(nameof(Title));
           }
       }
    }

    private decimal amount;

    public decimal Amount
    {
        get => amount;
        set
        {
            if (amount != value)
            {
                amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }
    }
    
    private DateTime transactionDate;

    public DateTime TransactionDate
    {
        get => transactionDate;
        set
        {
            if (transactionDate != value)
            {
                transactionDate = value;
                OnPropertyChanged(nameof(TransactionDate));
            }
        }
    }
    
    private int categoryId;

    public int CategoryId
    {
        get => categoryId;
        set
        {
            if (categoryId != value)
            {
                categoryId = value;
                OnPropertyChanged(nameof(CategoryId));
            }
        } 
    }
    
    private ObservableCollection<Category> categories;

    public ObservableCollection<Category> Categories
    {
        get => categories;
        set
        {
            if (categories != value)
            {
                categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }
    }
    
    private Category selectedCategory;

    public Category SelectedCategory
    {
        get => selectedCategory;
        set
        {
            if (selectedCategory != value)
            {
                selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }
    }
    public ICommand SaveTransactionCommand { get; }

    private async void OnSaveTransaction()
    {
        if (Amount <= 0)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid amount.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(Title))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid title.", "OK");
            return;
        }
        if (SelectedCategory == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Please select a category.", "OK");
            return;
        }

        var transaction = new Transaction
        {
            UserId = App.CurrentUser.UserId,
            CategoryId = SelectedCategory.CategoryId,
            Title = Title,
            Amount = Amount,
            Date = TransactionDate,
            CategoryName = SelectedCategory.Name
        };

        int result = Database.AddTransaction(transaction);
        if (result > 0)
        {
            await Application.Current.MainPage.DisplayAlert("Success", "Transaction added", "OK");
            // Navigate explicitly to the DashboardPage.
            await Application.Current.MainPage.Navigation.PushAsync(new DashboardPage());
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Transaction could not be added", "OK");
        }
    }

    //loading categories from the db
    private void LoadCategories()
    {
        var categoryList = Database.Query<Category>("SELECT * FROM Categories");
        Categories = new ObservableCollection<Category>(categoryList);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}