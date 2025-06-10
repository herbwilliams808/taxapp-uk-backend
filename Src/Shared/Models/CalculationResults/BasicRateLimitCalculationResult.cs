namespace Shared.Models.CalculationResults;

/// <summary>
/// Represents the result of a basic rate limit calculation process.
/// </summary>
public class BasicRateLimitCalculationResult
{
    public decimal BasicRateLimitValue { get; set; }
    public decimal BasicRateLimitExtendedByAmount { get; set; }
    public decimal GrossPensionContributionsValue { get; set; }
    public decimal GrossGiftAidPaymentsValue { get; set; }
    public string? Message { get; set; }
    public required string CurrencyCode { get; set; }
}