namespace Application.Calculators
{
    public class TotalIncomeCalculator
    {
        /// <summary>
        /// Calculates the total income received.
        /// </summary>
        /// <param name="employmentIncomes">A collection of employment incomes, which can be null.</param>
        /// <param name="profitFromProperties">The profit from properties, which can be null.</param>
        /// <returns>The total income received.</returns>
        public decimal Calculate(IEnumerable<decimal>? employmentIncomes, decimal? profitFromProperties)
        {
            var totalEmploymentIncome = employmentIncomes?.Sum() ?? 0;
            var totalProfitFromProperties = profitFromProperties ?? 0;

            return totalEmploymentIncome + totalProfitFromProperties;
        }
    }
}