using System.ComponentModel.DataAnnotations;

namespace Core.Models.IndividualsReliefs.ForeignReliefs
{
    public class ForeignTaxForFtcrNotClaimed
    {
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public decimal Amount { get; init; }
    }
}