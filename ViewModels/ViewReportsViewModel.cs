using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FrugalFoxBudgetApp.Database;
using FrugalFoxBudgetApp.Models;

namespace FrugalFoxBudgetApp.ViewModels
{
    public class ViewReportsViewModel : INotifyPropertyChanged
    {
        private readonly FrugalFoxDB Database;

        public ViewReportsViewModel()
        {
            Database = new FrugalFoxDB();

            TimePeriodOptions = new ObservableCollection<string>
            {
                "This Month",
                "This Year",
                "All Time"
            };
            SelectedTimePeriod = TimePeriodOptions.FirstOrDefault();

            ReportChartData = new ObservableCollection<ReportItem>();
            ReportItems = new ObservableCollection<ReportItem>();
            
            SelectedTimePeriod = TimePeriodOptions.FirstOrDefault() ?? "This Month";

            LoadReports();
        }

        // Time period properties
        public ObservableCollection<string> TimePeriodOptions { get; set; }
        
        private string selectedTimePeriod;
        public string SelectedTimePeriod
        {
            get => selectedTimePeriod;
            set
            {
                if (selectedTimePeriod != value)
                {
                    selectedTimePeriod = value;
                    OnPropertyChanged();
                    LoadReports();
                }
            }
        }

        // Data for Syncfusion chart.
        private ObservableCollection<ReportItem> reportChartData;
        public ObservableCollection<ReportItem> ReportChartData
        {
            get => reportChartData;
            set
            {
                if (reportChartData != value)
                {
                    reportChartData = value;
                    OnPropertyChanged();
                }
            }
        }

        // Detailed report items for a list, if needed.
        private ObservableCollection<ReportItem> reportItems;
        public ObservableCollection<ReportItem> ReportItems
        {
            get => reportItems;
            set
            {
                if (reportItems != value)
                {
                    reportItems = value;
                    OnPropertyChanged();
                }
            }
        }

        // Loads report data based on the selected time period.
        private void LoadReports()
        {
            if (App.CurrentUser == null)
            {
                System.Diagnostics.Debug.WriteLine("Error: No user is logged in.");
                return;

            }
            if (Database == null)
            {
                System.Diagnostics.Debug.WriteLine("Error: Database is NULL");
                return;
            }
           

            DateTime startDate;
            DateTime endDate = DateTime.Today.AddDays(1).AddTicks(-1);
            switch (SelectedTimePeriod)
            {
                case "This Month":
                    startDate = new DateTime(endDate.Year, endDate.Month, 1);
                    break;
                case "This Year":
                    startDate = new DateTime(endDate.Year, 1, 1);
                    break;
                case "All Time":
                default:
                    startDate = DateTime.MinValue;
                    break;
            }

            var transactions = Database.Query<Transaction>(
                "SELECT t.*, c.Name as CategoryName, c.Icon as CategoryIcon " +
                "FROM Transactions t " +
                "LEFT JOIN Categories c ON t.CategoryId = c.CategoryId " +
                "WHERE t.UserId = ? AND t.Date BETWEEN ? AND ?",
                App.CurrentUser.UserId, startDate, endDate);
            
            if (transactions == null)
            {
                System.Diagnostics.Debug.WriteLine("Error: Transactions query returned NULL");
                transactions = new List<Transaction>(); // Avoid null reference
            }

            // Group transactions by category and sum the amounts.
            var groups = transactions
                .Where(t => t != null) // Ensure no null transactions
                .GroupBy(t => t.CategoryName)
                .Select(g => new ReportItem
                {
                    Category = g.Key,
                    TotalValue = g.Sum(t => (double)t.Amount)
                })
                .ToList();

            // Update the chart data collection.
           ReportChartData = new ObservableCollection<ReportItem>(groups);
           OnPropertyChanged(nameof(ReportChartData));
           
           ReportItems = new ObservableCollection<ReportItem>(groups);
           OnPropertyChanged(nameof(ReportItems));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
