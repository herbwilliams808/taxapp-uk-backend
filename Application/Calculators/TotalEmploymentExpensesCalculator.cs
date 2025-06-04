using Shared.Models.Incomes;

namespace Application.Calculators
{
    public class TotalEmploymentExpensesCalculator
    {
        // TODO: Added virtual. Implement interface
        public virtual decimal Calculate(IncomeDetails incomeDetails)
        {
            if (incomeDetails?.EmploymentsAndFinancialDetails == null || !incomeDetails.EmploymentsAndFinancialDetails.Any())
                return 0;

            return incomeDetails.EmploymentsAndFinancialDetails
                .Select(e => e.BenefitsInKind?.Expenses)?.Sum() ?? 0;
        }
    }
}