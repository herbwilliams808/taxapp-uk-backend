using Core.Models.CalculationResults;

namespace Core.Models.HttpMessages
{
    public class TaxEstimationResponse
    {
        public decimal? PayFromAllEmployments { get; set; }
        public decimal? ProfitFromUkLandAndProperty { get; set; }
        
        public decimal TotalIncome { get; set; }
        public decimal PersonalAllowance { get; set; }
        public decimal TaxableIncome { get; set; }
        public required BasicRateLimitCalculationResult BasicRateLimitDetails { get; set; }
        public decimal TaxOwed { get; set; }
        public decimal NetIncome { get; set; }
    }
}