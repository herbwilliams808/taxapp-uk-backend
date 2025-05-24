using Shared.Models.IndividualsReliefs.CharitableGivings;
using Shared.Models.IndividualsReliefs.Pensions;

namespace Shared.Models.IndividualsReliefs;

public class IndividualsReliefs
{
    public PensionReliefs? PensionReliefs { get; set; }
    public GiftAidPayments? GiftAidPayments { get; set; }
}