using Application.Interfaces.Calculators;
using Shared.Models.Incomes;

namespace Application.Calculators;

public class ProfitFromPropertiesCalculator : IProfitFromPropertiesCalculator
{
    /// <summary>
    /// Calculates the profit from properties.
    /// </summary>
    /// <param name="incomeSources"></param>
    /// <returns>The profit from properties after deducting allowable expenses.</returns>
    public decimal Calculate(IncomeSources? incomeSources)
    {
        return (incomeSources?.UkPropertyBusiness?.Income ?? 0m) - 
               (incomeSources?.UkPropertyBusiness?.AllowablePropertyLettingExpenses ?? 0m);
    }
}