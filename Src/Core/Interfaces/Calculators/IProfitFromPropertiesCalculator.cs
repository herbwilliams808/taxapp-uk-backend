using Core.Models.Incomes;

namespace Core.Interfaces.Calculators;

public interface IProfitFromPropertiesCalculator
{
    decimal Calculate(IncomeSourcesDetails? incomeSources);
}