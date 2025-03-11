using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Models;
using Microsoft.Maui.Controls;

namespace FrugalFoxBudgetApp.ViewModels
{
    public class EditTransactionViewModel : INotifyPropertyChanged
    {
        private readonly FrugalFoxDB Database;
        private Transaction transaction;

        public EditTransactionViewModel(Transaction existingTransaction)
        {
            Database = new FrugalFoxDB();
            Transaction = existingTransaction;

            SaveCommand = new Command(OnSaveTransaction);
            DeleteCommand = new Command(OnDeleteTransaction);
        }

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

        public string Title
        {
            get => Transaction.Title;
            set
            {
                if (Transaction.Title != value)
                {
                    Transaction.Title = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Amount
        {
            get => Transaction.Amount;
            set
            {
                if (Transaction.Amount != value)
                {
                    Transaction.Amount = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedCategory
        {
            get => Transaction.CategoryName;
            set
            {
                if (Transaction.CategoryName != value)
                {
                    Transaction.CategoryName = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Date
        {
            get => Transaction.Date;
            set
            {
                if (Transaction.Date != value)
                {
                    Transaction.Date = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> Categories { get; set; } = new ObservableCollection<string>
        {
            "Food", "Transport", "Entertainment", "Utilities", "Other"
        };

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        private async void OnSaveTransaction()
        {
            Database.UpdateTransaction(Transaction);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void OnDeleteTransaction()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirm Delete", "Are you sure you want to delete this transaction?", "Yes", "No");

            if (confirm)
            {
                Database.DeleteTransaction(Transaction.TransactionId);
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
