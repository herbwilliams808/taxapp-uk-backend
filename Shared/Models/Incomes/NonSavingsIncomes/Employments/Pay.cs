using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.Incomes.NonSavingsIncomes.Employments
{
    public class Pay
    {
        public Pay()
        {
        }

        public Pay(decimal? taxablePayToDate)
        {
            TaxablePayToDate = taxablePayToDate;
        }

        [SwaggerSchema(Description = "The cumulative taxable pay up to the current date.")]
        [Range(0, double.MaxValue, ErrorMessage = "Taxable Pay To Date must be a non-negative number.")]
        public decimal? TaxablePayToDate { get; set; }

        [SwaggerSchema(Description = "The cumulative total tax deducted up to the current date.")]
        [Range(0, double.MaxValue, ErrorMessage = "Total Tax To Date must be a non-negative number.")]
        public decimal? TotalTaxToDate { get; set; }
    }
}