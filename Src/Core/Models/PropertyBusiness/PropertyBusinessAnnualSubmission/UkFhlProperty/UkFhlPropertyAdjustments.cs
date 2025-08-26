using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Core.Models.PropertyBusiness.PropertyBusinessAnnualSubmission.UkFhlProperty
{
    /// <summary>
    /// Object holding UK FHL Property UkPropertyAdjustments.
    /// </summary>
    [SwaggerSchema("Object holding UK FHL Property UkPropertyAdjustments.")]
    public class UkFhlPropertyAdjustments
    {
        /// <summary>
        /// Any adjustments that are not solely for the property business.
        /// </summary>
        [SwaggerSchema("Any adjustments that are not solely for the property business. The value must be between 0 and 99999999999.99 up to 2 decimal places.")]
        [Range(0, 99999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Up to 2 decimal places allowed.")]
        [JsonPropertyName("privateUseAdjustment")]
        public decimal? PrivateUseAdjustment { get; set; }

        /// <summary>
        /// If an item for which capital allowance was claimed has been sold, given away or is no longer in use.
        /// </summary>
        [SwaggerSchema("If an item for which capital allowance was claimed has been sold, given away or is no longer in use. The value must be between 0 and 99999999999.99 up to 2 decimal places.")]
        [Range(0, 99999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Up to 2 decimal places allowed.")]
        [JsonPropertyName("balancingCharge")]
        public decimal? BalancingCharge { get; set; }

        /// <summary>
        /// A boolean to identify a property which didn't qualify for FHL this year, but qualified the previous year.
        /// </summary>
        [SwaggerSchema("A boolean to identify a property which didn't qualify for FHL this year, but qualified the previous year.")]
        [Required]
        [JsonPropertyName("periodOfGraceAdjustment")]
        public bool PeriodOfGraceAdjustment { get; set; }

        /// <summary>
        /// Income from the sale or grant of a long lease for a premium of renovated business premises within 7 years of first use.
        /// </summary>
        [SwaggerSchema("Income from the sale or grant of a long lease for a premium of renovated business premises within 7 years of first use. The value must be between 0 and 99999999999.99 up to 2 decimal places.")]
        [Range(0, 99999999999.99)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Up to 2 decimal places allowed.")]
        [JsonPropertyName("businessPremisesRenovationAllowanceBalancingCharges")]
        public decimal? BusinessPremisesRenovationAllowanceBalancingCharges { get; set; }

        /// <summary>
        /// A boolean to identify that the user is a Non-Resident Landlord.
        /// </summary>
        [SwaggerSchema("A boolean to identify that the user is a Non-Resident Landlord.")]
        [Required]
        [JsonPropertyName("nonResidentLandlord")]
        public bool NonResidentLandlord { get; set; }

        /// <summary>
        /// Object holding UK FHL Rent A Room income shared.
        /// </summary>
        [SwaggerSchema("Object holding UK FHL Rent A Room income shared.")]
        [JsonPropertyName("rentARoom")]
        public UkFhlRentARoom? RentARoom { get; set; }
    }
}
