using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System;

namespace FrugalFoxBudgetApp.ViewModels;

public class DashboardPageViewModel:INotifyPropertyChanged
{

    private decimal monthlyBudget;
    private decimal amountSpent;
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}