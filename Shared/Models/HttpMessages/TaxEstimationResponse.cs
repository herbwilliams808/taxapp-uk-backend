namespace Shared.Models.HttpMessages
{
    public class TaxEstimationResponse
    {
        public decimal? PayFromAllEmployments { get; set; }
        public decimal? ProfitFromUkLandAndProperty { get; set; }
        
        public decimal TotalIncome { get; set; }
        public string? BasicRateLimitExtendedMessage { get; set; }
        public decimal BasicRateLimit { get; set; }
        public decimal TaxOwed { get; set; }
        public decimal NetIncome { get; set; }
    }
}