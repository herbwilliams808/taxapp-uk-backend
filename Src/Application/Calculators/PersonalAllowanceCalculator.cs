using Core.Interfaces.Calculators;

namespace Application.Calculators;

public class PersonalAllowanceCalculator : IPersonalAllowanceCalculator
{
    /// <summary>
    /// Calculates the personal allowance based on adjusted net income.
    /// </summary>
    /// <param name="standardPersonalAllowance">The standard personal allowance for the tax year.</param>
    /// <param name="adjustedNetIncome">The adjusted net income.</param>
    /// <param name="incomeLimit">The income limit for personal allowance tapering.</param>
    /// <returns>The calculated personal allowance.</returns>
    public decimal Calculate(decimal standardPersonalAllowance, decimal adjustedNetIncome, decimal incomeLimit)
    {
        if (adjustedNetIncome <= incomeLimit)
        {
            return standardPersonalAllowance;
        }

        var excessIncome = adjustedNetIncome - incomeLimit;
        var reducedAllowance = standardPersonalAllowance - (excessIncome / 2);

        return reducedAllowance > 0 ? reducedAllowance : 0;
    }
}