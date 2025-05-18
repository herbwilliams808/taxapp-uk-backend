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
            var employmentIncomes = new[] { 50000m, 20000m }; // Example incomes
            var nonPayeEmploymentIncome = 2000;
            var propertyIncome = 10000m;

            // Act
            var result = calculator.Calculate(employmentIncomes, nonPayeEmploymentIncome, propertyIncome);

            // Assert
            Assert.Equal(82000m, result);
        }
    }
}