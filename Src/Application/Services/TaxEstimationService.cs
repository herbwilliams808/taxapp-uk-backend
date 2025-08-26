using Core.Interfaces.Calculators;
using Core.Interfaces.Services;
using Core.Models.HttpMessages;
using Core.Models.Incomes;
using Core.Models.IndividualsForeignIncome;
using Core.Models.IndividualsReliefs;
using Core.Models.IndividualsReliefs.ForeignReliefs;
using Core.Models.OtherDeductions;
using Microsoft.Extensions.Logging;

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
    IAdjustedNetIncomeCalculator adjustedNetIncomeCalculator,
    IPersonalAllowanceCalculator personalAllowanceCalculator,
    IGiftAidPaymentsCalculator giftAidPaymentsCalculator,
    IBasicRateLimitCalculator basicRateLimitCalculator,
    ITaxOwedCalculator taxOwedCalculator)
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
        
        var adjustedNetIncome = adjustedNetIncomeCalculator.Calculate(totalIncome, regularPensionContributions, giftAidPayments);
        
        var standardPersonalAllowance = taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, region, "personalAllowance");
        var personalAllowanceIncomeLimit = taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, region, "personalAllowanceIncomeLimit");
        
        var personalAllowance = personalAllowanceCalculator.Calculate(
            standardPersonalAllowance,
            adjustedNetIncome,
            personalAllowanceIncomeLimit);
        
        var taxableIncome = totalIncome - personalAllowance;

        var basicRateLimit = basicRateLimitCalculator.Calculate(
            basicRateThreshold,
            regularPensionContributions,
            giftAidPayments);

        var taxOwed = taxOwedCalculator.Calculate(totalIncome, basicRateLimit);

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
            PersonalAllowance = personalAllowance,
            TaxableIncome = taxableIncome,
            BasicRateLimitDetails = basicRateLimit,
            TaxOwed = taxOwed,
            NetIncome = netIncome
        };
        return Task.FromResult(taxEstimationResponse);
    }
}