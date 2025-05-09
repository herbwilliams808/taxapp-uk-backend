using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class TaxEstimationRequest
    {
        [Required]
        public decimal EmploymentIncome { get; set; }

        [Required]
        public decimal PropertyIncome { get; set; }

        public decimal TaxDeducted { get; set; }
    }
}