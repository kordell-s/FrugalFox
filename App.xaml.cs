using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Views;

namespace FrugalFoxBudgetApp;

public partial class App : Application
{
    public FrugalFoxDB FFoxDB;
    public App()
    {
        InitializeComponent();
        FFoxDB = new FrugalFoxDB();
        MainPage = new SplashScreen();
        

    }
}