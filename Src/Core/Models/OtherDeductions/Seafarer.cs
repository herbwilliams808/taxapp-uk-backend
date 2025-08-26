using System.ComponentModel.DataAnnotations;

namespace Core.Models.OtherDeductions
{
    public class Seafarer(decimal amountDeducted, string nameOfShip, DateTime fromDate, DateTime toDate )
    {
        [StringLength(100)]
        public string? CustomerReference { get; init; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "AmountDeducted must be a positive value.")]
        public decimal AmountDeducted { get; init; } = amountDeducted;

        [Required]
        [StringLength(200)]
        public string NameOfShip { get; init; } = nameOfShip;

        [Required]
        public DateTime FromDate { get; init; } = fromDate;

        [Required]
        public DateTime ToDate { get; init; } = toDate;
    }
}