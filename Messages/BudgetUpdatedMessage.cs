using CommunityToolkit.Mvvm.Messaging.Messages;

namespace FrugalFoxBudgetApp.Messages;

    public class BudgetUpdatedMessage : ValueChangedMessage<bool>
    {
        public BudgetUpdatedMessage(bool value) : base(value)
        {
        }
    }