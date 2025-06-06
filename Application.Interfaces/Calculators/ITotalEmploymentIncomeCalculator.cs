using Shared.Models.Incomes;
using Shared.Models.IndividualsForeignIncome;
using Shared.Models.IndividualsReliefs.ForeignReliefs;

namespace Application.Interfaces.Calculators;

public interface ITotalEmploymentIncomeCalculator
{
    decimal Calculate(
        IncomeSourcesDetails? incomes,
        // OtherDeductionsDetails otherDeductions,
        IndividualsForeignIncomeDetails? individualsForeignIncome,
        ForeignReliefsDetails? foreignReliefs);
}