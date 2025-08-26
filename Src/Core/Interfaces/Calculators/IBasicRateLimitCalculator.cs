using Core.Models.CalculationResults;

namespace Core.Interfaces.Calculators;

public interface IBasicRateLimitCalculator
{
    BasicRateLimitCalculationResult Calculate(decimal basicRateThreshold,
        decimal pensionContributions, decimal giftAidContributions);
}