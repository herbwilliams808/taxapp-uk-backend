using Core.Models.IndividualsReliefs.CharitableGivings;
using Core.Models.IndividualsReliefs.Pensions;

namespace Core.Models.IndividualsReliefs;

public class IndividualsReliefsDetails
{
    public PensionReliefs? PensionReliefs { get; set; }
    public GiftAidPayments? GiftAidPayments { get; set; }
}