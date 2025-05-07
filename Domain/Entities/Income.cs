using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Income
    {
        [Required]
        public decimal EmploymentIncome { get; set; }

        [Required]
        public decimal PropertyIncome { get; set; }

        public decimal TaxDeducted { get; set; }
    }
}