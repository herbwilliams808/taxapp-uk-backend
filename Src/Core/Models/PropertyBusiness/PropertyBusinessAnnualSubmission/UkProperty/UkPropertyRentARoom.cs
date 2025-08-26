using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Core.Models.PropertyBusiness.PropertyBusinessAnnualSubmission.UkProperty;

/// <summary>
/// Represents Rent A Room income details for UK Non FHL property.
/// </summary>
[SwaggerSchema("Object holding UK Non FHL Rent A Room income shared.")]
public class UkPropertyRentARoom
{
    /// <summary>
    /// Gets or sets a value indicating whether the Rent A Room income (RAR) is shared with another individual.
    /// </summary>
    /// <example>true</example>
    [SwaggerSchema("A boolean to identify that the Rent A Room income (RAR) is shared with another individual. The value must be true or false.")]
    [Required]
    public bool JointlyLet { get; init; }
}