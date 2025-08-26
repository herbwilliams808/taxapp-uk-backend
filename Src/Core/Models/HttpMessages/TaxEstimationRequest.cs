using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Core.Models.Incomes;
using Core.Models.IndividualsForeignIncome;
using Core.Models.IndividualsReliefs;
using Core.Models.IndividualsReliefs.ForeignReliefs;
using Core.Models.OtherDeductions;
using Swashbuckle.AspNetCore.Annotations;

namespace Core.Models.HttpMessages
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

        public IncomeSourcesDetails? IncomeSources { get; set; }
        public IndividualsReliefsDetails? IndividualsReliefs { get; set; }

        public ForeignReliefsDetails? ForeignReliefs { get; set; }
        public OtherDeductionsDetails? OtherDeductions { get; set; }
        
        public IndividualsForeignIncomeDetails? IndividualsForeignIncome { get; set; }  
    }
}