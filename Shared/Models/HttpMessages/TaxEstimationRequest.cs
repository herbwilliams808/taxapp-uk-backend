using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shared.Models.IndividualsReliefs.ForeignReliefs;
using Shared.Models.Incomes;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.HttpMessages
{
    public class TaxEstimationRequest
    {
        
        // Calculate default tax year ending
        private static readonly DateTime today = DateTime.UtcNow;
        private static readonly DateTime taxYearStart = new DateTime(today.Year, 4, 6); // UK tax year starts April 6
        private static readonly int defaultTaxYearEnding = today < taxYearStart ? today.Year - 1 : today.Year;
        
        [SwaggerSchema(
            Description = "The tax year ending date (e.g. 2024 for tax year 2023/24). If not specified, defaults to current tax year.",
            Format = "int32")]
        [Range(2000, 2100, ErrorMessage = "Tax year must be between 2000 and 2100")]
        [DisplayName("Tax Year Ending")]
        [DefaultValue(2025)]
        public int? TaxYearEnding { get; set; } = defaultTaxYearEnding;
        /// <summary>
        /// The region for tax estimation. Defaults to "england".
        /// </summary>
        [SwaggerSchema(Description = "The region for tax estimation. Defaults to 'england'.")]
        [DefaultValue("england")]
        public string Region { get; set; } = "england";

        public Incomes.Incomes? Incomes { get; set; }
        public IndividualsReliefs.IndividualsReliefs? IndividualsReliefs { get; set; }

        public ForeignReliefs? ForeignReliefs { get; set; }
        public OtherDeductions.OtherDeductions? OtherDeductions { get; set; }
        
        public IndividualsForeignIncome.IndividualsForeignIncome? IndividualsForeignIncome { get; set; }  
    }
}