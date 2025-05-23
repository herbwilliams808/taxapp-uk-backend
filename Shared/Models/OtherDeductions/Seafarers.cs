using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.OtherDeductions
{
    public class Seafarers
    {
        [StringLength(100)]
        public string? CustomerReference { get; init; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "AmountDeducted must be a positive value.")]
        public decimal AmountDeducted { get; init; }

        [Required]
        [StringLength(200)]
        public string NameOfShip { get; init; }

        [Required]
        public DateTime FromDate { get; init; }

        [Required]
        public DateTime ToDate { get; init; }
    }
}