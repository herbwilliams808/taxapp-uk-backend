using Core.Interfaces.Calculators;
using Core.Interfaces.Services;
using Core.Models.CalculationResults;
using Microsoft.Extensions.Logging;

namespace Application.Calculators;

public class TaxOwedCalculator : ITaxOwedCalculator
{
    private readonly ITaxRatesCacheService _taxRatesCacheService;
    private readonly ILogger<TaxOwedCalculator> _logger;

    public TaxOwedCalculator(ITaxRatesCacheService taxRatesCacheService, ILogger<TaxOwedCalculator> logger)
    {
        _taxRatesCacheService = taxRatesCacheService ?? throw new ArgumentNullException(nameof(taxRatesCacheService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Legacy method for backward compatibility - uses default rates
    /// </summary>
    public decimal Calculate(decimal totalIncome, BasicRateLimitCalculationResult basicRateLimit)
    {
        totalIncome = Math.Round(totalIncome, 0, MidpointRounding.ToNegativeInfinity);
        basicRateLimit.BasicRateLimitValue = Math.Round(basicRateLimit.BasicRateLimitValue, 0, MidpointRounding.ToPositiveInfinity);
        
        _logger.LogWarning("Using legacy TaxOwedCalculator method with hardcoded rates. Consider using the overload with taxYearEnding and region parameters.");
        
        decimal taxOwed = 0;
        if (totalIncome > basicRateLimit.BasicRateLimitValue)
        {
            // Legacy behavior - hardcoded 20% above basic rate limit
            taxOwed = (totalIncome - basicRateLimit.BasicRateLimitValue) * 0.2m;
        }

        return Math.Round(taxOwed, 0, MidpointRounding.ToNegativeInfinity);
    }

    /// <summary>
    /// Calculates tax owed using proper UK tax bands based on region and tax year
    /// </summary>
    public decimal Calculate(decimal totalIncome, BasicRateLimitCalculationResult basicRateLimit, int taxYearEnding, string region)
    {
        // Round income appropriately
        totalIncome = Math.Round(totalIncome, 0, MidpointRounding.ToNegativeInfinity);
        var basicRateLimit_Value = Math.Round(basicRateLimit.BasicRateLimitValue, 0, MidpointRounding.ToPositiveInfinity);

        _logger.LogInformation("Calculating tax for income {Income} in region {Region} for tax year ending {TaxYear}", 
            totalIncome, region, taxYearEnding);

        // If total income is below the basic rate limit, no tax is owed
        if (totalIncome <= basicRateLimit_Value)
        {
            _logger.LogInformation("Income {Income} is below basic rate limit {BasicRateLimit}, no tax owed", 
                totalIncome, basicRateLimit_Value);
            return 0m;
        }

        decimal totalTaxOwed = 0m;

        try
        {
            if (region.Equals("scotland", StringComparison.OrdinalIgnoreCase))
            {
                totalTaxOwed = CalculateScottishTax(totalIncome, basicRateLimit_Value, taxYearEnding);
            }
            else
            {
                // England, Wales, Northern Ireland use the same rates
                totalTaxOwed = CalculateStandardUkTax(totalIncome, basicRateLimit_Value, taxYearEnding, region);
            }

            _logger.LogInformation("Total tax owed: {TaxOwed} for income {Income} in region {Region}", 
                totalTaxOwed, totalIncome, region);

            return Math.Round(totalTaxOwed, 0, MidpointRounding.ToNegativeInfinity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating tax for income {Income} in region {Region} for tax year {TaxYear}", 
                totalIncome, region, taxYearEnding);
            throw;
        }
    }

    private decimal CalculateStandardUkTax(decimal totalIncome, decimal basicRateLimit_Value, int taxYearEnding, string region)
    {
        // Get tax rates from cache
        var basicRatePercentage = _taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, region, "basicRatePercentage") / 100m;
        var higherRatePercentage = _taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, region, "higherRatePercentage") / 100m;
        var additionalRatePercentage = _taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, region, "additionalRatePercentage") / 100m;
        var higherRateThreshold = _taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, region, "higherRateThreshold");

        _logger.LogInformation("Using standard UK tax rates - Basic: {Basic}%, Higher: {Higher}%, Additional: {Additional}%", 
            basicRatePercentage * 100, higherRatePercentage * 100, additionalRatePercentage * 100);

        decimal totalTaxOwed = 0m;
        decimal remainingIncome = totalIncome - basicRateLimit_Value; // Income above basic rate limit

