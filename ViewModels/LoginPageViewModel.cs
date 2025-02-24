using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace FrugalFoxBudgetApp.ViewModels
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        private string email;
        private string password;
        private string errorMessage;

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

        public LoginPageViewModel()
        {
            LoginCommand = new Command(OnLogin);
            NavigateToCreateAccountCommand = new Command(OnNavigateToCreateAccount);
        }

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
                // Here you would typically:
                // 1. Create a User model from the credentials
                // 2. Validate the credentials against a database or API

                // Simulate authentication for now
                bool isAuthenticated = Email.Contains("@") && Password.Length >= 6;

                if (isAuthenticated)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Login successful.", "OK");
                    // Navigate to the main/home page of the app
                    await Shell.Current.GoToAsync("//DashboardPage");
                }
                else
                {
                    ErrorMessage = "Invalid email or password.";
                }
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