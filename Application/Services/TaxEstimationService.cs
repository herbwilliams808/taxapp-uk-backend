using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class TaxEstimationService
    {
        private readonly ILogger<TaxEstimationService> _logger; // Added ILogger

        public TaxEstimationService(ILogger<TaxEstimationService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Initialize logger
            _logger.LogInformation("*_*_*_*_*_*_ Logger initialised");
            
        }
        public decimal CalculateTax(decimal totalIncome)
        {
            _logger.LogInformation("Starting to calculate tax...");

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