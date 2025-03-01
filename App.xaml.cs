using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Models;
using FrugalFoxBudgetApp.Views;

namespace FrugalFoxBudgetApp;

public partial class App : Application
{
    public FrugalFoxDB FFoxDB;
    public static User CurrentUser { get; set; }
    public App()
    {
        InitializeComponent();
        FFoxDB = new FrugalFoxDB();
        MainPage = new NavigationPage( new SplashScreen());
        

    }
}