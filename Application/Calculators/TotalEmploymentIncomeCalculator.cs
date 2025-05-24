using Shared.Models.Incomes;
using Shared.Models.IndividualsForeignIncome;
using Shared.Models.IndividualsReliefs.ForeignReliefs;
using Shared.Models.OtherDeductions;

namespace Application.Calculators;

public class TotalEmploymentIncomeCalculator(
    TotalEmploymentBenefitsCalculator benefitsCalculator,
    TotalEmploymentExpensesCalculator expensesCalculator,
    TotalOtherDeductionsCalculator otherDeductionsCalculator)
{
    private readonly TotalEmploymentBenefitsCalculator _benefitsCalculator = benefitsCalculator;
    private readonly TotalEmploymentExpensesCalculator _expensesCalculator = expensesCalculator;
    private readonly TotalOtherDeductionsCalculator _otherDeductionsCalculator = otherDeductionsCalculator;

    public decimal Calculate(
        Incomes incomes,
        OtherDeductions otherDeductions,
        IndividualsForeignIncome individualsForeignIncome,
        ForeignReliefs foreignReliefs)
    {
        var employmentIncomes = incomes.Employments.Select(employment => employment.Pay?.TaxablePayToDate ?? 0m);
        var totalEmploymentIncome = employmentIncomes?.Sum() ?? 0;

        var nonPayeEmploymentIncome = incomes.NonPayeEmploymentIncome.Tips ?? 0;

        var taxableLumpSums =
            incomes.OtherEmploymentIncome.LumpSums?.Select(lumpSum =>
                lumpSum.TaxableLumpSumsAndCertainIncome?.Amount ?? 0m);
        var totalTaxableLumpSums = taxableLumpSums?.Sum() ?? 0;

        var employerFinancedRetirementSchemeBenefits =
            incomes.OtherEmploymentIncome.LumpSums?.Select(lumpSum =>
                lumpSum.BenefitFromEmployerFinancedRetirementScheme?.Amount ?? 0m);
        var totalEmployerFinancedRetirementSchemeBenefits = employerFinancedRetirementSchemeBenefits?.Sum() ?? 0;

        var totalBenefitsInKind = _benefitsCalculator.Calculate(incomes);

        var totalEmploymentExpenses = _expensesCalculator.Calculate(incomes);

        var totalOtherDeductions = _otherDeductionsCalculator.Calculate(otherDeductions);

        var earningsEarningsNotTaxableUk = individualsForeignIncome.ForeignEarnings?.EarningsNotTaxableUK ?? 0;

        var foreignTaxForFtcrNotClaimed = foreignReliefs.ForeignTaxForFtcrNotClaimed?.Amount ?? 0;

        return totalEmploymentIncome +
               nonPayeEmploymentIncome +
               totalTaxableLumpSums +
               totalEmployerFinancedRetirementSchemeBenefits +
               totalBenefitsInKind -
               totalEmploymentExpenses -
               totalOtherDeductions -
               earningsEarningsNotTaxableUk -
               foreignTaxForFtcrNotClaimed;
    }
}
