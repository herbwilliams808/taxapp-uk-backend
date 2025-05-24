using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.IndividualsForeignIncome;

/// <summary>
/// Represents foreign earnings, including a customer reference and the amount of earnings not taxable in the UK.
/// </summary>
public class ForeignEarnings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForeignEarnings"/> class.
    /// </summary>
    /// <param name="earningsNotTaxableUk">The amount of earnings not taxable in the UK.</param>
    public ForeignEarnings(decimal earningsNotTaxableUk)
    {
        EarningsNotTaxableUK = earningsNotTaxableUk;
    }

    /// <summary>
    /// A reference provided by the customer (if available).
    /// </summary>
    [SwaggerSchema(Description = "A reference provided by the customer (if available).")]
    [JsonPropertyName("customerReference")]
    [StringLength(100, ErrorMessage = "Customer reference must not exceed 100 characters.")]
    public string? CustomerReference { get; init; }

    /// <summary>
    /// The amount of earnings not taxable in the UK.
    /// </summary>
    [SwaggerSchema(Description = "The amount of earnings not taxable in the UK.")]
    [JsonPropertyName("earningsNotTaxableUK")]
    [Required(ErrorMessage = "Earnings not taxable in the UK is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Earnings not taxable in the UK must be a non-negative value.")]
    public decimal EarningsNotTaxableUK { get; init; }
}