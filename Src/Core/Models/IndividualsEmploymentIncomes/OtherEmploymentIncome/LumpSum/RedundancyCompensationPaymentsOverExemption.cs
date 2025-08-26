using Swashbuckle.AspNetCore.Annotations;

namespace Core.Models.IndividualsEmploymentIncomes.OtherEmploymentIncome.LumpSum
{
    /// <summary>
    /// Represents redundancy compensation payments over the exemption limit.
    /// </summary>
    public class RedundancyCompensationPaymentsOverExemption
    {
        /// <summary>
        /// Amount of redundancy compensation payments over exemption.
        /// </summary>
        [SwaggerSchema("Amount of redundancy compensation payments over exemption.")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Tax paid on the redundancy compensation payments, if applicable.
        /// </summary>
        [SwaggerSchema("Tax paid on the redundancy compensation payments, if applicable.")]
        public decimal? TaxPaid { get; set; }

        /// <summary>
        /// Indicates whether tax was taken off in employment, if applicable.
        /// </summary>
        [SwaggerSchema("Indicates whether tax was taken off in employment, if applicable.")]
        public bool? TaxTakenOffInEmployment { get; set; }
    }
}