namespace Application.Calculators
{
    public class BasicRateTaxCalculator
    {
        /// <summary>
        /// Calculates the tax on income within the basic rate band.
        /// </summary>
        /// <param name="incomeWithinBasicRateBand">The income that falls within the basic rate band.</param>
        /// <param name="basicRatePercentage">The basic rate percentage (e.g., 0.20 for 20%).</param>
        /// <returns>The tax owed on income within the basic rate band.</returns>
        public decimal Calculate(decimal incomeWithinBasicRateBand, decimal basicRatePercentage)
        {
            return incomeWithinBasicRateBand * basicRatePercentage;
        }
    }
}