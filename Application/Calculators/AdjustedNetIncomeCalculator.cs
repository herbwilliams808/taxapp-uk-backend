namespace Application.Calculators
{
    public class AdjustedNetIncomeCalculator
    {
        /// <summary>
        /// Calculates the adjusted net income.
        /// </summary>
        /// <param name="netIncome">The total income received.</param>
        /// <param name="pensionContributions">The amount contributed to pensions, which can be null.</param>
        /// <param name="giftAidContributions">The amount contributed to gift aid, which can be null.</param>
        /// <returns>The adjusted net income.</returns>
        public decimal Calculate(decimal netIncome, decimal? pensionContributions, decimal? giftAidContributions)
        {
            var totalDeductions = (pensionContributions ?? 0) + (giftAidContributions ?? 0);
            return netIncome - totalDeductions;
        }
    }
}