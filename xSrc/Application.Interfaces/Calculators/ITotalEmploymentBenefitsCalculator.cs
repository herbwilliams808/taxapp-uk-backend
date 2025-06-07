using Shared.Models.Incomes;

namespace Application.Interfaces.Calculators;

public interface ITotalEmploymentBenefitsCalculator
{
    decimal Calculate(IncomeSourcesDetails? incomeSources);
}