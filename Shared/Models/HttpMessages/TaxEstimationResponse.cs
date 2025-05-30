namespace Shared.Models.HttpMessages
{
    public class TaxEstimationResponse
    {
        public decimal TotalIncome { get; set; }
        public decimal BasicRateLimit { get; set; }
        public decimal TaxOwed { get; set; }
        public decimal NetIncome { get; set; }
    }
}