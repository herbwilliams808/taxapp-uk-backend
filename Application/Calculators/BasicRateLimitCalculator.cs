namespace Application.Calculators
{
    public class BasicRateLimitCalculator
    {
        public class BasicRateLimitCalculationResult
        {
            public decimal Value { get; set; }
            public string? Message { get; set; }
        }
        public BasicRateLimitCalculationResult Calculate(decimal basicRateThreshold, decimal pensionContributions, decimal giftAidContributions)
        {
            if (pensionContributions < 0 || giftAidContributions < 0)
            {
                throw new ArgumentException("Pension contributions and gift aid contributions must be non-negative.");
            }

            var result = new BasicRateLimitCalculationResult();
            result.Value = basicRateThreshold + pensionContributions + giftAidContributions;
            if (pensionContributions > 0 || giftAidContributions > 0)
            {
                result.Message =
                    $"Your basic rate limit has been extended by {pensionContributions + giftAidContributions}";
            }
            return result;
        }


    }
}