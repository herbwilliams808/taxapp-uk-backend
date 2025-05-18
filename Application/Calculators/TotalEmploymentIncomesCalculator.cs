namespace Application.Calculators;

public class TotalEmploymentIncomesCalculator
{
    public decimal Calculate(IEnumerable<decimal>? employmentIncomes)
    {
        return employmentIncomes?.Sum() ?? 0;
    }
}