using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.Incomes.NonSavingsIncomes.Employments;

[SwaggerSchema(Description = "Represents employer details for employment income")]
public class Employer
{
    [SwaggerSchema(Description = "The reference number for the employer")]
    [StringLength(50, ErrorMessage = "Employer reference cannot exceed 50 characters")]
    public string? EmployerRef { get; set; }
    
    [SwaggerSchema(Description = "The name of the employer")]
    [Required(ErrorMessage = "Employer name must be provided")]
    [StringLength(160, ErrorMessage = "Employer name cannot exceed 160 characters")]
    public required string EmployerName { get; set; }
}