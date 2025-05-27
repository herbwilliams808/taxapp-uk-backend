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
            const decimal employmentIncome = 72000m;
            const decimal benefitsInKind = 1000m;
            const decimal employmentExpenses = 500m;
            const decimal otherDeductions = 400m;
            const decimal profitFromProperties = 10000m;

            // Act
            var result = calculator.Calculate(employmentIncome, benefitsInKind, employmentExpenses, otherDeductions, profitFromProperties);

            // Assert
            Assert.Equal(82100, result);
        }
    }
}