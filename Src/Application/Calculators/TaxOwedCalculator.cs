using Application.Interfaces.Calculators;
using Shared.Models.CalculationResults;

namespace Application.Calculators;

public class TaxOwedCalculator : ITaxOwedCalculator
{
    public decimal Calculate(decimal totalIncome, BasicRateLimitCalculationResult basicRateLimit)
    {
        totalIncome = Math.Round(totalIncome, 0, MidpointRounding.ToNegativeInfinity);
        basicRateLimit.BasicRateLimitValue = Math.Round(basicRateLimit.BasicRateLimitValue, 0, MidpointRounding.ToPositiveInfinity);
        decimal taxOwed = 0;
        if (totalIncome > basicRateLimit.BasicRateLimitValue)
        {
            taxOwed = (totalIncome - basicRateLimit.BasicRateLimitValue) * 0.2m; // Example tax calculation
        }

        return Math.Round(taxOwed, 0, MidpointRounding.ToNegativeInfinity);
    }
}