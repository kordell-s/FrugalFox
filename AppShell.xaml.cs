using FrugalFoxBudgetApp.Views;

namespace FrugalFoxBudgetApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        // Register routes for each page
        Routing.RegisterRoute("LoginPage", typeof(LoginPage));
        Routing.RegisterRoute("DashboardPage", typeof(DashboardPage));
        Routing.RegisterRoute("AddTransactionPage", typeof(AddTransactionPage));
        Routing.RegisterRoute("ViewReportsPage", typeof(ViewReportsPage));
        Routing.RegisterRoute("ViewTransactionsPage", typeof(ViewTransactionsPage));

    }
}