using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Core.Models.IndividualsForeignIncome;

/// <summary>
/// Represents an item of unremittable foreign income.
/// </summary>
public class UnremittableForeignIncomeItem
{
    public UnremittableForeignIncomeItem(string exampleProperty)
    {
        ExampleProperty = exampleProperty;
    }

    /// <summary>
    /// Example property for unremittable foreign income item.
    /// </summary>
    [SwaggerSchema(Description = "Example property for unremittable foreign income item.")]
    [JsonPropertyName("exampleProperty")]
    public string ExampleProperty { get; init; }
}