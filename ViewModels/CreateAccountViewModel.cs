using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FrugalFoxBudgetApp.Models;
using FrugalFoxBudgetApp.Views;
using Microsoft.Maui.Controls;

namespace FrugalFoxBudgetApp.ViewModels
{
   public class CreateAccountViewModel : INotifyPropertyChanged
{
    private User user;
    private string confirmPassword;
    private string errorMessage;
    
    public CreateAccountViewModel()
    {
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
        if (!user.IsValidForRegistration(ConfirmPassword, out string error))
        {
            await Application.Current.MainPage.DisplayAlert("Error", error, "OK");
            return;
        }
        
        // Try to create the account
        bool success = user.CreateAccount();
        
        if (success)
        {
            await Application.Current.MainPage.DisplayAlert("Account", "Account created successfully", "OK");
            await Shell.Current.GoToAsync("//LoginPage");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Failed to create account", "OK");
        }
    }
    
    private async void OnNavigateToLogin()
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
}