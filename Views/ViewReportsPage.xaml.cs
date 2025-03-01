using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrugalFoxBudgetApp.Views;

public partial class ViewReportsPage : ContentPage
{
    public ViewReportsPage()
    {
        InitializeComponent();
        BindingContext = new ViewReportsPage();
    }
}