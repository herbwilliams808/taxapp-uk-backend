using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.OtherDeductions
{
    public class Seafarer(string nameOfShip, DateTime fromDate, DateTime toDate)
    {
        [StringLength(100)]
        public string? CustomerReference { get; init; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "AmountDeducted must be a positive value.")]
        public decimal AmountDeducted { get; init; }

        [Required]
        [StringLength(200)]
        public string NameOfShip { get; init; } = nameOfShip;

        [Required]
        public DateTime FromDate { get; init; } = fromDate;

        [Required]
        public DateTime ToDate { get; init; } = toDate;
    }
}