using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrugalFoxBudgetApp.Views;

public partial class SplashScreen : ContentPage
{
    public SplashScreen()
    {
        InitializeComponent();

        // Option 1: Simple timer-based approach
        Device.StartTimer(TimeSpan.FromSeconds(2), () =>
        {
            // Navigate to Shell or MainPage
            Application.Current.MainPage = new NavigationPage(new CreateAccountPage()); 
            return false; // Return false to stop the timer
        });
    }
}