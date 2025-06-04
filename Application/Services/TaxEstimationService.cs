using Application.Calculators;
using Microsoft.Extensions.Logging;
using Shared.Models.HttpMessages;
using Shared.Models.Incomes;
using Shared.Models.IndividualsForeignIncome;
using Shared.Models.IndividualsReliefs;
using Shared.Models.IndividualsReliefs.ForeignReliefs;
using Shared.Models.OtherDeductions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TaxEstimationService
    {
        private readonly TaxRatesCacheService _taxRatesCacheService;
        private readonly ILogger<TaxEstimationService> _logger;

        public TaxEstimationService(
            TaxRatesCacheService taxRatesCacheService,
            ILogger<TaxEstimationService> logger)
        {
            _taxRatesCacheService = taxRatesCacheService 
                ?? throw new ArgumentNullException(nameof(taxRatesCacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<TaxEstimationResponse> CalculateTaxAsync(
            int taxYearEnding,
            string region,
            IncomeDetails? incomes,
            IndividualsReliefsDetails? individualsReliefs,
            OtherDeductionsDetails? otherDeductions,
            IndividualsForeignIncomeDetails? individualsForeignIncome,
            ForeignReliefsDetails? foreignReliefs)
        {
            // Because GetDecimalTaxRateValue is synchronous, no need to await here.
            decimal basicRateThreshold = _taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, region, "basicRateThreshold");

            var totalEmploymentIncome = new TotalEmploymentIncomeCalculator().Calculate(incomes, individualsForeignIncome, foreignReliefs);
            var totalBenefitsInKind = new TotalEmploymentBenefitsCalculator().Calculate(incomes);
            var totalEmploymentExpenses = new TotalEmploymentExpensesCalculator().Calculate(incomes);
            var totalOtherDeductions = new TotalOtherDeductionsCalculator().Calculate(otherDeductions);
            var profitFromProperties = new ProfitFromPropertiesCalculator().Calculate(incomes);

            var totalIncome = new TotalIncomeCalculator().Calculate(
                totalEmploymentIncome,
                totalBenefitsInKind,
                totalEmploymentExpenses,
                totalOtherDeductions,
                profitFromProperties);

            var regularPensionContributions = individualsReliefs?.PensionReliefs?.RegularPensionContributions ?? 0m;
            var giftAidCurrentYear = individualsReliefs?.GiftAidPayments?.CurrentYear ?? 0m;
            var giftAidCurrentYearTreatedAsPrevious = individualsReliefs?.GiftAidPayments?.CurrentYearTreatedAsPreviousYear ?? 0m;
            var giftAidNextYearTreatedAsCurrent = individualsReliefs?.GiftAidPayments?.NextYearTreatedAsCurrentYear ?? 0m;

            var giftAidPayments = (giftAidCurrentYear - giftAidCurrentYearTreatedAsPrevious) + giftAidNextYearTreatedAsCurrent;
            var paymentsIntoPensions = regularPensionContributions;

            var basicRateLimit = new BasicRateLimitCalculator().Calculate(
                basicRateThreshold,
                paymentsIntoPensions,
                giftAidPayments);

            decimal taxOwed = 0;

            if (totalIncome > basicRateLimit.value)
            {
                taxOwed = (totalIncome - basicRateLimit.value) * 0.2m; // Example tax calculation
            }

            _logger.LogInformation("Tax calculated for year ending {TaxYearEnding}, region {Region}: {TaxOwed}",
                taxYearEnding, region, taxOwed);

            var netIncome = totalIncome - taxOwed -
                            (incomes?.EmploymentsAndFinancialDetails?.Sum(e => e.Pay.TotalTaxToDate ?? 0m) ?? 0m);

            var taxEstimationResponse = new TaxEstimationResponse
            {
                TotalIncome = totalIncome,
                BasicRateLimitExtendedMessage = basicRateLimit.message,
                BasicRateLimit = basicRateLimit.value,
                TaxOwed = taxOwed,
                NetIncome = netIncome
            };
            return Task.FromResult(taxEstimationResponse);
        }
    }
}
