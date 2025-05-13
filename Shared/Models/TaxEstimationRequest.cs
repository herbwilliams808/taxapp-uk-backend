using Shared.Models.Contributions;
using Shared.Models.Incomes;

namespace Shared.Models
{
    public record TaxEstimationRequest(
        int? TaxYearEnding, 
        string Region, 
        Incomes.Incomes Incomes, 
        Contributions.Contributions Contributions // Added Contributions property
    );
}