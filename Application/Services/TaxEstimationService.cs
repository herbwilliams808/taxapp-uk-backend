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
    ILogger<TaxEstimationService> logger) : GiftAidPaymentsCalculator
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
        var basicRateThreshold = _taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, region, "basicRateThreshold");

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
        var giftAidPayments = Calculate(individualsReliefs);
        var basicRateLimit = new BasicRateLimitCalculator().Calculate(
            basicRateThreshold,
            regularPensionContributions,
            giftAidPayments);

        decimal taxOwed = 0;

        if (totalIncome > basicRateLimit.Value)
        {
            taxOwed = (totalIncome - basicRateLimit.Value) * 0.2m; // Example tax calculation
        }

        _logger.LogInformation("Tax calculated for year ending {TaxYearEnding}, region {Region}: {TaxOwed}",
            taxYearEnding, region, taxOwed);

        var netIncome = totalIncome - taxOwed -
                        (incomeSources?.EmploymentsAndFinancialDetails?.Sum(e => e.Pay?.TotalTaxToDate ?? 0m) ?? 0m);

        var taxEstimationResponse = new TaxEstimationResponse
        {
            TotalIncome = totalIncome,
            BasicRateLimitExtendedMessage = basicRateLimit.Message,
            BasicRateLimit = basicRateLimit.Value,
            TaxOwed = taxOwed,
            NetIncome = netIncome
        };
        return Task.FromResult(taxEstimationResponse);
    }
}