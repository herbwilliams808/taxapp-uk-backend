namespace Shared.Models.IndividualsReliefs.ForeignReliefs
{
    public class ForeignReliefsDetails
    {
        public ForeignTaxCreditRelief? ForeignTaxCreditRelief { get; init; }

        public List<ForeignIncomeTaxCreditRelief>? ForeignIncomeTaxCreditRelief { get; init; }

        public ForeignTaxForFtcrNotClaimed? ForeignTaxForFtcrNotClaimed { get; init; }
    }
}