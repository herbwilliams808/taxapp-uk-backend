using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.Incomes.NonSavingsIncomes.OtherEmploymentIncome
{
    /// <summary>
    /// Represents details of disability-related income deductions.
    /// </summary>
    public class Disability
    {
        /// <summary>
        /// Customer reference associated with the disability income, if available.
        /// </summary>
        [SwaggerSchema("Customer reference associated with the disability income, if available.")]
        public string? CustomerReference { get; set; }

        /// <summary>
        /// Amount deducted for disability-related income.
        /// </summary>
        [SwaggerSchema("Amount deducted for disability-related income.")]
        public decimal AmountDeducted { get; set; }
    }
}