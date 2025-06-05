namespace Shared.Models.PropertyBusiness.PropertyBusinessAnnualSubmission.UkProperty
{
    /// <summary>
    /// Details about structured building allowance.
    /// </summary>
    public class UkPropertyStructuredBuildingAllowance
    {
        /// <summary>
        /// The amount of structured building allowance.
        /// Must be between 0 and 99999999999.99 up to 2 decimal places.
        /// </summary>
        public decimal Amount { get; init; }

        /// <summary>
        /// Object holding UK Non FHL structured building allowance details.
        /// </summary>
        public UkPropertyStructuredBuildingAllowanceFirstYear? FirstYear { get; init; }

        /// <summary>
        /// Object holding UK Non FHL structured building details.
        /// Postcode is mandatory and minimum one of name and number field must be supplied.
        /// </summary>
        public UkPropertyStructuredBuildingAllowanceBuilding Building { get; init; } = null!;
    }
}
