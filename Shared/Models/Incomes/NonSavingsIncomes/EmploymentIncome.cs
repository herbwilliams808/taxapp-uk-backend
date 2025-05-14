namespace Shared.Models.Incomes.NonSavingsIncomes
{
    public class EmploymentIncome
    {
        /// <summary>
        /// The name of the employer.
        /// </summary>
        public string EmployerName { get; set; } = string.Empty;

        /// <summary>
        /// The total income earned from this employment.
        /// </summary>
        public decimal Income { get; set; }

        /// <summary>
        /// The tax paid on this income.
        /// </summary>
        public decimal TaxPaid { get; set; }
    }
}