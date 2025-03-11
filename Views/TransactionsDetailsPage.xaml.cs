using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrugalFoxBudgetApp.Models;
using FrugalFoxBudgetApp.ViewModels;

namespace FrugalFoxBudgetApp.Views;

public partial class TransactionsDetailsPage : ContentPage
{
    public TransactionsDetailsPage(Transaction transaction)
    {
        InitializeComponent();

        this.BindingContext = new TransactionDetailsViewModel(transaction);
    }

}