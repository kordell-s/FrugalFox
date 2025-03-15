using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Models;
using FrugalFoxBudgetApp.Views;

namespace FrugalFoxBudgetApp.ViewModels
{
    public class EditTransactionViewModel : INotifyPropertyChanged
    {
        private readonly FrugalFoxDB Database;
        private Transaction _transaction;

        // Default constructor for XAML previewer
        public EditTransactionViewModel()
        {
            // Initialize with empty transaction to prevent null reference exceptions
            _transaction = new Transaction();
            Database = new FrugalFoxDB();
            LoadCategories();
        }

        public EditTransactionViewModel(Transaction existingTransaction)
        {
            Database = new FrugalFoxDB();
            _transaction = existingTransaction;
            
            // Load categories first, so SelectedCategory can be set
            LoadCategories();
            
            // Set selected category after categories are loaded
            SelectedCategory = Categories.FirstOrDefault(c => c.CategoryId == _transaction.CategoryId);

            SaveCommand = new Command(OnSaveTransaction);
            DeleteCommand = new Command(OnDeleteTransaction);
        }

        // Transaction property
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

        // Title property
        public string Title
        {
            get => _transaction?.Title ?? string.Empty;
            set
            {
                if (_transaction.Title != value)
                {
                    _transaction.Title = value;
                    OnPropertyChanged();
                }
            }
        }

        // Amount property
        public decimal Amount
        {
            get => _transaction?.Amount ?? 0;
            set
            {
                if (_transaction.Amount != value)
                {
                    _transaction.Amount = value;
                    OnPropertyChanged();
                }
            }
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    
                    // Update the transaction's CategoryId and CategoryName when category changes
                    if (_selectedCategory != null)
                    {
                        _transaction.CategoryId = _selectedCategory.CategoryId;
                        _transaction.CategoryName = _selectedCategory.Name;
                    }
                    
                    OnPropertyChanged();
                }
            }
        }

        // Date property
        public DateTime Date
        {
            get => _transaction?.Date ?? DateTime.Now;
            set
            {
                if (_transaction.Date != value)
                {
                    _transaction.Date = value;
                    OnPropertyChanged();
                }
            }
        }

        // Categories collection
        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        private async void OnSaveTransaction()
        {
            // Ensure we have a valid category selected
            if (SelectedCategory == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select a category", "OK");
                return;
            }

            // Update transaction with current SelectedCategory info
            _transaction.CategoryId = SelectedCategory.CategoryId;
            _transaction.CategoryName = SelectedCategory.Name;

            // Try to update the transaction
            try
            {
                
                Database.UpdateTransaction(_transaction);
                await Application.Current.MainPage.Navigation.PushAsync(new ViewTransactionsPage());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save: {ex.Message}", "OK");
            }
        }

        private void LoadCategories()
        {
            try
            {
                var categoryList = Database.Query<Category>("SELECT * FROM Categories");
                Categories = new ObservableCollection<Category>(categoryList);
                OnPropertyChanged(nameof(Categories));
            }
            catch (Exception ex)
            {
                // Handle exception (could log or show message)
                System.Diagnostics.Debug.WriteLine($"Error loading categories: {ex.Message}");
            }
        }

        private async void OnDeleteTransaction()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirm Delete", "Are you sure you want to delete this transaction?", "Yes", "No");

            if (confirm && _transaction != null) 
            {
                try
                {
                    
                    Database.DeleteTransaction(_transaction);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Failed to delete: {ex.Message}", "OK");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}