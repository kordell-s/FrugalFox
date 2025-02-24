using FrugalFoxBudgetApp.Views;

namespace FrugalFoxBudgetApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new SplashScreen();
        

    }
}