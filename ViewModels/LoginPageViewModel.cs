using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Models;
using Microsoft.Maui.Controls;

namespace FrugalFoxBudgetApp.ViewModels
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        private readonly FrugalFoxDB Database;
        private string email;
        private string password;
        private string errorMessage;
        
        public LoginPageViewModel()
        {
            Database = new FrugalFoxDB();
            LoginCommand = new Command(OnLogin);
            NavigateToCreateAccountCommand = new Command(OnNavigateToCreateAccount);
        }


        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsErrorVisible));
            }
        }

        public bool IsErrorVisible => !string.IsNullOrEmpty(ErrorMessage);

        public ICommand LoginCommand { get; }
        public ICommand NavigateToCreateAccountCommand { get; }
        
        private async void OnLogin()
        {
            // Clear previous error
            ErrorMessage = string.Empty;

            // Validate input fields
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Please fill in both email and password.";
                return;
            }

         

            try
            {
                ErrorMessage = string.Empty;
                
                //getting user by email
                User user = Database.GetUserByEmail(Email);
                if (user == null)
                {
                    ErrorMessage = "Invalid email or password.";
                    return;
                }
                
                //verify password
                bool passwordValid = BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash);
                if (!passwordValid)
                {
                    ErrorMessage = "Invalid password.";
                    return;
                }
                
                App.CurrentUser = user; //storing logged in user
                
                //navigate to dashboard
                await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
            }
        }

        private async void OnNavigateToCreateAccount()
        {
            await Shell.Current.GoToAsync("//CreateAccountPage");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}