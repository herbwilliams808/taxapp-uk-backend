namespace Application.Calculators
{
    public class ProfitFromPropertiesCalculator
    {
        /// <summary>
        /// Calculates the profit from properties.
        /// </summary>
        /// <param name="rentalIncome">The total rental income from properties.</param>
        /// <param name="allowableExpenses">The total allowable expenses for letting properties.</param>
        /// <returns>The profit from properties after deducting allowable expenses.</returns>
        public decimal Calculate(decimal rentalIncome, decimal allowableExpenses)
        {
            return rentalIncome - allowableExpenses;
        }
    }
}