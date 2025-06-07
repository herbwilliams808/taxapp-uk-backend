using Shared.Models.CalculationResults;

namespace Application.Interfaces.Calculators;

public interface IBasicRateLimitCalculator
{
    BasicRateLimitCalculationResult Calculate(decimal basicRateThreshold,
        decimal pensionContributions, decimal giftAidContributions);
}