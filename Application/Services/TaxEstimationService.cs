using Application.Calculators;
using Microsoft.Extensions.Logging;
using Shared.Models.HttpMessages;
using Shared.Models.Incomes;
using Shared.Models.IndividualsForeignIncome;
using Shared.Models.IndividualsReliefs;
using Shared.Models.IndividualsReliefs.ForeignReliefs;
using Shared.Models.OtherDeductions;

namespace Application.Services;

public class TaxEstimationService(
    TaxRatesCacheService taxRatesCacheService,
    ILogger<TaxEstimationService> logger)
{
    private readonly TaxRatesCacheService _taxRatesCacheService = taxRatesCacheService 
                                                                  ?? throw new ArgumentNullException(nameof(taxRatesCacheService));
    private readonly ILogger<TaxEstimationService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Task<TaxEstimationResponse> CalculateTaxAsync(
        int taxYearEnding,
        string region,
        IncomeSources? incomeSources,
        IndividualsReliefsDetails? individualsReliefs,
        OtherDeductionsDetails? otherDeductions,
        IndividualsForeignIncomeDetails? individualsForeignIncome,
        ForeignReliefsDetails? foreignReliefs)
    {
        // Because GetDecimalTaxRateValue is synchronous, no need to await here.
        decimal basicRateThreshold = _taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, region, "basicRateThreshold");

        var totalEmploymentIncome = new TotalEmploymentIncomeCalculator().Calculate(incomeSources, individualsForeignIncome, foreignReliefs);
        var totalBenefitsInKind = new TotalEmploymentBenefitsCalculator().Calculate(incomeSources);
        var totalEmploymentExpenses = new TotalEmploymentExpensesCalculator().Calculate(incomeSources);
        var totalOtherDeductions = new TotalOtherDeductionsCalculator().Calculate(otherDeductions);
        var profitFromProperties = new ProfitFromPropertiesCalculator().Calculate(incomeSources);

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
                        (incomeSources?.EmploymentsAndFinancialDetails?.Sum(e => e.Pay?.TotalTaxToDate ?? 0m) ?? 0m);

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