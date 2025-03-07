using CommunityToolkit.Maui;
using FrugalFoxBudgetApp.Views;
using Microsoft.Extensions.Logging;

namespace FrugalFoxBudgetApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<AddTransactionPage>();
        builder.Services.AddTransient<ViewReportsPage>();
        builder.Services.AddSingleton<AppShell>();
        return builder.Build();
    }
}