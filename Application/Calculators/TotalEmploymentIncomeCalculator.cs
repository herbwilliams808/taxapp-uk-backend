using Shared.Models.Incomes;

namespace Application.Calculators;

public class TotalEmploymentIncomeCalculator
{
    public decimal Calculate(Incomes incomes)
    {
        var employmentIncomes = incomes.Employments.Select(employment => employment.Pay.TaxablePayToDate ?? 0m);

        return employmentIncomes?.Sum() ?? 0;
    }
}