using Shared.Models.Incomes;

namespace Application.Calculators
{
    public class TotalEmploymentExpensesCalculator
    {
        public decimal CalculateTotalBenefits(Incomes incomes)
        {
            if (incomes?.Employments == null || !incomes.Employments.Any())
                return 0;

            return incomes.Employments
                .Where(e => e.Expenses != null) // Filter out employments without BenefitsInKind
                .SelectMany(e => e.Expenses!.GetType()
                    .GetProperties()
                    .Where(p => p.PropertyType == typeof(decimal?))
                    .Select(p => (decimal?)p.GetValue(e.BenefitsInKind) ?? 0))
                .Sum();
        }
    }
}