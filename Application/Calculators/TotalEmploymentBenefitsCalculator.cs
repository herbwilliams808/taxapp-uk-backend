using Application.Interfaces.Calculators;
using Shared.Models.Incomes;

namespace Application.Calculators;

public class TotalEmploymentBenefitsCalculator : ITotalEmploymentBenefitsCalculator
{ 
    public decimal Calculate(IncomeSourcesDetails? incomeSources)
    {
        if (incomeSources?.EmploymentsAndFinancialDetails == null || incomeSources.EmploymentsAndFinancialDetails.Count == 0)
            return 0;

        return incomeSources.EmploymentsAndFinancialDetails
            .Where(e => e.BenefitsInKind != null) // Filter out employments without BenefitsInKind
            .SelectMany(e => e.BenefitsInKind!.GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(decimal?))
                .Select(p => (decimal?)p.GetValue(e.BenefitsInKind) ?? 0))
            .Sum();
    }
}