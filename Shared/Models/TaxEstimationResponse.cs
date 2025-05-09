namespace Shared.Models
{
    public class TaxEstimationResponse
    {
        public decimal TotalIncome { get; set; }
        public decimal TaxOwed { get; set; }
        public decimal NetIncome { get; set; }
    }
}