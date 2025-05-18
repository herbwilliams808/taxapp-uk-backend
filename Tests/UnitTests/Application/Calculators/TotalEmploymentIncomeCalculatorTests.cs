using Application.Calculators;

namespace UnitTests.Application.Calculators
{
    public class TotalEmploymentIncomeCalculatorTests
    {
        [Fact]
        public void Calculate_WithValidInputs_ReturnsCorrectTotalEmploymentIncome()
        {
            // Arrange
            var calculator = new TotalEmploymentIncomeCalculator();
            var employmentIncomes = new[] { 50000m, 20000m }; // Example incomes

            // Act
            var result = calculator.Calculate(employmentIncomes);

            // Assert
            Assert.Equal(70000, result);
        }
    }
}