        // Higher rate band (40%) - from basic rate limit to higher rate threshold
        var higherRateBandLimit = Math.Max(0, higherRateThreshold - basicRateLimit_Value);
        if (remainingIncome > 0 && higherRateBandLimit > 0)
        {
            var higherRateIncome = Math.Min(remainingIncome, higherRateBandLimit);
            var higherRateTax = higherRateIncome * higherRatePercentage;
            totalTaxOwed += higherRateTax;
            remainingIncome -= higherRateIncome;

            _logger.LogInformation("Higher rate tax: {Tax} on income {Income} at rate {Rate}%", 
                higherRateTax, higherRateIncome, higherRatePercentage * 100);
        }

        // Additional rate band (45%) - above higher rate threshold
        if (remainingIncome > 0)
        {
            var additionalRateTax = remainingIncome * additionalRatePercentage;
            totalTaxOwed += additionalRateTax;

            _logger.LogInformation("Additional rate tax: {Tax} on income {Income} at rate {Rate}%", 
                additionalRateTax, remainingIncome, additionalRatePercentage * 100);
        }

        return totalTaxOwed;
    }

    private decimal CalculateScottishTax(decimal totalIncome, decimal basicRateLimit_Value, int taxYearEnding)
    {
        // Get Scottish tax rates from cache
        var starterRatePercentage = _taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, "scotland", "starterRatePercentage") / 100m;
        var basicRatePercentage = _taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, "scotland", "basicRatePercentage") / 100m;
        var intermediateRatePercentage = _taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, "scotland", "intermediateRatePercentage") / 100m;
        var higherRatePercentage = _taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, "scotland", "higherRatePercentage") / 100m;
        var topRatePercentage = _taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, "scotland", "topRatePercentage") / 100m;

        // Get Scottish tax bands
        var starterRateBand = _taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, "scotland", "starterRateBand");
        var basicRateBand = _taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, "scotland", "basicRateBand");
        var intermediateRateBand = _taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, "scotland", "intermediateRateBand");
        var higherRateBand = _taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, "scotland", "higherRateBand");
        var topRateBand = _taxRatesCacheService.GetDecimalTaxRateValue(taxYearEnding, "scotland", "topRateBand");

        _logger.LogInformation("Using Scottish tax rates - Starter: {Starter}%, Basic: {Basic}%, Intermediate: {Intermediate}%, Higher: {Higher}%, Top: {Top}%", 
            starterRatePercentage * 100, basicRatePercentage * 100, intermediateRatePercentage * 100, 
            higherRatePercentage * 100, topRatePercentage * 100);

        decimal totalTaxOwed = 0m;
        decimal remainingIncome = totalIncome - basicRateLimit_Value; // Income above basic rate limit

        // Scottish tax bands calculation
        var cumulativeThreshold = basicRateLimit_Value;

        // Intermediate rate band (21%)
        var intermediateRateLimit = Math.Max(0, intermediateRateBand);
        if (remainingIncome > 0 && intermediateRateLimit > 0)
        {
            var intermediateRateIncome = Math.Min(remainingIncome, intermediateRateLimit);
            var intermediateRateTax = intermediateRateIncome * intermediateRatePercentage;
            totalTaxOwed += intermediateRateTax;
            remainingIncome -= intermediateRateIncome;
            cumulativeThreshold += intermediateRateIncome;

            _logger.LogInformation("Scottish intermediate rate tax: {Tax} on income {Income} at rate {Rate}%", 
                intermediateRateTax, intermediateRateIncome, intermediateRatePercentage * 100);
        }

        // Higher rate band (42%)
        var higherRateLimit = Math.Max(0, higherRateBand);
        if (remainingIncome > 0 && higherRateLimit > 0)
        {
            var higherRateIncome = Math.Min(remainingIncome, higherRateLimit);
            var higherRateTax = higherRateIncome * higherRatePercentage;
            totalTaxOwed += higherRateTax;
            remainingIncome -= higherRateIncome;
            cumulativeThreshold += higherRateIncome;

            _logger.LogInformation("Scottish higher rate tax: {Tax} on income {Income} at rate {Rate}%", 
                higherRateTax, higherRateIncome, higherRatePercentage * 100);
        }

        // Top rate band (47%)
        if (remainingIncome > 0)
        {
            var topRateTax = remainingIncome * topRatePercentage;
            totalTaxOwed += topRateTax;

            _logger.LogInformation("Scottish top rate tax: {Tax} on income {Income} at rate {Rate}%", 
                topRateTax, remainingIncome, topRatePercentage * 100);
        }

        return totalTaxOwed;
    }
}