using Application.Calculators;
using Shared.Models.Incomes;
using Shared.Models.Incomes.NonSavingsIncomes;

namespace UnitTests.Application.Calculators;

public class ProfitFromPropertiesCalculatorTests
{
    private readonly ProfitFromPropertiesCalculator _calculator = new();

    [Fact]
    public void Calculate_WithNullPropertyBusiness_ReturnsZero()
    {
        // Arrange
        var incomes = new Incomes { UkPropertyBusiness = null };

        // Act
        var result = _calculator.Calculate(incomes);

        // Assert
        Assert.Equal(0m, result);
    }

    [Fact]
    public void Calculate_WithZeroIncomeAndExpenses_ReturnsZero()
    {
        // Arrange
        var incomes = new Incomes
        {
            UkPropertyBusiness = new UkPropertyBusinessIncome
            {
                Income = 0m,
                AllowablePropertyLettingExpenses = 0m
            }
        };

        // Act
        var result = _calculator.Calculate(incomes);

        // Assert
        Assert.Equal(0m, result);
    }

    [Theory]
    [InlineData(1000, 300, 700)]
    [InlineData(5000, 2000, 3000)]
    [InlineData(2000, 2500, -500)]
    public void Calculate_WithVariousIncomesAndExpenses_ReturnsCorrectProfit(
        decimal income, decimal expenses, decimal expectedProfit)
    {
        // Arrange
        var incomes = new Incomes
        {
            UkPropertyBusiness = new UkPropertyBusinessIncome
            {
                Income = income,
                AllowablePropertyLettingExpenses = expenses
            }
        };

        // Act
        var result = _calculator.Calculate(incomes);

        // Assert
        Assert.Equal(expectedProfit, result);
    }

    [Fact]
    public void Calculate_WithNullIncomeAndExpenses_ReturnsZero()
    {
        // Arrange
        var incomes = new Incomes
        {
            UkPropertyBusiness = new UkPropertyBusinessIncome
            {
                Income = null,
                AllowablePropertyLettingExpenses = null
            }
        };

        // Act
        var result = _calculator.Calculate(incomes);

        // Assert
        Assert.Equal(0m, result);
    }
}