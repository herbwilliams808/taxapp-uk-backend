using Application.Calculators;
using Shared.Models.OtherDeductions;

namespace UnitTests.Application.Calculators
{
    public class TotalOtherDeductionsCalculatorTests
    {
        private readonly TotalOtherDeductionsCalculator _calculator = new();

        [Fact]
        public void Calculate_ShouldReturnZero_WhenOtherDeductionsIsNull()
        {
            // Arrange
            var otherDeductions = new OtherDeductions();

            // Act
            var result = _calculator.Calculate(otherDeductions);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void Calculate_ShouldReturnZero_WhenSeafarersIsEmpty()
        {
            // Arrange
            var otherDeductions = new OtherDeductions
            {
                Seafarers = []
            };

            // Act
            var result = _calculator.Calculate(otherDeductions);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void Calculate_ShouldSumAmountsCorrectly_WhenSeafarersHaveDeductions()
        {
            // Arrange
            var otherDeductions = new OtherDeductions
            {
                Seafarers = 
                [
                    new Seafarer("Ship1", DateTime.Today, DateTime.Today) { AmountDeducted = 100m },
                    new Seafarer("Ship2", DateTime.Today, DateTime.Today) { AmountDeducted = 200m },
                    new Seafarer("Ship3", DateTime.Today, DateTime.Today) { AmountDeducted = 300m }
                ]
            };

            // Act
            var result = _calculator.Calculate(otherDeductions);

            // Assert
            Assert.Equal(600m, result);
        }

        [Fact]
        public void Calculate_ShouldHandleNullableSeafarerAmounts()
        {
            // Arrange
            var otherDeductions = new OtherDeductions
            {
                Seafarers = 
                [
                    new Seafarer("Ship1", DateTime.Today, DateTime.Today) { AmountDeducted = 100m },
                    new Seafarer("Ship1", DateTime.Today, DateTime.Today) {  },
                    new Seafarer("Ship1", DateTime.Today, DateTime.Today) { AmountDeducted = 300m }
                ]
            };

            // Act
            var result = _calculator.Calculate(otherDeductions);

            // Assert
            Assert.Equal(400m, result);
        }

        [Fact]
        public void Calculate_ShouldReturnZero_WhenSeafarersIsNull()
        {
            // Arrange
            var otherDeductions = new OtherDeductions
            {
                Seafarers = null
            };

            // Act
            var result = _calculator.Calculate(otherDeductions);

            // Assert
            Assert.Equal(0m, result);
        }
    }
}
