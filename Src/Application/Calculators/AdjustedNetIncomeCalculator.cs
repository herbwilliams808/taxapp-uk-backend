using Core.Interfaces.Calculators;

namespace Application.Calculators;

public class AdjustedNetIncomeCalculator : IAdjustedNetIncomeCalculator
{
    /// <summary>
    ///     Calculates the adjusted net income.
    ///     TODO: Review this calculation to align with HMRC guidance.
    ///     https://www.gov.uk/guidance/adjusted-net-income
    /// </summary>
    /// <param name="netIncome">The total income received.</param>
    /// <param name="pensionContributions">The amount contributed to pensions, which can be null.</param>
    /// <param name="giftAidContributions">The amount contributed to gift aid, which can be null.</param>
    /// <returns>The adjusted net income.</returns>
    public decimal Calculate(decimal netIncome, decimal? pensionContributions, decimal? giftAidContributions)
    {
        var totalDeductions = Math.Round(pensionContributions ?? 0, 0, MidpointRounding.ToPositiveInfinity) +
                              Math.Round(giftAidContributions ?? 0, 0, MidpointRounding.ToPositiveInfinity);
        return netIncome - totalDeductions;
    }
}