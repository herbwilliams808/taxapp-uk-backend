using Application.Calculators;
using Microsoft.Extensions.Logging;
using Shared.Models;
using Shared.Models.Contributions;
using Shared.Models.Incomes;

namespace Application.Services
{
    public class TaxEstimationService(AzureBlobTaxRatesService taxRatesService, ILogger<TaxEstimationService> logger)
    {
        private readonly ILogger<TaxEstimationService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public TaxEstimationResponse CalculateTax(Incomes incomes, int taxYearEnding, string region, Contributions contributions)
        {
            // Load tax rates for the specified year and region
            var taxRates = taxRatesService.LoadTaxRatesAsync().Result;

            var key = $"year_{taxYearEnding - 1}_{taxYearEnding}.{region}.basicRateThreshold";
            var basicRateThreshold = taxRates.TryGetValue(key, out var threshold) ? threshold : 0m;

            // Calculate profit from properties
            var profitFromProperties = (incomes.UkPropertyBusiness?.Income ?? 0m) - (incomes.UkPropertyBusiness?.AllowablePropertyLettingExpenses ?? 0m);

            // Use the TotalIncomeCalculator
            var totalIncome = new TotalIncomeCalculator().Calculate(incomes.Employment.Select(e => e.Income), profitFromProperties);

            // Use the BasicRateLimitCalculator
            // Check if contributions are null and use 0 if they are.
            var pensionContributions = contributions.TotalPensionContributions?.GrossedUpAmount ?? 0m;
            var giftAidContributions = contributions.TotalGiftAidPayments?.GrossedUpAmount ?? 0m;
            var basicRateLimit = new BasicRateLimitCalculator().Calculate(basicRateThreshold, pensionContributions, giftAidContributions);

            decimal taxOwed = 0;

            if (totalIncome > basicRateLimit)
            {
                taxOwed = (totalIncome - basicRateLimit) * 0.2m; // Example tax calculation
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
