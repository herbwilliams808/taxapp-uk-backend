using Application.Calculators;
using Application.Interfaces.Calculators;
using Shared.Models.OtherDeductions;
// For OtherDeductionsDetails, Seafarer
// To reference the concrete calculator
// To reference the calculator interface
// For [Fact] and Assert
// For List<T>

// For DateTime

namespace UnitTests.Application.Calculators;

public class TotalOtherDeductionsCalculatorTests
{
    private readonly ITotalOtherDeductionsCalculator _calculator = new TotalOtherDeductionsCalculator();

    [Fact]
    public void Calculate_ReturnsCorrectTotal_WhenMultipleSeafarersArePresent()
    {
        // Arrange
        var otherDeductionsDetails = new OtherDeductionsDetails
        {
            Seafarers =
            [
                new Seafarer(100m, "Ship A", new DateTime(2023, 1, 1), new DateTime(2023, 1, 31)),
                new Seafarer(250m, "Ship B", new DateTime(2023, 2, 1), new DateTime(2023, 2, 28)),
                new Seafarer(50m, "Ship C", new DateTime(2023, 3, 1), new DateTime(2023, 3, 31))
            ]
        };

        // Act
        var result = _calculator.Calculate(otherDeductionsDetails);

        // Assert: 100 + 250 + 50 = 400
        Assert.Equal(400m, result);
    }

    [Fact]
    public void Calculate_ReturnsCorrectTotal_WhenSomeSeafarersHaveZeroAmountDeducted() // Name adjusted
    {
        // Arrange
        var otherDeductionsDetails = new OtherDeductionsDetails
        {
            Seafarers =
            [
                new Seafarer(100m, "Ship A", new DateTime(2023, 1, 1), new DateTime(2023, 1, 31)),
                new Seafarer(0m, "Ship B", new DateTime(2023, 2, 1), new DateTime(2023, 2, 28)), // Explicitly 0m
                new Seafarer(50m, "Ship C", new DateTime(2023, 3, 1), new DateTime(2023, 3, 31))
            ]
        };

        // Act
        var result = _calculator.Calculate(otherDeductionsDetails);

        // Assert: 100 + 0 + 50 = 150
        Assert.Equal(150m, result);
    }

    // Keep all other test cases as they are, but ensure Seafarer instantiation is correct
    // For these, we typically only need valid placeholder dates/ship names if AmountDeducted is 0 or it's a null list.

    [Fact]
    public void Calculate_ReturnsZero_WhenNoSeafarersArePresent()
    {
        // Arrange
        var otherDeductionsDetails = new OtherDeductionsDetails
        {
            Seafarers = [] // Empty list
        };

        // Act
        var result = _calculator.Calculate(otherDeductionsDetails);

        // Assert
        Assert.Equal(0m, result);
    }

    [Fact]
    public void Calculate_ReturnsZero_WhenSeafarersListIsNull()
    {
        // Arrange
        var otherDeductionsDetails = new OtherDeductionsDetails
        {
            Seafarers = null // Null list
        };

        // Act
        var result = _calculator.Calculate(otherDeductionsDetails);

        // Assert
        Assert.Equal(0m, result);
    }

    [Fact]
    public void Calculate_ReturnsZero_WhenOtherDeductionsDetailsIsNull()
    {
        // Arrange
        OtherDeductionsDetails? otherDeductionsDetails = null; // Entire input is null

        // Act
        var result = _calculator.Calculate(otherDeductionsDetails);

        // Assert
        Assert.Equal(0m, result);
    }

    [Fact]
    public void Calculate_ReturnsCorrectTotal_WhenOnlyOneSeafarerIsPresent()
    {
        // Arrange
        var otherDeductionsDetails = new OtherDeductionsDetails
        {
            Seafarers = [new Seafarer(75m, "Single Ship", new DateTime(2023, 4, 1), new DateTime(2023, 4, 30))]
        };

        // Act
        var result = _calculator.Calculate(otherDeductionsDetails);

        // Assert
        Assert.Equal(75m, result);
    }

    [Fact]
    public void Calculate_ReturnsZero_WhenAllSeafarerAmountsAreZero()
    {
        // Arrange
        var otherDeductionsDetails = new OtherDeductionsDetails
        {
            Seafarers =
            [
                new Seafarer(0m, "Ship X", new DateTime(2023, 5, 1), new DateTime(2023, 5, 10)),
                new Seafarer(0m, "Ship Y", new DateTime(2023, 6, 1), new DateTime(2023, 6, 15))
            ]
        };

        // Act
        var result = _calculator.Calculate(otherDeductionsDetails);

        // Assert
        Assert.Equal(0m, result);
    }
}