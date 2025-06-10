using Shared.Models.CalculationResults;

namespace Application.Interfaces.Calculators;

public interface ITaxOwedCalculator
{
    decimal Calculate(decimal totalIncome, BasicRateLimitCalculationResult basicRateLimit);
}