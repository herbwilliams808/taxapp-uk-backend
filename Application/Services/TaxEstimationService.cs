using Application.Calculators;
using Microsoft.Extensions.Logging;
using Shared.Models;
using Shared.Models.HttpMessages;
using Shared.Models.Incomes;
using Shared.Models.IndividualsForeignIncome;
using Shared.Models.IndividualsReliefs;
using Shared.Models.IndividualsReliefs.ForeignReliefs;
using Shared.Models.OtherDeductions;

namespace Application.Services
{
    public class TaxEstimationService(AzureBlobTaxRatesService taxRatesService, ILogger<TaxEstimationService> logger)
    {
        private readonly ILogger<TaxEstimationService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public TaxEstimationResponse CalculateTax(
            int taxYearEnding, 
            string region, 
            Incomes? incomes,
            IndividualsReliefs? individualsReliefs,
            OtherDeductions? otherDeductions, 
            IndividualsForeignIncome? individualsForeignIncome,
            ForeignReliefs? foreignReliefs
        )
        {
            // Load tax rates for the specified year and region
            var taxRates = taxRatesService.LoadTaxRatesAsync().Result;

            var key = $"year_{taxYearEnding - 1}_{taxYearEnding}.{region}.basicRateThreshold";
            var basicRateThreshold = taxRates.TryGetValue(key, out var threshold) ? threshold : 0m;

            // Use the TotalIncomeCalculator
            var totalEmploymentIncome = new TotalEmploymentIncomeCalculator(new TotalEmploymentBenefitsCalculator(), new TotalEmploymentExpensesCalculator(), new TotalOtherDeductionsCalculator()).Calculate(incomes, otherDeductions, individualsForeignIncome, foreignReliefs );
            var nonPayeEmploymentIncome = incomes.NonPayeEmploymentIncome.Tips ?? 0m;
            var profitFromProperties = new ProfitFromPropertiesCalculator().Calculate(incomes);
            var totalIncome = new TotalIncomeCalculator().Calculate(totalEmploymentIncome, nonPayeEmploymentIncome, profitFromProperties);

            // Use the BasicRateLimitCalculator
            // Check if contributions are null and use 0 if they are.
            var regularPensionContributions = individualsReliefs.PensionReliefs?.RegularPensionContributions ?? 0m;
            var giftAidCurrentYear = individualsReliefs.GiftAidPayments?.CurrentYear ?? 0m;
            var giftAidCurrentYearTreatedAsPrevious = individualsReliefs.GiftAidPayments?.CurrentYearTreatedAsPreviousYear ?? 0m;
            var giftAidNextYearTreatedAsCurrent = individualsReliefs.GiftAidPayments?.NextYearTreatedAsCurrentYear ?? 0m;

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