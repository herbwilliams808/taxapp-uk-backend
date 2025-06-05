using Shared.Models.Incomes; // For IncomeSources, UkPropertyBusinessIncome
using Application.Calculators; // To reference the concrete calculator
using Application.Interfaces;
using Application.Interfaces.Calculators;
using Shared.Models.PropertyBusiness; // To reference the calculator interface
using Xunit; // For [Fact] and Assert

namespace UnitTests.Application.Calculators;

public class ProfitFromPropertiesCalculatorTests
{
    private readonly IProfitFromPropertiesCalculator _calculator;

    public ProfitFromPropertiesCalculatorTests()
    {
        _calculator = new ProfitFromPropertiesCalculator();
    }

    [Fact]
    public void Calculate_ReturnsCorrectProfit_WhenBothIncomeAndExpensesAreSet()
    {
        // Arrange
        var incomeSources = new IncomeSources
        {
            UkPropertyBusinessIncome = new UkPropertyBusinessIncome
            {
                Income = 1000m,
                AllowablePropertyLettingExpenses = 300m
            }
        };

        // Act
        var result = _calculator.Calculate(incomeSources);

        // Assert: 1000 - 300 = 700
        Assert.Equal(700m, result);
    }

    [Fact]
    public void Calculate_ReturnsIncome_WhenExpensesAreNull()
    {
        // Arrange
        var incomeSources = new IncomeSources
        {
            UkPropertyBusinessIncome = new UkPropertyBusinessIncome
            {
                Income = 1000m,
                AllowablePropertyLettingExpenses = null // Expenses are null
            }
        };

        // Act
        var result = _calculator.Calculate(incomeSources);

        // Assert: 1000 - 0 = 1000
        Assert.Equal(1000m, result);
    }

    [Fact]
    public void Calculate_ReturnsNegativeExpenses_WhenIncomeIsNull()
    {
        // Arrange
        var incomeSources = new IncomeSources
        {
            UkPropertyBusinessIncome = new UkPropertyBusinessIncome
            {
                Income = null, // Income is null
                AllowablePropertyLettingExpenses = 300m
            }
        };

        // Act
        var result = _calculator.Calculate(incomeSources);

        // Assert: 0 - 300 = -300 (Represents a loss)
        Assert.Equal(-300m, result);
    }

    [Fact]
    public void Calculate_ReturnsZero_WhenBothIncomeAndExpensesAreNull()
    {
        // Arrange
        var incomeSources = new IncomeSources
        {
            UkPropertyBusinessIncome = new UkPropertyBusinessIncome
            {
                Income = null,
                AllowablePropertyLettingExpenses = null
            }
        };

        // Act
        var result = _calculator.Calculate(incomeSources);

        // Assert: 0 - 0 = 0
        Assert.Equal(0m, result);
    }

    [Fact]
    public void Calculate_ReturnsZero_WhenUkPropertyBusinessIsNull()
    {
        // Arrange
        var incomeSources = new IncomeSources
        {
            UkPropertyBusinessIncome = null // UkPropertyBusinessIncome itself is null
        };

        // Act
        var result = _calculator.Calculate(incomeSources);

        // Assert: 0 - 0 = 0
        Assert.Equal(0m, result);
    }

    [Fact]
    public void Calculate_ReturnsZero_WhenIncomeSourcesIsNull()
    {
        // Arrange
        IncomeSources? incomeSources = null; // Entire input is null

        // Act
        var result = _calculator.Calculate(incomeSources);

        // Assert: 0 - 0 = 0
        Assert.Equal(0m, result);
    }

    [Fact]
    public void Calculate_WithZeroValues_ReturnsZero()
    {
        // Arrange
        var incomeSources = new IncomeSources
        {
            UkPropertyBusinessIncome = new UkPropertyBusinessIncome
            {
                Income = 0m,
                AllowablePropertyLettingExpenses = 0m
            }
        };

        // Act
        var result = _calculator.Calculate(incomeSources);

        // Assert
        Assert.Equal(0m, result);
    }
}