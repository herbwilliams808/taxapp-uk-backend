using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.IndividualsEmploymentIncomes.OtherEmploymentIncome
{
    /// <summary>
    /// Represents details of a share option.
    /// </summary>
    public class ShareOption
    {
        /// <summary>
        /// Name of the employer providing the share option.
        /// </summary>
        [SwaggerSchema("Name of the employer providing the share option.")]
        public string EmployerName { get; set; } = default!;

        /// <summary>
        /// Reference of the employer, if available.
        /// </summary>
        [SwaggerSchema("Reference of the employer, if available.")]
        public string? EmployerRef { get; set; }

        /// <summary>
        /// Type of the scheme plan.
        /// </summary>
        [SwaggerSchema("Type of the scheme plan.")]
        public SchemePlanType SchemePlanType { get; set; } = default!;

        /// <summary>
        /// Date when the option was granted.
        /// </summary>
        [SwaggerSchema("Date when the option was granted.")]
        public DateTime DateOfOptionGrant { get; set; }

        /// <summary>
        /// Date of the event related to the share option.
        /// </summary>
        [SwaggerSchema("Date of the event related to the share option.")]
        public DateTime DateOfEvent { get; set; }

        /// <summary>
        /// Indicates whether the option was not exercised, but consideration was received.
        /// </summary>
        [SwaggerSchema("Indicates whether the option was not exercised, but consideration was received.")]
        public bool? OptionNotExercisedButConsiderationReceived { get; set; }

        /// <summary>
        /// Amount of consideration received.
        /// </summary>
        [SwaggerSchema("Amount of consideration received.")]
        public decimal AmountOfConsiderationReceived { get; set; }

        /// <summary>
        /// Number of shares acquired.
        /// </summary>
        [SwaggerSchema("Number of shares acquired.")]
        public int NoOfSharesAcquired { get; set; }

        /// <summary>
        /// Class of shares acquired, if available.
        /// </summary>
        [SwaggerSchema("Class of shares acquired, if available.")]
        public string? ClassOfSharesAcquired { get; set; }

        /// <summary>
        /// Exercise price of the share option.
        /// </summary>
        [SwaggerSchema("Exercise price of the share option.")]
        public decimal ExercisePrice { get; set; }

        /// <summary>
        /// Amount paid for the option.
        /// </summary>
        [SwaggerSchema("Amount paid for the option.")]
        public decimal AmountPaidForOption { get; set; }

        /// <summary>
        /// Market value of shares at the time of exercise.
        /// </summary>
        [SwaggerSchema("Market value of shares at the time of exercise.")]
        public decimal MarketValueOfSharesOnExcise { get; set; }

        /// <summary>
        /// Profit made on the option exercised.
        /// </summary>
        [SwaggerSchema("Profit made on the option exercised.")]
        public decimal ProfitOnOptionExercised { get; set; }

        /// <summary>
        /// Employer's National Insurance Contributions paid on the option.
        /// </summary>
        [SwaggerSchema("Employer's National Insurance Contributions paid on the option.")]
        public decimal EmployersNicPaid { get; set; }

        /// <summary>
        /// Taxable amount of the share option.
        /// </summary>
        [SwaggerSchema("Taxable amount of the share option.")]
        public decimal TaxableAmount { get; set; }
    }
}
