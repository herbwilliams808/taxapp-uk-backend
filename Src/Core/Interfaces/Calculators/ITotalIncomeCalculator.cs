namespace Core.Interfaces.Calculators;

public interface ITotalIncomeCalculator
{
    decimal Calculate(decimal? employmentIncome, decimal? benefitsInKind, decimal? employmentExpenses,
        decimal? otherDeductions, decimal? profitFromProperties);
}