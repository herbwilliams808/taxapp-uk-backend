namespace Shared.Models.PropertyBusiness.PropertyBusinessAnnualSubmission.UkProperty;

/// <summary>
/// Object holding UK Non FHL enhanced structured building allowance first year details.
/// </summary>
public class UkPropertyEnhancedStructuredBuildingAllowanceFirstYear
{
    /// <summary>
    /// The date qualified for enhanced structured building allowance. Must be YYYY-MM-DD.
    /// </summary>
    public DateTime QualifyingDate { get; init; }

    /// <summary>
    /// The amount of qualifying expenditure.
    /// Must be between 0 and 99999999999.99 up to 2 decimal places.
    /// </summary>
    public decimal QualifyingAmountExpenditure { get; init; }
}