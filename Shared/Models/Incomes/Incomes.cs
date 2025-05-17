using Shared.Models.Incomes.NonSavingsIncomes;
using Shared.Models.Incomes.NonSavingsIncomes.Employments;

namespace Shared.Models.Incomes;

public class Incomes
{
    public List<Employment> Employments { get; set; } = new();
    public UkPropertyBusinessIncome UkPropertyBusiness { get; set; } = new();
}