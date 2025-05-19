using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.Incomes.NonSavingsIncomes.Employments
{
    public class Employment
    {
        [Required]
        [SwaggerSchema(Description = "Details about the pay, including taxable pay and tax to date.")]
        public required Pay Pay { get; set; }

        [SwaggerSchema(Description = "Details about deductions such as student and postgraduate loan payments.")]
        public Deductions? Deductions { get; set; }

        [SwaggerSchema(Description = "Details about benefits in kind provided by the employer.")]
        public Benefits? BenefitsInKind { get; set; }

        [SwaggerSchema(Description = "Indicates if the worker is considered an off-payroll worker.")]
        public bool? OffPayrollWorker { get; set; }
    }
}