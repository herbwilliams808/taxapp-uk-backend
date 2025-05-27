using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.IndividualsEmploymentIncomes.Employments
{
    public class Deductions
    {
        [SwaggerSchema(Description = "The amount of student loan deductions.")]
        [Range(0, double.MaxValue, ErrorMessage = "Student Loan Amount must be a non-negative number.")]
        public decimal? StudentLoans { get; set; }

        [SwaggerSchema(Description = "The amount of postgraduate loan deductions.")]
        [Range(0, double.MaxValue, ErrorMessage = "Postgraduate Loan Amount must be a non-negative number.")]
        public decimal? PostgraduateLoans { get; set; }
    }
}