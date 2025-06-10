using Application.Interfaces.Calculators;
using Shared.Models.Incomes;
using Shared.Models.IndividualsForeignIncome;
using Shared.Models.IndividualsReliefs.ForeignReliefs;

namespace Application.Calculators;

public class TotalEmploymentIncomeCalculator : ITotalEmploymentIncomeCalculator
{
    public decimal Calculate(
        IncomeSourcesDetails? incomes,
        // OtherDeductionsDetails otherDeductions,
        IndividualsForeignIncomeDetails? individualsForeignIncome,
        ForeignReliefsDetails? foreignReliefs)
    {
        var employmentIncomes = incomes?.EmploymentsAndFinancialDetails?.Select(employment => employment.Pay?.TaxablePayToDate ?? 0m);
        var totalEmploymentIncome = employmentIncomes?.Sum() ?? 0m;

        var nonPayeEmploymentIncome = incomes?.NonPayeEmploymentIncome?.Tips ?? 0m;

        var taxableLumpSums =
            incomes?.OtherEmploymentIncome?.LumpSums?.Select(lumpSum =>
                lumpSum.TaxableLumpSumsAndCertainIncome?.Amount ?? 0m);
        
        var totalTaxableLumpSums = taxableLumpSums?.Sum() ?? 0m;

        var employerFinancedRetirementSchemeBenefits =
            incomes?.OtherEmploymentIncome?.LumpSums?.Select(lumpSum =>
                lumpSum.BenefitFromEmployerFinancedRetirementScheme?.Amount ?? 0m);
        
        var totalEmployerFinancedRetirementSchemeBenefits = employerFinancedRetirementSchemeBenefits?.Sum() ?? 0;

        var earningsEarningsNotTaxableUk = individualsForeignIncome?.ForeignEarnings?.EarningsNotTaxableUK ?? 0;

        var foreignTaxForFtcrNotClaimed = foreignReliefs?.ForeignTaxForFtcrNotClaimed?.Amount ?? 0;

        return Math.Round(totalEmploymentIncome, 0, MidpointRounding.ToNegativeInfinity) +
               Math.Round(nonPayeEmploymentIncome, 0, MidpointRounding.ToNegativeInfinity) +
               Math.Round(totalTaxableLumpSums, 0, MidpointRounding.ToNegativeInfinity) +
               Math.Round(totalEmployerFinancedRetirementSchemeBenefits, 0, MidpointRounding.ToNegativeInfinity) -
               Math.Round(earningsEarningsNotTaxableUk, 0, MidpointRounding.ToPositiveInfinity) -
               Math.Round(foreignTaxForFtcrNotClaimed, 0, MidpointRounding.ToPositiveInfinity);
    }
}
