using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Models;
using Microcharts;
using SkiaSharp;

namespace FrugalFoxBudgetApp.ViewModels
{
    public class ViewReportsViewModel : INotifyPropertyChanged
    {
        private readonly FrugalFoxDB Database;

        public ViewReportsViewModel()
        {
            Database = new FrugalFoxDB();
            LoadReports(); 
        }

        private ObservableCollection<ReportItem> reportItems;
        private Chart reportChart;

        public ObservableCollection<ReportItem> ReportItems
        {
            get => reportItems;
            set
            {
                if (reportItems != value)
                {
                    reportItems = value;
                    OnPropertyChanged(nameof(ReportItems));
                }
            }
        }

        public Chart ReportChart
        {
            get => reportChart;
            set
            {
                if (reportChart != value)
                {
                    reportChart = value;
                    OnPropertyChanged(nameof(ReportChart));
                }
            }
        }

        private void LoadReports()
        {
            // Query transactions for the current user.
            // Make sure App.CurrentUser is not null.
            var transactions = Database.Query<Transaction>(
                "SELECT * FROM Transactions WHERE UserId = ?", App.CurrentUser.UserId);

            // Group transactions by category and sum the amounts.
            var categoryTotals = transactions.GroupBy(t => t.CategoryName)
                .Select(g => new {
                    Category = g.Key,
                    Total = g.Sum(t => t.Amount)
                })
                .ToList();

            // Create chart entries for each category.
            var entries = categoryTotals.Select(ct => new Microcharts.ChartEntry((float)ct.Total)
            {
                Label = ct.Category,
                ValueLabel = ct.Total.ToString("C"),
                Color = SKColor.Parse(GetColorForCategory(ct.Category))
            }).ToArray();


            // Create a pie chart with the entries.
            ReportChart = new PieChart
            {
                Entries = entries,
                LabelTextSize = 40,
                BackgroundColor = SKColors.Transparent
            };

            // Build a list of report items for the ListView.
            ReportItems = new ObservableCollection<ReportItem>(
                transactions.Select(t => new ReportItem
                {
                    Title = $"{t.CategoryName}: {t.Amount:C}",
                    Detail = t.Date.ToString("MMM dd, yyyy")
                })
            );
        }

        private string GetColorForCategory(string category)
        {
            // Return fixed colors based on category.
            return category switch
            {
                "Food" => "#F39C12",
                "Transport" => "#27AE60",
                "Entertainment" => "#8E44AD",
                "Utilities" => "#2980B9",
                _ => "#2C3E50"
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
