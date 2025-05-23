using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.IndividualsReliefs
{
    public class CreateAndAmendForeignReliefsBody
    {
        public ForeignTaxCreditRelief? ForeignTaxCreditRelief { get; init; }

        public List<ForeignIncomeTaxCreditRelief>? ForeignIncomeTaxCreditRelief { get; init; }

        public ForeignTaxForFtcrNotClaimed? ForeignTaxForFtcrNotClaimed { get; init; }
    }
}