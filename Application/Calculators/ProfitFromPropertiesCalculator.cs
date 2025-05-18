using Shared.Models.Incomes;

namespace Application.Calculators
{
    public class ProfitFromPropertiesCalculator
    {
        /// <summary>
        /// Calculates the profit from properties.
        /// </summary>
        /// <param name="incomes"></param>
        /// <returns>The profit from properties after deducting allowable expenses.</returns>
        public decimal Calculate(Incomes incomes)
        {
            return (incomes.UkPropertyBusiness?.Income ?? 0m) - (incomes.UkPropertyBusiness?.AllowablePropertyLettingExpenses ?? 0m);
        }
    }
}