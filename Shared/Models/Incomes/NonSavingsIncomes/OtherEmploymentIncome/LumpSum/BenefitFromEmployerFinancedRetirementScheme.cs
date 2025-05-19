using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.Incomes.NonSavingsIncomes.OtherEmploymentIncome.LumpSum
{
    /// <summary>
    /// Represents benefits from employer financed retirement schemes.
    /// </summary>
    public class BenefitFromEmployerFinancedRetirementScheme
    {
        /// <summary>
        /// Total amount of the benefit.
        /// </summary>
        [SwaggerSchema("Total amount of the benefit.")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Exempt amount of the benefit, if applicable.
        /// </summary>
        [SwaggerSchema("Exempt amount of the benefit, if applicable.")]
        public decimal? ExemptAmount { get; set; }

        /// <summary>
        /// Tax paid on the benefit, if applicable.
        /// </summary>
        [SwaggerSchema("Tax paid on the benefit, if applicable.")]
        public decimal? TaxPaid { get; set; }

        /// <summary>
        /// Indicates whether tax was taken off in employment, if applicable.
        /// </summary>
        [SwaggerSchema("Indicates whether tax was taken off in employment, if applicable.")]
        public bool? TaxTakenOffInEmployment { get; set; }
    }
}