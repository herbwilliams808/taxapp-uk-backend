using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Shared.Models.Incomes.NonSavingsIncomes.Employments;

/// <summary>
/// Represents a customer's estimated pay details
/// </summary>
[ModelMetadataType(typeof(CustomerEstimatedPay))]
public class CustomerEstimatedPay
{
    /// <summary>
    /// The estimated pay amount in decimal format
    /// </summary>
    /// <example>1000.50</example>
    // [Required(AllowEmptyStrings = false, ErrorMessage = "Amount is required")]
    // [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Amount must be non-negative")]
    // [DefaultValue(null)]
    [DisplayName("Estimated Pay Amount")]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    public decimal? Amount { get; set; }
}