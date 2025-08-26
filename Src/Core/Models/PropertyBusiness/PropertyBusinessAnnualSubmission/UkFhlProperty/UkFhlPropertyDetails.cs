using Swashbuckle.AspNetCore.Annotations;

namespace Core.Models.PropertyBusiness.PropertyBusinessAnnualSubmission.UkFhlProperty
{
    /// <summary>
    /// Represents UK Furnished Holiday Lettings (FHL) property details for annual submissions.
    /// </summary>
    [SwaggerSchema("Details of a UK Furnished Holiday Lettings (FHL) property submission.")]
    public class UkFhlPropertyDetails
    {
        /// <summary>
        /// Adjustments applicable to UK FHL properties.
        /// </summary>
        [SwaggerSchema("UkPropertyAdjustments applicable to the UK FHL property.")]
        public UkFhlPropertyAdjustments? Adjustments { get; init; }

        /// <summary>
        /// Allowances applicable to UK FHL properties.
        /// </summary>
        [SwaggerSchema("Allowances applicable to the UK FHL property.")]
        public UkFhlPropertyAllowances? Allowances { get; init; }
    }
}