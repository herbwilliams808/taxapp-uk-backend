using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.Incomes.NonSavingsIncomes.OtherEmploymentIncome
{
    /// <summary>
    /// Represents other employment income details.
    /// </summary>
    public class OtherEmploymentIncome
    {
        /// <summary>
        /// Date when the data was submitted.
        /// </summary>
        [SwaggerSchema("Date when the data was submitted.")]
        public DateTimeOffset? SubmittedOn { get; set; }

        /// <summary>
        /// Share options associated with the employment.
        /// </summary>
        [SwaggerSchema("Share options associated with the employment.")]
        public HashSet<ShareOption>? ShareOptions { get; set; }

        /// <summary>
        /// Shares awarded or received details.
        /// </summary>
        [SwaggerSchema("Shares awarded or received details.")]
        public HashSet<SharesAwardedOrReceived>? SharesAwardedOrReceived { get; set; }

        /// <summary>
        /// Lump sums associated with the employment.
        /// </summary>
        [SwaggerSchema("Lump sums associated with the employment.")]
        public HashSet<LumpSum.LumpSum>? LumpSums { get; set; }

        /// <summary>
        /// Disability related income details.
        /// </summary>
        [SwaggerSchema("Disability related income details.")]
        public Disability? Disability { get; set; }

        /// <summary>
        /// Foreign service related income details.
        /// </summary>
        [SwaggerSchema("Foreign service related income details.")]
        public ForeignService? ForeignService { get; set; }
    }
}