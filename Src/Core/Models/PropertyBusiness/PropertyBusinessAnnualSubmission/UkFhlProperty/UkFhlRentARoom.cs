using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Core.Models.PropertyBusiness.PropertyBusinessAnnualSubmission.UkFhlProperty
{
    /// <summary>
    /// Object holding UK FHL Rent A Room income shared.
    /// </summary>
    [SwaggerSchema("Object holding UK FHL Rent A Room income shared.")]
    public class UkFhlRentARoom
    {
        /// <summary>
        /// A boolean to identify that the Rent A Room income (RAR) is shared with another individual.
        /// </summary>
        [SwaggerSchema("A boolean to identify that the Rent A Room income (RAR) is shared with another individual.")]
        [Required]
        [JsonPropertyName("jointlyLet")]
        public bool JointlyLet { get; set; }
    }
}