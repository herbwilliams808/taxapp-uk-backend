using System.Globalization;
using Application.Interfaces.Calculators;
using Shared.Models.CalculationResults;

namespace Application.Calculators;

public class BasicRateLimitCalculator : IBasicRateLimitCalculator
{
    public BasicRateLimitCalculationResult Calculate(decimal basicRateThreshold, decimal pensionContributions, decimal giftAidContributions)
    {
        
        if (pensionContributions < 0 || giftAidContributions < 0)
        {
            throw new ArgumentException("Pension contributions and gift aid contributions must be non-negative.");
        }

        pensionContributions = Math.Round(pensionContributions, 0, MidpointRounding.ToPositiveInfinity);
        giftAidContributions = Math.Round(giftAidContributions, 0, MidpointRounding.ToPositiveInfinity);
        var basicRateLimitExtendedByAmount = pensionContributions + giftAidContributions;

        return new BasicRateLimitCalculationResult
        {
            BasicRateLimitValue = basicRateThreshold + pensionContributions + giftAidContributions,
            BasicRateLimitExtendedByAmount = basicRateLimitExtendedByAmount,
            CurrencyCode = "GBP",
            GrossGiftAidPaymentsValue = giftAidContributions,
            GrossPensionContributionsValue = pensionContributions,
            Message = basicRateLimitExtendedByAmount > 0 ? "Your basic rate limit has been extended due to the payments you made into pension / gift aid." : null
        };
    }


}