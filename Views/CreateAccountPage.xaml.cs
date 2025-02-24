using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrugalFoxBudgetApp.ViewModels;

namespace FrugalFoxBudgetApp.Views;

public partial class CreateAccountPage : ContentPage
{
    public CreateAccountPage()
    {
        InitializeComponent();
        this.BindingContext = new CreateAccountViewModel();
    }
}