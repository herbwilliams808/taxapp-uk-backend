using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.IndividualsForeignIncome
{
    /// <summary>
    /// Represents a request body for creating or amending foreign income.
    /// </summary>
    public class IndividualsForeignIncome
    {
        /// <summary>
        /// Details about foreign earnings, if any.
        /// </summary>
        [SwaggerSchema(Description = "Details about foreign earnings, if any.")]
        [JsonPropertyName("foreignEarnings")]
        public ForeignEarnings? ForeignEarnings { get; init; }

        /// <summary>
        /// Details about unremittable foreign income, if any.
        /// </summary>
        [SwaggerSchema(Description = "Details about unremittable foreign income, if any.")]
        [JsonPropertyName("unremittableForeignIncome")]
        public List<UnremittableForeignIncomeItem>? UnremittableForeignIncome { get; init; }
    }
    
}