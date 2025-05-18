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
            var employmentIncome = 70000m;
            var nonPayeEmploymentIncome = 2000m;
            var propertyIncome = 10000m;

            // Act
            var result = calculator.Calculate(employmentIncome, nonPayeEmploymentIncome, propertyIncome);

            // Assert
            Assert.Equal(82000m, result);
        }
    }
}