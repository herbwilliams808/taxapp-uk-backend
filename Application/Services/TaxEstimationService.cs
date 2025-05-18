using Application.Calculators;
using Microsoft.Extensions.Logging;
using Shared.Models;
using Shared.Models.Reliefs;
using Shared.Models.Incomes;

namespace Application.Services
{
    public class TaxEstimationService(AzureBlobTaxRatesService taxRatesService, ILogger<TaxEstimationService> logger)
    {
        private readonly ILogger<TaxEstimationService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public TaxEstimationResponse CalculateTax(Incomes incomes, int taxYearEnding, string region, Reliefs reliefs)
        {
            // Load tax rates for the specified year and region
            var taxRates = taxRatesService.LoadTaxRatesAsync().Result;

            var key = $"year_{taxYearEnding - 1}_{taxYearEnding}.{region}.basicRateThreshold";
            var basicRateThreshold = taxRates.TryGetValue(key, out var threshold) ? threshold : 0m;

            var employmentIncomes = incomes.Employments.Select(employment => employment.Pay.TaxablePayToDate ?? 0m);
            var totalEmploymentIncome = new TotalEmploymentIncomeCalculator().Calculate(employmentIncomes);
            
            var nonPayeEmploymentIncome = incomes.NonPayeEmploymentIncome.Tips ?? 0m;
            
            // Calculate profit from properties
            var profitFromProperties = new ProfitFromPropertiesCalculator().Calculate(incomes);
            
            // Use the TotalIncomeCalculator
            var totalIncome = new TotalIncomeCalculator().Calculate(totalEmploymentIncome, nonPayeEmploymentIncome, profitFromProperties);

            // Use the BasicRateLimitCalculator
            // Check if contributions are null and use 0 if they are.
            var regularPensionContributions = reliefs.PensionReliefs?.RegularPensionContributions ?? 0m;
            var giftAidCurrentYear = reliefs.GiftAidPayments?.CurrentYear ?? 0m;
            var giftAidCurrentYearTreatedAsPrevious = reliefs.GiftAidPayments?.CurrentYearTreatedAsPreviousYear ?? 0m;
            var giftAidNextYearTreatedAsCurrent = reliefs.GiftAidPayments?.NextYearTreatedAsCurrentYear ?? 0m;

            var giftAidPayments = (giftAidCurrentYear - giftAidCurrentYearTreatedAsPrevious)
                                  + giftAidNextYearTreatedAsCurrent;
            var paymentsIntoPensions = regularPensionContributions;
            
            var basicRateLimit = new BasicRateLimitCalculator().Calculate(
                basicRateThreshold, 
                paymentsIntoPensions, 
                giftAidPayments
                );

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
                NetIncome = totalIncome - taxOwed - incomes.Employments.Sum(e => e.Pay.TotalTaxToDate ?? 0m)
            };
        }
    }
}
