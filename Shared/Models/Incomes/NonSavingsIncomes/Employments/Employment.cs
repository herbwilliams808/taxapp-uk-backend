using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.Incomes.NonSavingsIncomes.Employments
{
    public class Employment
    {
        [SwaggerSchema(Description = "A unique identifier for the employment record.")]
        public string? EmploymentSequenceNumber { get; set; } = "DefaultSequence";

        [SwaggerSchema(Description = "The payroll ID associated with the employment.")]
        public string? PayrollId { get; set; }

        [SwaggerSchema(Description = "Indicates if the individual is a company director.")]
        public bool? CompanyDirector { get; set; } = false;

        [SwaggerSchema(Description = "Indicates if the company is a close company.")]
        public bool? CloseCompany { get; set; } = false;

        [SwaggerSchema(Description = "The date the directorship ceased, if applicable.")]
        public string? DirectorshipCeasedDate { get; set; }

        [SwaggerSchema(Description = "The start date of the employment.")]
        public string? StartDate { get; set; }

        [SwaggerSchema(Description = "The cessation date of the employment.")]
        public string? CessationDate { get; set; }

        [SwaggerSchema(Description = "Indicates if the employment is an occupational pension.")]
        public bool? OccupationalPension { get; set; } = false;

        [SwaggerSchema(Description = "Indicates if the employment involves disguised remuneration.")]
        public bool? DisguisedRemuneration { get; set; } = false;

        [SwaggerSchema(Description = "Indicates if the worker is considered an off-payroll worker.")]
        public bool? OffPayrollWorker { get; set; } = false;

        [Required]
        [SwaggerSchema(Description = "Details about the pay, including taxable pay and tax to date.")]
        public required Pay Pay { get; set; }

        [SwaggerSchema(Description = "The customer's estimated pay for this employment.")]
        public CustomerEstimatedPay? CustomerEstimatedPay { get; set; }

        [SwaggerSchema(Description = "Details about deductions such as student and postgraduate loan payments.")]
        public Deductions? Deductions { get; set; }

        [SwaggerSchema(Description = "Details about benefits in kind provided by the employer.")]
        public Benefits? BenefitsInKind { get; set; }
        
        [SwaggerSchema(Description = "Details about expenses while at the employer.")]
        public Expenses.Expenses? Expenses { get; set; }

        [SwaggerSchema(Description = "Details about the employer.")]
        public required Employer Employer { get; set; }
    }
}
