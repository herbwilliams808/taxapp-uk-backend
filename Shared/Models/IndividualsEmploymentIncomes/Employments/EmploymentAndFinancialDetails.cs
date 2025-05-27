using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Shared.Models.IndividualsEmploymentIncomes.Employments;

public class EmploymentAndFinancialDetails
{
    /// <summary>
    /// A number that identifies the position of the employment in a sequence of employments for a given tax year.
    /// </summary>
    /// <example>1</example>
    [RegularExpression("^[1-9][0-9]{0,9}$")]
    [XmlElement("employmentSequenceNumber")]
    public string? EmploymentSequenceNumber { get; set; }

    /// <summary>
    /// A unique identifier used by the employer to identify the employee.
    /// </summary>
    /// <example>124214112412</example>
    [RegularExpression("^[A-Za-z0-9.,\\-()/=!\"%&*;<>'+:\\?]{0,38}$")]
    [XmlElement("payrollId")]
    public string? PayrollId { get; set; }

    /// <summary>
    /// Identifies the employee as a director when set to True.
    /// This field will not be applicable for tax years 2025-26 onwards.
    /// </summary>
    /// <example>false</example>
    [XmlElement("companyDirector")]
    public bool? CompanyDirector { get; set; }

    /// <summary>
    /// Identifies if the employer is a close company.
    /// This field will not be applicable for tax years 2025-26 onwards.
    /// </summary>
    /// <example>false</example>
    [XmlElement("closeCompany")]
    public bool? CloseCompany { get; set; }

    /// <summary>
    /// The date the directorship ended.
    /// Format: YYYY-MM-DD
    /// This field will not be applicable for tax years 2025-26 onwards.
    /// </summary>
    /// <example>2020-07-01</example>
    [XmlElement("directorshipCeasedDate")]
    public string? DirectorshipCeasedDate { get; set; }

    /// <summary>
    /// The date the employment began.
    /// Format: YYYY-MM-DD
    /// </summary>
    /// <example>2020-07-01</example>
    [XmlElement("startDate")]
    public string? StartDate { get; set; }

    /// <summary>
    /// The date the employment ended.
    /// Format: YYYY-MM-DD
    /// </summary>
    /// <example>2020-07-01</example>
    [XmlElement("cessationDate")]
    public string? CessationDate { get; set; }

    /// <summary>
    /// Indicates that the employee pays into an occupational pension when set to True.
    /// </summary>
    /// <example>false</example>
    [XmlElement("occupationalPension")]
    public bool? OccupationalPension { get; set; }

    /// <summary>
    /// Indicates that the employee is declaring disguised remuneration when set to True.
    /// </summary>
    /// <example>false</example>
    [XmlElement("disguisedRemuneration")]
    public bool? DisguisedRemuneration { get; set; }

    /// <summary>
    /// Indicates whether the employer is deducting PAYE as they consider the worker contract falls under Off-Payroll Working (OPW) rules.
    /// Required for tax year 2023/24 or later.
    /// </summary>
    /// <example>true</example>
    [XmlElement("offPayrollWorker")]
    public bool? OffPayrollWorker { get; set; }

    /// <summary>
    /// Object containing employer details.
    /// </summary>
    [XmlElement("employer")]
    public required Employer Employer { get; set; }

    /// <summary>
    /// Object containing pay details for a single employment.
    /// </summary>
    [XmlElement("pay")]
    public Pay? Pay { get; set; }

    /// <summary>
    /// Object containing estimated pay details for a single employment.
    /// </summary>
    [XmlElement("customerEstimatedPay")]
    public CustomerEstimatedPay? CustomerEstimatedPay { get; set; }

    /// <summary>
    /// Deductions details.
    /// </summary>
    [XmlElement("deductions")]
    public Deductions? Deductions { get; set; }

    /// <summary>
    /// Benefits in kind details.
    /// </summary>
    [XmlElement("benefitsInKind")]
    public BenefitsInKind? BenefitsInKind { get; set; }
}