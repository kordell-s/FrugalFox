using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Models;
using FrugalFoxBudgetApp.Views;
using Microsoft.Maui.Controls;

namespace FrugalFoxBudgetApp.ViewModels
{
    public class TransactionDetailsViewModel : INotifyPropertyChanged
    {
        private readonly FrugalFoxDB Database;

        private Transaction _transaction;
        
        //Properties we're binding
        
        public string Title => _transaction?.Title ?? string.Empty;
        public decimal Amount => _transaction?.Amount ?? 0;
        public string CategoryName => _transaction?.CategoryName ?? string.Empty;
        public DateTime Date => _transaction?.Date ?? DateTime.Now;

        public ICommand EditTransactionCommand { get; private set; }
        public ICommand DeleteTransactionCommand { get; private set; }

        public TransactionDetailsViewModel()
        {
            _transaction = new Transaction();
            InitializeCommands();
        }

        public TransactionDetailsViewModel(Transaction transaction)
        {
            _transaction = transaction;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            EditTransactionCommand = new Command(OnEditTransaction);
            DeleteTransactionCommand = new Command(OnDeleteTransaction);
        }
        public void Initialize(Transaction transaction)
        {
            _transaction = Transaction;
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Amount));
            OnPropertyChanged(nameof(CategoryName));
            OnPropertyChanged(nameof(Date));
        }

        public Transaction Transaction
        {
            get => _transaction;
            set
            {
                if (_transaction != value)
                {
                    _transaction = value;
                    OnPropertyChanged();
                }
            }
        }

        private async void OnEditTransaction()
        {
            if (_transaction != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new EditTransactionPage(_transaction));
            }
        }
        
        private async void OnDeleteTransaction()
        {
            if (_transaction != null)
            {
                bool confirm = await Application.Current.MainPage.DisplayAlert(
                    "Confirm Delete", 
                    $"Are you sure you want to delete '{_transaction.Title}'?", 
                    "Yes", "No");
                
                if (confirm)
                {
                    // Delete from database
                    var db = new FrugalFoxDB();
                    Database.DeleteTransaction(_transaction);
                
                    // Go back to previous page
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}