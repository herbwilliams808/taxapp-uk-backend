using Core.Models.CalculationResults;

namespace Core.Interfaces.Calculators;

public interface ITaxOwedCalculator
{
    decimal Calculate(decimal totalIncome, BasicRateLimitCalculationResult basicRateLimit);
}