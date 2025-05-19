namespace Shared.Models.HttpMessages
{
    public record TaxEstimationRequest(
        int? TaxYearEnding, 
        string Region, 
        Incomes.Incomes Incomes, 
        Reliefs.Reliefs Reliefs // Added Contributions property
    );
}