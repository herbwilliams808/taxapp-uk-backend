using Core.Interfaces.Calculators;

namespace Application.Calculators;

public class TotalIncomeCalculator : ITotalIncomeCalculator
{
    /// <summary>
    ///     Calculates the total income received.
    /// </summary>
    /// <param name="employmentIncome">The total value of employment incomes, which can be null.</param>
    /// <param name="benefitsInKind"></param>
    /// <param name="employmentExpenses"></param>
    /// <param name="otherDeductions"></param>
    /// <param name="profitFromProperties">The profit from properties, which can be null.</param>
    /// <returns>The total income received.</returns>
    public decimal Calculate(decimal? employmentIncome, decimal? benefitsInKind, decimal? employmentExpenses,
        decimal? otherDeductions, decimal? profitFromProperties)
    {
        return
            Math.Round(employmentIncome?? 0m, 0, MidpointRounding.ToNegativeInfinity) +
            Math.Round(benefitsInKind?? 0m, 0, MidpointRounding.ToNegativeInfinity) -
            Math.Round(employmentExpenses?? 0m, 0, MidpointRounding.ToPositiveInfinity) -
            Math.Round(otherDeductions?? 0m, 0, MidpointRounding.ToPositiveInfinity) +
            Math.Round(profitFromProperties?? 0m, 0, MidpointRounding.ToNegativeInfinity);
    }
}