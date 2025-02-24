using FrugalFoxBudgetApp.Views;

namespace FrugalFoxBudgetApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(CreateAccountPage), typeof(CreateAccountPage));
        Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
    }
}