using Application.Calculators; // To reference the concrete calculator
using Application.Interfaces;
using Application.Interfaces.Calculators; // To reference the calculator interface
using Xunit; // For [Fact] and Assert

namespace UnitTests.Application.Calculators;

public class TotalIncomeCalculatorTests
{
    private readonly ITotalIncomeCalculator _calculator = new TotalIncomeCalculator();

    [Fact]
    public void Calculate_ReturnsCorrectTotal_WhenAllInputsArePositive()
    {
        // Arrange
        const decimal employmentIncome = 1000.99m;
        const decimal benefitsInKind = 200.50m;
        const decimal employmentExpenses = 149.90m;
        const decimal otherDeductions = 49.40m;
        const decimal profitFromProperties = 300.30m;

        // Act
        var result = _calculator.Calculate(
            employmentIncome,
            benefitsInKind,
            employmentExpenses,
            otherDeductions,
            profitFromProperties);

        // Assert: 1000 + 200 - 150 - 50 + 300 = 1300
        Assert.Equal(1300m, result);
    }

    [Fact]
    public void Calculate_ReturnsCorrectTotal_WhenSomeInputsAreNull()
    {
        // Arrange
        decimal? employmentIncome = 1000m;
        decimal? benefitsInKind = null; // Null
        decimal? employmentExpenses = 150m;
        decimal? otherDeductions = null; // Null
        decimal? profitFromProperties = 300m;

        // Act
        var result = _calculator.Calculate(
            employmentIncome,
            benefitsInKind,
            employmentExpenses,
            otherDeductions,
            profitFromProperties);

        // Assert: 1000 + 0 - 150 - 0 + 300 = 1150
        Assert.Equal(1150m, result);
    }

    [Fact]
    public void Calculate_ReturnsZero_WhenAllInputsAreZeroOrNull()
    {
        // Arrange
        decimal? employmentIncome = 0m;
        decimal? benefitsInKind = 0m;
        decimal? employmentExpenses = 0m;
        decimal? otherDeductions = 0m;
        decimal? profitFromProperties = 0m;

        // Act
        var result = _calculator.Calculate(
            employmentIncome,
            benefitsInKind,
            employmentExpenses,
            otherDeductions,
            profitFromProperties);

        // Assert
        Assert.Equal(0m, result);

        // Arrange (all nulls)
        result = _calculator.Calculate(null, null, null, null, null);

        // Assert
        Assert.Equal(0m, result);
    }

    [Fact]
    public void Calculate_ReturnsCorrectTotal_WhenOnlyEmploymentIncomeIsProvided()
    {
        // Arrange
        decimal? employmentIncome = 5000m;

        // Act
        var result = _calculator.Calculate(
            employmentIncome,
            null, // All others null
            null,
            null,
            null);

        // Assert
        Assert.Equal(5000m, result);
    }

    [Fact]
    public void Calculate_ReturnsCorrectTotal_WhenOnlyDeductionsAreProvided()
    {
        // Arrange
        decimal? employmentExpenses = 200m;
        decimal? otherDeductions = 100m;

        // Act
        var result = _calculator.Calculate(
            null,
            null,
            employmentExpenses,
            otherDeductions,
            null);

        // Assert: 0 + 0 - 200 - 100 + 0 = -300
        Assert.Equal(-300m, result); // Represents a net loss from deductions
    }

    [Fact]
    public void Calculate_ReturnsCorrectTotal_WithMixedPositiveAndNegativeProfitFromProperties()
    {
        // Arrange
        decimal employmentIncome = 500m;
        decimal profitFromProperties = -100m; // Example: a loss from property

        // Act
        var result = _calculator.Calculate(
            employmentIncome,
            null,
            null,
            null,
            profitFromProperties);

        // Assert: 500 + 0 - 0 - 0 - 100 = 400
        Assert.Equal(400m, result);
    }

    // Add more specific tests for other individual components if desired,
    // or combinations that are particularly relevant to your business logic.
}