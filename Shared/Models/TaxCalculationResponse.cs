namespace Shared.Models
{
    public class TaxCalculationResponse
    {
        public decimal TotalIncome { get; set; }
        public decimal TaxOwed { get; set; }
        public decimal NetIncome { get; set; }
    }
}