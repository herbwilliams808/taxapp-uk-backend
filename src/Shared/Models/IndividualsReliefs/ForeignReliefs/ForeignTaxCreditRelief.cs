using System.ComponentModel.DataAnnotations;

namespace Shared.Models.IndividualsReliefs.ForeignReliefs
{
    public class ForeignTaxCreditRelief
    {
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public decimal Amount { get; init; }
    }
}