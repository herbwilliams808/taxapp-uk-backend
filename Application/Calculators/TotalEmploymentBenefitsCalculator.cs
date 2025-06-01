using Shared.Models.Incomes;

namespace Application.Calculators
{
    public class TotalEmploymentBenefitsCalculator
    {
        public decimal Calculate(IncomeDetails incomeDetails)
        {
            if (incomeDetails?.EmploymentsAndFinancialDetails == null || !incomeDetails.EmploymentsAndFinancialDetails.Any())
                return 0;

            return incomeDetails.EmploymentsAndFinancialDetails
                .Where(e => e.BenefitsInKind != null) // Filter out employments without BenefitsInKind
                .SelectMany(e => e.BenefitsInKind!.GetType()
                    .GetProperties()
                    .Where(p => p.PropertyType == typeof(decimal?))
                    .Select(p => (decimal?)p.GetValue(e.BenefitsInKind) ?? 0))
                .Sum();
        }
    }
}