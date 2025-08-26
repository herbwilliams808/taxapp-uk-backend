namespace Core.Models.PropertyBusiness.PropertyBusinessAnnualSubmission.UkProperty;

/// <summary>
/// Object holding UK Non FHL structured building allowance first year details.
/// </summary>
public class UkPropertyStructuredBuildingAllowanceFirstYear
{
    /// <summary>
    /// The date qualified for structured building allowance. Must be YYYY-MM-DD.
    /// </summary>
    public DateTime QualifyingDate { get; init; }

    /// <summary>
    /// The amount of qualifying expenditure.
    /// Must be between 0 and 99999999999.99 up to 2 decimal places.
    /// </summary>
    public decimal QualifyingAmountExpenditure { get; init; }
}