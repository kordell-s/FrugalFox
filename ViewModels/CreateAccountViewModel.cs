using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FrugalFoxBudgetApp.Models;
using FrugalFoxBudgetApp.Views;
using Microsoft.Maui.Controls;
using FrugalFoxBudgetApp.Database;
using Microsoft.Win32.SafeHandles;

namespace FrugalFoxBudgetApp.ViewModels
{
   public class CreateAccountViewModel : INotifyPropertyChanged
   { 
       private readonly FrugalFoxDB Database;
    private User user;
    private string confirmPassword;
    private string errorMessage;
    
    public CreateAccountViewModel()
    {
        Database = new FrugalFoxDB();
        user = new User();
        CreateAccountCommand = new Command(OnCreateAccount);
        NavigateToLoginCommand = new Command(OnNavigateToLogin);
    }
    
    public string FirstName
    {
        get => user.FirstName;
        set 
        { 
            user.FirstName = value; 
            OnPropertyChanged();
        }
    }
    
    public string LastName
    {
        get => user.LastName;
        set 
        { 
            user.LastName = value; 
            OnPropertyChanged();
        }
    }
    
    public string Email
    {
        get => user.Email;
        set 
        { 
            user.Email = value; 
            OnPropertyChanged();
        }
    }
    
    public string Password
    {
        get => user.Password;
        set 
        { 
            user.Password = value; 
            OnPropertyChanged();
        }
    }
    
    public string ConfirmPassword
    {
        get => confirmPassword;
        set 
        { 
            confirmPassword = value; 
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
        }
    }
    
    public ICommand CreateAccountCommand { get; }
    public ICommand NavigateToLoginCommand { get; }

    private async void OnCreateAccount()
    {
        // Use the model's validation method
        if (string.IsNullOrWhiteSpace(FirstName))
        {
            ErrorMessage = "First name is required.";
            return;
        }

        if (string.IsNullOrWhiteSpace(LastName))
        {
            ErrorMessage = "Last name is required.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            ErrorMessage = "Email is required.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Password is required.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Passwords don't match.";
            return;
        }

        try
        {
            //checking if email/account already exists
            var existingUser = Database.GetUserByEmail(Email);
            if (existingUser != null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Email {Email} already exists.", "OK");
                return;
            }
            

            //setting creation date
            user.CreatedDate = DateTime.Now;

            //Hashing password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password);
            user.Password = null;

            //insert into database
            int result = Database.CreateUser(user);
            if (result > 0)
            {
                //setting the newly created user as the current user
                App.CurrentUser = user;
                await Application.Current.MainPage.DisplayAlert("Success", $"User {Email} has been created", "OK");
                await Shell.Current.GoToAsync(nameof(DashboardPage));
            }

            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to create account", "OK");
            }

        }
    catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }
    
    private async void OnNavigateToLogin()
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
}