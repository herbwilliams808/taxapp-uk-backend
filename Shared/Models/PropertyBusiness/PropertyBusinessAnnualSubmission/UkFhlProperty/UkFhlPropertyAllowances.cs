using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.PropertyBusiness.PropertyBusinessAnnualSubmission.UkFhlProperty
{
    /// <summary>
    /// Object holding Furnished Holiday Lettings (FHL) property allowances details.
    /// </summary>
    public class UkFhlPropertyAllowances
    {
        /// <summary>
        /// The amount claimed for annual investment allowance.
        /// Example: 5000.99
        /// </summary>
        [Range(0.00, 99999999999.99)]
        [SwaggerSchema(Description = "The amount claimed for annual investment allowance. The value must be between 0 and 99999999999.99 up to 2 decimal places.", Format = "decimal")]
        public decimal? AnnualInvestmentAllowance { get; set; }

        /// <summary>
        /// The allowance amount for renovation or conversion of derelict business properties.
        /// Example: 5000.99
        /// </summary>
        [Range(0.00, 99999999999.99)]
        [SwaggerSchema(Description = "The allowance amount for renovation or conversion of derelict business properties. The value must be between 0 and 99999999999.99 up to 2 decimal places.", Format = "decimal")]
        public decimal? BusinessPremisesRenovationAllowance { get; set; }

        /// <summary>
        /// All other capital allowances.
        /// Example: 5000.99
        /// </summary>
        [Range(0.00, 99999999999.99)]
        [SwaggerSchema(Description = "All other capital allowances. The value must be between 0 and 99999999999.99 up to 2 decimal places.", Format = "decimal")]
        public decimal? OtherCapitalAllowance { get; set; }

        /// <summary>
        /// The expenditure incurred on electric charge-point equipment.
        /// Example: 5000.99
        /// </summary>
        [Range(0.00, 99999999999.99)]
        [SwaggerSchema(Description = "The expenditure incurred on electric charge-point equipment. The value must be between 0 and 99999999999.99 up to 2 decimal places.", Format = "decimal")]
        public decimal? ElectricChargePointAllowance { get; set; }

        /// <summary>
        /// The amount of zero emissions car allowance.
        /// Example: 5000.99
        /// </summary>
        [Range(0.00, 99999999999.99)]
        [SwaggerSchema(Description = "The amount of zero emissions car allowance. The value must be between 0 and 99999999999.99 up to 2 decimal places.", Format = "decimal")]
        public decimal? ZeroEmissionsCarAllowance { get; set; }

        /// <summary>
        /// The amount of tax exemption for individuals with income from furnished holiday lettings.
        /// Example: 200.25
        /// </summary>
        [Range(0.00, 1000.00)]
        [SwaggerSchema(Description = "The amount of tax exemption for individuals with income from furnished holiday lettings. The value must be between 0 and 1000.00 up to 2 decimal places.", Format = "decimal")]
        public decimal? PropertyIncomeAllowance { get; set; }
    }
}
