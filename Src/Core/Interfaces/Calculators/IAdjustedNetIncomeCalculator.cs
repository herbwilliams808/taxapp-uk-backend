namespace Core.Interfaces.Calculators;

public interface IAdjustedNetIncomeCalculator
{
    decimal Calculate(decimal netIncome, decimal? pensionContributions, decimal? giftAidContributions);
}