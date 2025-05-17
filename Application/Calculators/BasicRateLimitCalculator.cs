namespace Application.Calculators
{
    public class BasicRateLimitCalculator
    {
        public decimal Calculate(decimal basicRateThreshold, decimal pensionContributions, decimal giftAidContributions)
        {
            if (pensionContributions < 0 || giftAidContributions < 0)
            {
                throw new ArgumentException("Pension contributions and gift aid contributions must be non-negative.");
            }

            return basicRateThreshold + (pensionContributions + giftAidContributions);
        }
    }
}