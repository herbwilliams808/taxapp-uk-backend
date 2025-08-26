using Swashbuckle.AspNetCore.Annotations;

namespace Core.Models.IndividualsEmploymentIncomes.OtherEmploymentIncome
{
    /// <summary>
    /// Represents details of foreign service income.
    /// </summary>
    public class ForeignService
    {
        /// <summary>
        /// Customer reference associated with the foreign service income, if available.
        /// </summary>
        [SwaggerSchema("Customer reference associated with the foreign service income, if available.")]
        public string? CustomerReference { get; set; }

        /// <summary>
        /// Amount deducted for foreign service income.
        /// </summary>
        [SwaggerSchema("Amount deducted for foreign service income.")]
        public decimal AmountDeducted { get; set; }
    }
}