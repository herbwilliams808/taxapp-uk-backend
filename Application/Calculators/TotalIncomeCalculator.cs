namespace Application.Calculators
{
    public class TotalIncomeCalculator
    {
        /// <summary>
        /// Calculates the total income received.
        /// </summary>
        /// <param name="employmentIncomes">A collection of employment incomes, which can be null.</param>
        /// <param name="nonPayeEmploymentIncome">The total amount of non-PAYE employment income received, which can be null.</param>
        /// <param name="profitFromProperties">The profit from properties, which can be null.</param>
        /// <returns>The total income received.</returns>
        public decimal Calculate(IEnumerable<decimal>? employmentIncomes, decimal? nonPayeEmploymentIncome, decimal? profitFromProperties)
        {
            var totalEmploymentIncome = employmentIncomes?.Sum() ?? 0;
            var totalnonPayeEmploymentIncome = nonPayeEmploymentIncome ?? 0;
            var totalprofitFromProperties = profitFromProperties ?? 0;
            
            return totalEmploymentIncome + totalnonPayeEmploymentIncome + totalprofitFromProperties;
        }
    }
}