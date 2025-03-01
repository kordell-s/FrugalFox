using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrugalFoxBudgetApp.ViewModels;

namespace FrugalFoxBudgetApp.Views;

public partial class SetBudgetPage : ContentPage
{
    public SetBudgetPage()
    {
        InitializeComponent();
        BindingContext = new SetBudgetViewModel();
    }
}