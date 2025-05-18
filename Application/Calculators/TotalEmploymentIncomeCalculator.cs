namespace Application.Calculators;

public class TotalEmploymentIncomeCalculator
{
    public decimal Calculate(IEnumerable<decimal>? employmentIncomes)
    {
        return employmentIncomes?.Sum() ?? 0;
    }
}