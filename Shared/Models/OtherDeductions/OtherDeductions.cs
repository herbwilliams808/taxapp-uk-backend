using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.OtherDeductions
{
    /// <summary>
    /// Represents the other deductions, including optional details for seafarers.
    /// </summary>
    public class OtherDeductions
    {
        /// <summary>
        /// Details of the seafarers' deductions, if any.
        /// </summary>
        [SwaggerSchema(Description = "Details of the seafarers' deductions, if any.")]
        [JsonPropertyName("seafarers")]
        public List<Seafarer>? Seafarers { get; init; } = new();
    }


}