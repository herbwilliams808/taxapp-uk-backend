using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.IndividualsReliefs
{
    public class ForeignIncomeTaxCreditRelief
    {
        public ForeignIncomeTaxCreditRelief(string countryCode)
        {
            CountryCode = countryCode;
        }

        [Required]
        [StringLength(3, MinimumLength = 2, ErrorMessage = "CountryCode must be 2 or 3 characters long.")]
        public string CountryCode { get; init; }

        [Range(0, double.MaxValue, ErrorMessage = "ForeignTaxPaid must be a positive value.")]
        public decimal? ForeignTaxPaid { get; init; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "TaxableAmount must be a positive value.")]
        public decimal TaxableAmount { get; init; }

        [Required]
        public bool EmploymentLumpSum { get; init; }
    }
}