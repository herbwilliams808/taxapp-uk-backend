namespace Application.Calculators;

public class TotalIncomeCalculator
{
    /// <summary>
    ///     Calculates the total income received.
    /// </summary>
    /// <param name="employmentIncome">The total value of employment incomes, which can be null.</param>
    /// <param name="benefitsInKind"></param>
    /// <param name="employmentExpenses"></param>
    /// <param name="otherDeductions"></param>
    /// <param name="profitFromProperties">The profit from properties, which can be null.</param>
    /// <returns>The total income received.</returns>
    public decimal Calculate(decimal? employmentIncome, decimal? benefitsInKind, decimal? employmentExpenses,
        decimal? otherDeductions, decimal? profitFromProperties)

    {
        employmentIncome ??= 0m;
        benefitsInKind ??= 0m;
        employmentExpenses ??= 0m;
        otherDeductions ??= 0m;
        profitFromProperties ??= 0m;

        return (decimal)(employmentIncome + benefitsInKind - employmentExpenses - otherDeductions + profitFromProperties);
    }
}