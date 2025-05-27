using Shared.Models.Incomes;

namespace Application.Calculators
{
    public class TotalEmploymentBenefitsCalculator
    {
        public decimal Calculate(Incomes incomes)
        {
            if (incomes?.EmploymentsAndFinancialDetails == null || !incomes.EmploymentsAndFinancialDetails.Any())
                return 0;

            return incomes.EmploymentsAndFinancialDetails
                .Where(e => e.BenefitsInKind != null) // Filter out employments without BenefitsInKind
                .SelectMany(e => e.BenefitsInKind!.GetType()
                    .GetProperties()
                    .Where(p => p.PropertyType == typeof(decimal?))
                    .Select(p => (decimal?)p.GetValue(e.BenefitsInKind) ?? 0))
                .Sum();
        }
    }
}