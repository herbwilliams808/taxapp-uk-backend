using System.ComponentModel.DataAnnotations;
using Shared.Models.PropertyBusiness.PropertyBusinessAnnualSubmission.UkFhlProperty;

namespace Shared.Models.PropertyBusiness.PropertyBusinessAnnualSubmission
{
    /// <summary>
    /// Represents the UK property income details for annual submissions.
    /// For TY 2024-25 and before
    /// </summary>
    public class PropertyBusinessAnnualSubmission
    {
        /// <summary>
        /// Gets or sets the income details for furnished holiday lettings in the UK.
        /// </summary>
        [Required]
        public UkFhlPropertyDetails UkFhlProperty { get; init; } = new();

        /// <summary>
        /// Gets or sets the income details for other UK property.
        /// </summary>
        [Required]
        public UkFhlPropertyDetails UkProperty { get; init; } = new();
    }
}