using Swashbuckle.AspNetCore.Annotations;

namespace Core.Models.IndividualsEmploymentIncomes.OtherEmploymentIncome.LumpSum
{
    /// <summary>
    /// Represents redundancy compensation payments under the exemption limit.
    /// </summary>
    public class RedundancyCompensationPaymentsUnderExemption
    {
        /// <summary>
        /// Amount of redundancy compensation payments under exemption.
        /// </summary>
        [SwaggerSchema("Amount of redundancy compensation payments under exemption.")]
        public decimal Amount { get; set; }
    }
}