using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Core.Models.IndividualsEmploymentIncomes.Employments;

public class Employer
{
    /// <summary>
    /// A unique identifier, the employer reference number.
    /// </summary>
    /// <example>123/AB56797</example>
    [RegularExpression("^[0-9]{3}\\/[^ ].{0,9}$")]
    [XmlElement("employerRef")]
    public string? EmployerRef { get; set; }

    /// <summary>
    /// The name of the employer the employee worked for.
    /// </summary>
    /// <example>Employer Name Ltd.</example>
    [Required]
    [RegularExpression("^\\S.{0,73}$")]
    [XmlElement("employerName")]
    public required string EmployerName { get; set; }
}