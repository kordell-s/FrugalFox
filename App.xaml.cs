using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Models;
using FrugalFoxBudgetApp.Views;
using Syncfusion.Licensing;

namespace FrugalFoxBudgetApp;

public partial class App : Application
{
    public FrugalFoxDB FFoxDB;
    public static User CurrentUser { get; set; }
    public static event Action BudgetUpdated;

    public App()
    {
        SyncfusionLicenseProvider.RegisterLicense("Mzc0OTUzN0AzMjM4MmUzMDJlMzBqRjFTRUZKbVJKenRNeEZKUGVWWWJoZmVMSDRYWWxWcEFZMFdkMng3eHBJPQ==");

        InitializeComponent();
        FFoxDB = new FrugalFoxDB();
        MainPage = new AppShell();
        

    }
    public static void OnBudgetUpdated() => BudgetUpdated?.Invoke();

}