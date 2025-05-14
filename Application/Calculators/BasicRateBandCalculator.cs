namespace Application.Calculators
{
    public class BasicRateBandCalculator
    {
        /// <summary>
        /// Calculates the income within the basic rate band.
        /// </summary>
        /// <param name="taxableIncome">The taxable income.</param>
        /// <param name="basicRateThreshold">The basic rate threshold for the tax year.</param>
        /// <param name="extensionAmount">Adjustments such as pension contributions and gift aid contributions, which can be null.</param>
        /// <returns>The income within the basic rate band.</returns>
        public decimal Calculate(decimal taxableIncome, decimal basicRateThreshold, decimal? extensionAmount)
        {
            var adjustedThreshold = basicRateThreshold + (extensionAmount ?? 0);
            return Math.Min(adjustedThreshold, taxableIncome);
        }
    }
}