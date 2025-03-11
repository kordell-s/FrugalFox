using CommunityToolkit.Maui;
using FrugalFoxBudgetApp.Views;
using Microsoft.Extensions.Logging;
using Microcharts.Maui;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Syncfusion.Licensing;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.Toolkit.Hosting;

namespace FrugalFoxBudgetApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureSyncfusionCore()
            .ConfigureSyncfusionToolkit()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Oswald-VariableFont_wght.ttf", "Oswald");
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