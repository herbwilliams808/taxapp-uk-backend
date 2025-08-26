using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Core.Models.PropertyBusiness.PropertyBusinessAnnualSubmission.UkProperty;

/// <summary>
/// Represents adjustments applicable to UK Non FHL property annual submissions.
/// </summary>
[SwaggerSchema("Object holding UK Non FHL Property annual adjustments.")]
public class UkPropertyAdjustments
{
    /// <summary>
    /// Gets or sets the balancing charge.
    /// If an item for which capital allowance was claimed has been sold, given away, or is no longer in use.
    /// </summary>
    /// <example>5000.99</example>
    [SwaggerSchema("If an item for which capital allowance was claimed has been sold, given away or is no longer in use. The value must be between 0 and 99999999999.99 up to 2 decimal places.")]
    [Range(0, 99999999999.99)]
    public decimal? BalancingCharge { get; init; }

    /// <summary>
    /// Gets or sets the private use adjustment.
    /// Adjustments that are not solely for the property business.
    /// </summary>
    /// <example>5000.99</example>
    [SwaggerSchema("Any adjustments that are not solely for the property business. The value must be between 0 and 99999999999.99 up to 2 decimal places.")]
    [Range(0, 99999999999.99)]
    public decimal? PrivateUseAdjustment { get; init; }

    /// <summary>
    /// Gets or sets the business premises renovation allowance balancing charges.
    /// Income from the sale or grant of a long lease for a premium of renovated business premises within 7 years of first use.
    /// </summary>
    /// <example>5000.99</example>
    [SwaggerSchema("Income from the sale or grant of a long lease for a premium of renovated business premises within 7 years of first use. The value must be between 0 and 99999999999.99 up to 2 decimal places.")]
    [Range(0, 99999999999.99)]
    public decimal? BusinessPremisesRenovationAllowanceBalancingCharges { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the user is a non-resident landlord.
    /// </summary>
    /// <example>false</example>
    [SwaggerSchema("A boolean to identify that the user is a Non-Resident Landlord. The value must be true or false.")]
    [Required]
    public bool NonResidentLandlord { get; init; }

    /// <summary>
    /// Gets or sets the rent-a-room income adjustments.
    /// Object holding UK Non FHL Rent A Room income shared details.
    /// </summary>
    [SwaggerSchema("Object holding UK Non FHL Rent A Room income shared.")]
    public UkPropertyRentARoom? RentARoom { get; init; }
}