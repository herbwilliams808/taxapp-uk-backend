using Application.Calculators;

namespace UnitTests.Application.Calculators
{
    public class TotalIncomeCalculatorTests
    {
        [Fact]
        public void Calculate_WithValidInputs_ReturnsCorrectTotalIncome()
        {
            // Arrange
            var calculator = new TotalIncomeCalculator();
            var incomes = new[] { 50000m, 20000m }; // Example incomes
            var propertyIncome = 10000m;

            // Act
            var result = calculator.Calculate(incomes, propertyIncome);

            // Assert
            Assert.Equal(80000m, result);
        }
    }
}