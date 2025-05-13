namespace Application.Calculators
{
    public class BasicRateLimitCalculator
    {
        public decimal Calculate(decimal basicRateThreshold, decimal pensionContributions, decimal giftAidContributions)
        {
            return basicRateThreshold + (pensionContributions + giftAidContributions);
        }
    }
}