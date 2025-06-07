using Application.Interfaces.Calculators;
using Shared.Models.Incomes;

namespace Application.Calculators
{
    public class TotalEmploymentExpensesCalculator : ITotalEmploymentExpensesCalculator
    {
        public decimal Calculate(IncomeSourcesDetails? incomeSources)
        {
            if (incomeSources?.EmploymentsAndFinancialDetails == null || 
                incomeSources.EmploymentsAndFinancialDetails.Count == 0)
                return 0;

            return incomeSources.EmploymentsAndFinancialDetails
                .Select(e => e.BenefitsInKind?.Expenses)?.Sum() ?? 0;
        }
    }
}