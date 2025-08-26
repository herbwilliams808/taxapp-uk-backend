using Core.Models.Incomes;

namespace Core.Interfaces.Calculators;

public interface ITotalEmploymentBenefitsCalculator
{
    decimal Calculate(IncomeSourcesDetails? incomeSources);
}