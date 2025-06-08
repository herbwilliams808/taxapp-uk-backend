using Application.Calculators;

namespace UnitTests.Application.Calculators;

using Xunit;

public class PersonalAllowanceCalculatorTests
{
    private readonly PersonalAllowanceCalculator _calculator = new();

    // Constructor to initialise the calculator before each test

    /// <summary>
    /// Tests the PersonalAllowanceCalculator.Calculate method with various scenarios.
    /// </summary>
    /// <param name="standardPersonalAllowance">The standard personal allowance for the tax year.</param>
    /// <param name="adjustedNetIncome">The adjusted net income.</param>
    /// <param name="incomeLimit">The income limit for personal allowance tapering.</param>
    /// <param name="expectedAllowance">The expected calculated personal allowance.</param>
    [Theory]
    // Scenario 1: Adjusted net income is less than the income limit.
    // Should return the full standard personal allowance.
    [InlineData(12570, 90000, 100000, 12570)] // Income well below limit
    [InlineData(10000, 80000, 90000, 10000)] // Different allowance, income below limit

    // Scenario 2: Adjusted net income is exactly at the income limit.
    // Should return the full standard personal allowance.
    [InlineData(12570, 100000, 100000, 12570)] // Income exactly at limit
    [InlineData(11000, 95000, 95000, 11000)] // Different allowance, income at limit

    // Scenario 3: Adjusted net income is above the income limit,
    // but the allowance is still positive after reduction.
    // (Excess income / 2) < standardPersonalAllowance
    // Example: Income 110,000, Limit 100,000 -> Excess 10,000 -> Reduction 5,000
    // 12,570 - 5,000 = 7,570
    [InlineData(12570, 110000, 100000, 7570)]
    [InlineData(15000, 105000, 100000, 12500)] // Different allowance, reduced but positive

    // Scenario 4: Adjusted net income causes the allowance to be reduced to exactly zero.
    // (Excess income / 2) == standardPersonalAllowance
    // Example: StandardAllowance 12,570. Need reduction of 12,570. ExcessIncome must be 25,140.
    // AdjustedNetIncome = IncomeLimit + ExcessIncome = 100,000 + 25,140 = 125,140
    [InlineData(12570, 125140, 100000, 0)]
    [InlineData(10000, 120000, 100000, 0)] // Different allowance, reduced to zero

    // Scenario 5: Adjusted net income causes the allowance to be reduced to a negative value.
    // Should return 0, as allowance cannot be negative.
    // (Excess income / 2) > standardPersonalAllowance
    // Example: Income 130,000, Limit 100,000 -> Excess 30,000 -> Reduction 15,000
    // 12,570 - 15,000 = -2,430 (should return 0)
    [InlineData(12570, 130000, 100000, 0)]
    [InlineData(5000, 115000, 100000, 0)] // Different allowance, reduced to negative

    // Edge Cases / Zero Values
    [InlineData(0, 50000, 100000, 0)]   // Zero standard allowance, income below limit
    [InlineData(12570, 0, 100000, 12570)]  // Zero adjusted net income
    [InlineData(12570, 100000, 0, 0)]   // Zero income limit (will always cause reduction to 0)
    public void Calculate_ReturnsCorrectPersonalAllowance(
        decimal standardPersonalAllowance,
        decimal adjustedNetIncome,
        decimal incomeLimit,
        decimal expectedAllowance)
    {
        // Act
        var actualAllowance = _calculator.Calculate(standardPersonalAllowance, adjustedNetIncome, incomeLimit);

        // Assert
        Assert.Equal(expectedAllowance, actualAllowance);
    }
}