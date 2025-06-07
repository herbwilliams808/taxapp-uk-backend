using Application.Interfaces.Calculators;
using Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Shared.Models.HttpMessages;
using Shared.Models.Incomes;
using Shared.Models.IndividualsForeignIncome;
using Shared.Models.IndividualsReliefs;
using Shared.Models.IndividualsReliefs.ForeignReliefs;
using Shared.Models.OtherDeductions;

namespace Application.Services;

public class TaxEstimationService(
    ITaxRatesCacheService taxRatesCacheService,
    ILogger<TaxEstimationService> logger,
    ITotalEmploymentIncomeCalculator totalEmploymentIncomeCalculator,
    ITotalEmploymentBenefitsCalculator totalEmploymentBenefitsCalculator,
    ITotalEmploymentExpensesCalculator totalEmploymentExpensesCalculator,
    ITotalOtherDeductionsCalculator totalOtherDeductionsCalculator,
    IProfitFromPropertiesCalculator profitFromPropertiesCalculator,
    ITotalIncomeCalculator totalIncomeCalculator,
    IGiftAidPaymentsCalculator giftAidPaymentsCalculator,
    IBasicRateLimitCalculator basicRateLimitCalculator)
{
    public Task<TaxEstimationResponse> CalculateTaxAsync(
        int taxYearEnding,
        string region,
        IncomeSourcesDetails? incomeSources,
        IndividualsReliefsDetails? individualsReliefs,
        OtherDeductionsDetails? otherDeductions,
        IndividualsForeignIncomeDetails? individualsForeignIncome,
        ForeignReliefsDetails? foreignReliefs)
    {
        var basicRateThreshold = taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, region, "basicRateThreshold");
        var totalEmploymentIncome = totalEmploymentIncomeCalculator.Calculate(incomeSources, individualsForeignIncome, foreignReliefs);
        var totalBenefitsInKind = totalEmploymentBenefitsCalculator.Calculate(incomeSources);
        var totalEmploymentExpenses = totalEmploymentExpensesCalculator.Calculate(incomeSources);
        var totalOtherDeductions = totalOtherDeductionsCalculator.Calculate(otherDeductions);
        var profitFromProperties = profitFromPropertiesCalculator.Calculate(incomeSources);

        var totalIncome = totalIncomeCalculator.Calculate(
            totalEmploymentIncome,
            totalBenefitsInKind,
            totalEmploymentExpenses,
            totalOtherDeductions,
            profitFromProperties);

        var regularPensionContributions = individualsReliefs?.PensionReliefs?.RegularPensionContributions ?? 0m;
        var giftAidPayments = giftAidPaymentsCalculator.Calculate(individualsReliefs);
        var basicRateLimit = basicRateLimitCalculator.Calculate(
            basicRateThreshold,
            regularPensionContributions,
            giftAidPayments);

        decimal taxOwed = 0;

        if (totalIncome > basicRateLimit.Value)
        {
            taxOwed = (totalIncome - basicRateLimit.Value) * 0.2m; // Example tax calculation
        }

        // CHANGED: Access parameter directly
        logger.LogInformation("Tax calculated for year ending {TaxYearEnding}, region {Region}: {TaxOwed}",
            taxYearEnding, region, taxOwed);

        var netIncome = totalIncome - taxOwed -
                        (incomeSources?.EmploymentsAndFinancialDetails?.Sum(e => e.Pay?.TotalTaxToDate ?? 0m) ?? 0m);

        var taxEstimationResponse = new TaxEstimationResponse
        {
            PayFromAllEmployments = totalEmploymentIncome,
            ProfitFromUkLandAndProperty = profitFromProperties,
            TotalIncome = totalIncome,
            BasicRateLimitExtendedMessage = basicRateLimit.Message,
            BasicRateLimit = basicRateLimit.Value,
            TaxOwed = taxOwed,
            NetIncome = netIncome
        };
        return Task.FromResult(taxEstimationResponse);
    }
}