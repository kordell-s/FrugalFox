using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrugalFoxBudgetApp.Models;
using FrugalFoxBudgetApp.ViewModels;

namespace FrugalFoxBudgetApp.Views;

public partial class EditTransactionPage : ContentPage
{
    public EditTransactionPage(Transaction transaction)
    {
        InitializeComponent();
        BindingContext = new EditTransactionViewModel(transaction);

    }
}