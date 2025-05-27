using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.IndividualsEmploymentIncomes.OtherEmploymentIncome.LumpSum
{
    /// <summary>
    /// Represents a lump sum associated with employment.
    /// </summary>
    public class LumpSum
    {
        /// <summary>
        /// Name of the employer providing the lump sum.
        /// </summary>
        [SwaggerSchema("Name of the employer providing the lump sum.")]
        public string EmployerName { get; set; } = null!;

        /// <summary>
        /// Reference ID of the employer.
        /// </summary>
        [SwaggerSchema("Reference ID of the employer.")]
        public string EmployerRef { get; set; } = null!;

        /// <summary>
        /// Taxable lump sums and certain income details.
        /// </summary>
        [SwaggerSchema("Taxable lump sums and certain income details.")]
        public TaxableLumpSumsAndCertainIncome? TaxableLumpSumsAndCertainIncome { get; set; }

        /// <summary>
        /// Details of benefits from employer-financed retirement schemes.
        /// </summary>
        [SwaggerSchema("Details of benefits from employer-financed retirement schemes.")]
        public BenefitFromEmployerFinancedRetirementScheme? BenefitFromEmployerFinancedRetirementScheme { get; set; }

        /// <summary>
        /// Details of redundancy compensation payments over exemption.
        /// </summary>
        [SwaggerSchema("Details of redundancy compensation payments over exemption.")]
        public RedundancyCompensationPaymentsOverExemption? RedundancyCompensationPaymentsOverExemption { get; set; }

        /// <summary>
        /// Details of redundancy compensation payments under exemption.
        /// </summary>
        [SwaggerSchema("Details of redundancy compensation payments under exemption.")]
        public RedundancyCompensationPaymentsUnderExemption? RedundancyCompensationPaymentsUnderExemption { get; set; }
    }
}