using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.IndividualsEmploymentIncomes.OtherEmploymentIncome.LumpSum
{
    /// <summary>
    /// Represents taxable lump sums and certain income details.
    /// </summary>
    public class TaxableLumpSumsAndCertainIncome
    {
        /// <summary>
        /// Total amount of the taxable lump sums and certain income.
        /// </summary>
        [SwaggerSchema("Total amount of the taxable lump sums and certain income.")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Tax paid on the taxable lump sums and certain income, if applicable.
        /// </summary>
        [SwaggerSchema("Tax paid on the taxable lump sums and certain income, if applicable.")]
        public decimal? TaxPaid { get; set; }

        /// <summary>
        /// Indicates whether tax was taken off in employment, if applicable.
        /// </summary>
        [SwaggerSchema("Indicates whether tax was taken off in employment, if applicable.")]
        public bool? TaxTakenOffInEmployment { get; set; }
    }
}