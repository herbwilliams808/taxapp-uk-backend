using Shared.Models.IndividualsReliefs.CharitableGivings;
using Shared.Models.IndividualsReliefs.Pensions;

namespace Shared.Models.IndividualsReliefs;

public class IndividualsReliefsDetails
{
    public PensionReliefs? PensionReliefs { get; set; }
    public GiftAidPayments? GiftAidPayments { get; set; }
}