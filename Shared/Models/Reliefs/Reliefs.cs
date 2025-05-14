using Shared.Models.Reliefs;
using Shared.Models.Reliefs.CharitableGivings;
using Shared.Models.Reliefs.Pensions;

namespace Shared.Models.Reliefs;

public class Reliefs
{
    public PensionReliefs? PensionReliefs { get; set; }
    public GiftAidPayments? GiftAidPayments { get; set; }
}