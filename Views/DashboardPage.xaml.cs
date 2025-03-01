using Microsoft.Maui.Controls;
using FrugalFoxBudgetApp.ViewModels;
using FrugalFoxBudgetApp.Views;

namespace FrugalFoxBudgetApp
{
    public partial class DashboardPage : ContentPage
    {
        public DashboardPage()
        {
            InitializeComponent();
            // Do not set BindingContext here.
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (App.CurrentUser == null)
            {
                // Navigate to login if there's no logged-in user.
                await Shell.Current.GoToAsync(nameof(LoginPage));
                return;
            }
            else
            {
                // Set the BindingContext only if App.CurrentUser is set.
                if (BindingContext == null)
                {
                    BindingContext = new DashboardPageViewModel();
                }
            }
        }
    }
}
