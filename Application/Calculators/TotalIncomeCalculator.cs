namespace Application.Calculators
{
    public class TotalIncomeCalculator
    {
        /// <summary>
        /// Calculates the total income received.
        /// </summary>
        /// <param name="employmentIncome">The total value of employment incomes, which can be null.</param>
        /// <param name="nonPayeEmploymentIncome">The total value of non-PAYE employment income received, which can be null.</param>
        /// <param name="profitFromProperties">The profit from properties, which can be null.</param>
        /// <returns>The total income received.</returns>
        public decimal Calculate(decimal? employmentIncome, decimal? nonPayeEmploymentIncome, decimal? profitFromProperties)
        {
            var totalEmploymentIncome = employmentIncome ?? 0;
            var totalnonPayeEmploymentIncome = nonPayeEmploymentIncome ?? 0;
            var totalprofitFromProperties = profitFromProperties ?? 0;
            
            return totalEmploymentIncome + totalnonPayeEmploymentIncome + totalprofitFromProperties;
        }
    }
}