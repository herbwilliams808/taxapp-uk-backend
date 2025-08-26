namespace Core.Interfaces.Calculators;

public interface IPersonalAllowanceCalculator
{
    decimal Calculate(decimal standardPersonalAllowance, decimal adjustedNetIncome, decimal incomeLimit);
}