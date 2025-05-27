using System;

namespace Shared.Models.PropertyBusiness.PropertyBusinessAnnualSubmission.UkProperty
{
    /// <summary>
    /// Details about enhanced structured building allowance.
    /// </summary>
    public class UkPropertyEnhancedStructuredBuildingAllowance
    {
        /// <summary>
        /// The amount of enhanced structured building allowance.
        /// Must be between 0 and 99999999999.99 up to 2 decimal places.
        /// </summary>
        public decimal Amount { get; init; }

        /// <summary>
        /// Object holding UK Non FHL enhanced structured building allowance details.
        /// </summary>
        public UkPropertyEnhancedStructuredBuildingAllowanceFirstYear? FirstYear { get; init; }

        /// <summary>
        /// Object holding UK Non FHL enhanced structured building details.
        /// Postcode is mandatory and minimum one of name and number field must be supplied.
        /// </summary>
        public UkPropertyEnhancedStructuredBuildingAllowanceBuilding Building { get; init; } = null!;
    }
}
