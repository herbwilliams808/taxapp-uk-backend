namespace Shared.Models
{
    public class TaxEstimationRequest
    {
        /// <summary>
        /// The tax year ending (4-digit numerical string). Optional. Defaults to the previous tax year based on the current date.
        /// </summary>
        public string? TaxYearEnding { get; set; }

        /// <summary>
        /// The region (e.g., "england", "scotland", "wales", "northern ireland"). Optional. Defaults to "england".
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// Detailed incomes, including employment and property business incomes. Required.
        /// </summary>
        public Incomes.Incomes Incomes { get; set; } = default!;
    }
    
}