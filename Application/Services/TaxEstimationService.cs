using Microsoft.Extensions.Logging;
using Shared.Models;
using Shared.Models.Incomes;

namespace Application.Services
{
    public class TaxEstimationService(AzureBlobTaxRatesService taxRatesService, ILogger<TaxEstimationService> logger)
    {
        private readonly ILogger<TaxEstimationService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public TaxEstimationResponse CalculateTax(Incomes incomes, int taxYearEnding, string region)
        {
            // Load tax rates for the specified year and region
            var taxRates = taxRatesService.LoadTaxRatesAsync().Result;

            var key = $"year_{taxYearEnding - 1}_{taxYearEnding}.{region}.basicRateThreshold";
            var basicRateThreshold = taxRates.TryGetValue(key, out var threshold) ? threshold : 0;

            decimal totalIncome = incomes.Employment.Sum(e => e.Income) + incomes.UkPropertyBusiness.Income;
            decimal taxOwed = 0;

            if (totalIncome > basicRateThreshold)
            {
                taxOwed = (totalIncome - basicRateThreshold) * 0.2m; // Example tax calculation
            }

            _logger.LogInformation($"Tax calculated for year ending {taxYearEnding}, region {region}: {taxOwed}");

            return new TaxEstimationResponse
            {
                TotalIncome = totalIncome,
                TaxOwed = taxOwed,
                NetIncome = totalIncome - taxOwed - incomes.Employment.Sum(e => e.TaxPaid)
            };
        }
    }
}