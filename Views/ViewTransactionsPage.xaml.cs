using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrugalFoxBudgetApp.ViewModels;

namespace FrugalFoxBudgetApp.Views;

public partial class ViewTransactionsPage : ContentPage
{
    public ViewTransactionsPage()
    {
        InitializeComponent();
        BindingContext = new ViewTransactionsViewModel();
    }
}