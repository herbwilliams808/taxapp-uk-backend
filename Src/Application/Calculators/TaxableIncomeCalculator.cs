namespace Application.Calculators
{
    public class TaxableIncomeCalculator
    {
        /// <summary>
        /// Calculates the taxable income.
        /// </summary>
        /// <param name="adjustedNetIncome">The adjusted net income.</param>
        /// <param name="personalAllowance">The personal allowance.</param>
        /// <returns>The taxable income, or 0 if adjusted net income is less than or equal to the personal allowance.</returns>
        public decimal Calculate(
            decimal adjustedNetIncome, 
            decimal personalAllowance)
        {
            return adjustedNetIncome > personalAllowance 
                ? adjustedNetIncome - personalAllowance 
                : 0;
        }
    }
}