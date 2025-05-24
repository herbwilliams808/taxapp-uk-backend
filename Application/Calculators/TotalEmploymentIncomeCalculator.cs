using Shared.Models.Incomes;
using Shared.Models.Incomes.NonSavingsIncomes.OtherEmploymentIncome.LumpSum;

namespace Application.Calculators;

public class TotalEmploymentIncomeCalculator
{
    public decimal Calculate(Incomes incomes)
    {
        var employmentIncomes = incomes.Employments.Select(employment => employment.Pay?.TaxablePayToDate ?? 0m);
        var totalEmploymentIncome = employmentIncomes?.Sum() ?? 0;
        
        var nonPayeEmploymentIncome = incomes.NonPayeEmploymentIncome.Tips ?? 0;

        var taxableLumpSums =
            incomes.OtherEmploymentIncome.LumpSums?.Select<LumpSum, decimal>(lumpSum =>
                lumpSum.TaxableLumpSumsAndCertainIncome?.Amount ?? 0m);
        var totalTaxableLumpSums = taxableLumpSums?.Sum() ?? 0;
        
        var employerFinancedRetirementSchemeBenefits = 
            incomes.OtherEmploymentIncome.LumpSums?.Select(lumpSum => 
                lumpSum.BenefitFromEmployerFinancedRetirementScheme?.Amount ?? 0m);
        var totalEmployerFinancedRetirementSchemeBenefits = employerFinancedRetirementSchemeBenefits?.Sum() ?? 0;
        
        var totalBenefitsInKind = new TotalEmploymentBenefitsCalculator().Calculate(incomes);
        
        return totalEmploymentIncome + 
               nonPayeEmploymentIncome + 
               totalTaxableLumpSums + 
               totalEmployerFinancedRetirementSchemeBenefits + 
               totalBenefitsInKind;
    }
}