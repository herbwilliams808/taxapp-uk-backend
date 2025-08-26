using Core.Models.Incomes;
using Core.Models.IndividualsForeignIncome;
using Core.Models.IndividualsReliefs.ForeignReliefs;

namespace Core.Interfaces.Calculators;

public interface ITotalEmploymentIncomeCalculator
{
    decimal Calculate(
        IncomeSourcesDetails? incomes,
        // OtherDeductionsDetails otherDeductions,
        IndividualsForeignIncomeDetails? individualsForeignIncome,
        ForeignReliefsDetails? foreignReliefs);
}