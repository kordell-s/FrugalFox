using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Models;
using FrugalFoxBudgetApp.Views;
using Microsoft.Maui.Controls;

namespace FrugalFoxBudgetApp.ViewModels
{
    public class ProfilePageViewModel : INotifyPropertyChanged
    {
        private readonly FrugalFoxDB Database;
        private User currentUser;
        
        public ProfilePageViewModel()
        {
            Database = new FrugalFoxDB();

            // Load User Data
            if (App.CurrentUser != null)
            {
                currentUser = Database.GetUserByEmail(App.CurrentUser.Email);
                Name = currentUser.FirstName;
                Email = currentUser.Email;
                SelectedTheme = Application.Current.RequestedTheme == AppTheme.Dark ? "Dark" : "Light";
            }

            ThemeOptions = new ObservableCollection<string> { "Light", "Dark" };
            
            // Commands
            SaveCommand = new Command(SaveChanges);
            LogoutCommand = new Command(Logout);
        }

        private string name;
        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(); }
        }

        private string email;
        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(); }
        }

        private string newPassword;
        public string NewPassword
        {
            get => newPassword;
            set { newPassword = value; OnPropertyChanged(); }
        }

        private string selectedTheme;
        public string SelectedTheme
        {
            get => selectedTheme;
            set
            {
                if (selectedTheme != value)
                {
                    selectedTheme = value;
                    OnPropertyChanged();
                    ApplyTheme();
                }
            }
        }

        public ObservableCollection<string> ThemeOptions { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand LogoutCommand { get; }

        private void SaveChanges()
        {
            if (currentUser != null)
            {
                currentUser.FirstName = Name;
                if (!string.IsNullOrEmpty(NewPassword))
                {
                    currentUser.Password = NewPassword; 
                }
                
                Database.UpdateUser(currentUser);
                Application.Current.MainPage.DisplayAlert("Success", "Profile updated successfully", "OK");
            }
        }

        private void ApplyTheme()
        {
            Application.Current.UserAppTheme = SelectedTheme == "Dark" ? AppTheme.Dark : AppTheme.Light;
        }

        private async void Logout()
        {
            App.CurrentUser = null;
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
