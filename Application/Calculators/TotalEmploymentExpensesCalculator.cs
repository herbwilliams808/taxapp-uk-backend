using Shared.Models.Incomes;

namespace Application.Calculators
{
    public class TotalEmploymentExpensesCalculator
    {
        public decimal Calculate(Incomes incomes)
        {
            if (incomes?.EmploymentsAndFinancialDetails == null || !incomes.EmploymentsAndFinancialDetails.Any())
                return 0;

            return incomes.EmploymentsAndFinancialDetails
                .Select(e => e.BenefitsInKind?.Expenses)?.Sum() ?? 0;
        }
    }
}