using Application.Calculators;
using Shared.Models.Incomes;
using Shared.Models.Incomes.NonSavingsIncomes.Employments;

namespace UnitTests.Application.Calculators
{
    public class TotalEmploymentIncomeCalculatorTests
    {
        [Fact]
        public void Calculate_WithValidInputs_ReturnsCorrectTotalEmploymentIncome()
        {
            // Arrange
            var calculator = new TotalEmploymentIncomeCalculator();
            
            var incomes = new Incomes
            {
                Employments =
                [
                    new Employment { Pay = new Pay(taxablePayToDate: 20000m) },
                    new Employment { Pay = new Pay(taxablePayToDate: 50000m) }
                ]
            };
            
            // Act
            var result = calculator.Calculate(incomes);

            // Assert
            Assert.Equal(70000, result);
        }
    }
}