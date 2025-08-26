using Swashbuckle.AspNetCore.Annotations;

namespace Core.Models.PropertyBusiness.PropertyBusinessAnnualSubmission.UkProperty;

/// <summary>
/// Represents allowances and adjustments for UK property business (excluding Furnished Holiday Lettings - FHL) for a given period.
/// </summary>
[SwaggerSchema(Description = "Represents allowances and adjustments for UK property business (excluding Furnished Holiday Lettings - FHL) for a given period.")]
public class UkPropertyDetails
{
    /// <summary>
    /// Details of annual adjustments for UK Non-FHL property business.
    /// </summary>
    /// <remarks>
    /// Example adjustments may include private use adjustments, balancing charges, or other specified changes.
    /// </remarks>
    [SwaggerSchema(Description = "Details of annual adjustments for UK Non-FHL property business.")]
    public UkPropertyAdjustments? Adjustments { get; set; }

    /// <summary>
    /// Details of annual allowances for UK Non-FHL property business.
    /// </summary>
    /// <remarks>
    /// Example allowances may include replacement furniture relief or capital allowances.
    /// </remarks>
    [SwaggerSchema(Description = "Details of annual allowances for UK Non-FHL property business.")]
    public UkPropertyAllowances? Allowances { get; set; }
}