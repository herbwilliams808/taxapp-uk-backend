using Shared.Models.Reliefs;
using Shared.Models.Incomes;

namespace Shared.Models
{
    public record TaxEstimationRequest(
        int? TaxYearEnding, 
        string Region, 
        Incomes.Incomes Incomes, 
        Reliefs.Reliefs Reliefs // Added Contributions property
    );
}