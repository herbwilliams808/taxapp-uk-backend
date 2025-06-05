using Shared.Models.Incomes;

namespace Application.Calculators
{
    public class TotalEmploymentExpensesCalculator
    {
        // TODO: Added virtual. Implement interface
        public virtual decimal Calculate(IncomeSources? incomeSources)
        {
            if (incomeSources?.EmploymentsAndFinancialDetails == null || 
                incomeSources.EmploymentsAndFinancialDetails.Count == 0)
                return 0;

            return incomeSources.EmploymentsAndFinancialDetails
                .Select(e => e.BenefitsInKind?.Expenses)?.Sum() ?? 0;
        }
    }
}