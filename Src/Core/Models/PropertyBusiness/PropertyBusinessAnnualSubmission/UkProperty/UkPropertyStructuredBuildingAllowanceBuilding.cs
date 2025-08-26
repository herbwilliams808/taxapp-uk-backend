namespace Core.Models.PropertyBusiness.PropertyBusinessAnnualSubmission.UkProperty;

/// <summary>
/// Object holding UK Non FHL structured building details.
/// Postcode is mandatory and minimum one of name and number field must be supplied.
/// </summary>
public class UkPropertyStructuredBuildingAllowanceBuilding
{
    /// <summary>
    /// The name of the building.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// The number of the building.
    /// </summary>
    public string? Number { get; init; }

    /// <summary>
    /// The postcode for the building.
    /// </summary>
    public string Postcode { get; init; } = null!;
}