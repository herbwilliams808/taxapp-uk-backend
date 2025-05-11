using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class TaxEstimationService(ILogger<TaxEstimationService> logger)
    {
        private readonly ILogger<TaxEstimationService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public decimal CalculateTax(decimal totalIncome)
        {
            if (totalIncome <= 12570) // Personal Allowance threshold
            {
                return 0;
            }
            else if (totalIncome <= 50270) // Basic Rate threshold
            {
                return (totalIncome - 12570) * 0.2m;
            }
            else
            {
                decimal basicRateTax = (50270 - 12570) * 0.2m;
                decimal higherRateTax = (totalIncome - 50270) * 0.4m;
                return basicRateTax + higherRateTax;
            }
        }
    }
}