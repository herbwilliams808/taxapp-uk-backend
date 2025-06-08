namespace Application.Interfaces.Calculators;

public interface IPersonalAllowanceCalculator
{
    /// <summary>
    /// Calculates the personal allowance based on adjusted net income.
    /// </summary>
    /// <param name="standardPersonalAllowance">The standard personal allowance for the tax year.</param>
    /// <param name="adjustedNetIncome">The adjusted net income.</param>
    /// <param name="incomeLimit">The income limit for personal allowance tapering.</param>
    /// <returns>The calculated personal allowance.</returns>
    decimal Calculate(decimal standardPersonalAllowance, decimal adjustedNetIncome, decimal incomeLimit);
}