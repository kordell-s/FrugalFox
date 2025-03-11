using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Models;
using FrugalFoxBudgetApp.Views;

namespace FrugalFoxBudgetApp.ViewModels;

public class TransactionDetailsViewModel : INotifyPropertyChanged
{
    private readonly FrugalFoxDB Database;
    public ICommand EditTransactionCommand { get; }

    public TransactionDetailsViewModel(Transaction transaction)
    {
        Database = new FrugalFoxDB();
        Transaction = transaction;
        EditTransactionCommand = new Command(OnEditTransaction);
    }


    private Transaction transaction;

    public Transaction Transaction
    {
        get => transaction;
        set
        {
            if (transaction != value)
            {
                transaction = value;
                OnPropertyChanged();
            }
        }
    }

    private async void OnEditTransaction()
    {
        await Application.Current.MainPage.Navigation.PushAsync(new EditTransactionPage(Transaction));
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}