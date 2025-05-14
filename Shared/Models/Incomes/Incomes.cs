using Shared.Models.Incomes.NonSavingsIncomes;

namespace Shared.Models.Incomes;

public class Incomes
{
    public List<EmploymentIncome> Employment { get; set; } = new();
    public UkPropertyBusinessIncome UkPropertyBusiness { get; set; } = new();
}