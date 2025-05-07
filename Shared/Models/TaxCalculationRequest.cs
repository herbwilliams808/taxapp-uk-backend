using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class TaxCalculationRequest
    {
        [Required]
        public decimal EmploymentIncome { get; set; }

        [Required]
        public decimal PropertyIncome { get; set; }

        public decimal TaxDeducted { get; set; }
    }
}