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
  
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (App.CurrentUser == null)
            {
                
                await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                return;
            }
            else
            {
                // Set the BindingContext only if App.CurrentUser is set.
                if (BindingContext == null)
                {
                    BindingContext = new DashboardPageViewModel();
                } else if (BindingContext is DashboardPageViewModel vm)
                {
                    vm.LoadRecentTransactions();
                }
            }
        }
    }
}
