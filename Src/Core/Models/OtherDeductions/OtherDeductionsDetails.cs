using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Core.Models.OtherDeductions
{
    /// <summary>
    /// Represents the other deductions, including optional details for seafarers.
    /// </summary>
    public class OtherDeductionsDetails
    {
        /// <summary>
        /// Details of the seafarers' deductions, if any.
        /// </summary>
        [SwaggerSchema(Description = "Details of the seafarers' deductions, if any.")]
        [JsonPropertyName("seafarers")]
        public List<Seafarer>? Seafarers { get; init; } = new();
    }


}