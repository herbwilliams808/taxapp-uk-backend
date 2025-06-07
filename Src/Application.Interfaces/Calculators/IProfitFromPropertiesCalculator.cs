using Shared.Models.Incomes;

namespace Application.Interfaces.Calculators;

public interface IProfitFromPropertiesCalculator
{
    /// <summary>
    /// Calculates the profit from properties.
    /// </summary>
    /// <param name="incomeSources"></param>
    /// <returns>The profit from properties after deducting allowable expenses.</returns>
    decimal Calculate(IncomeSourcesDetails? incomeSources);
}