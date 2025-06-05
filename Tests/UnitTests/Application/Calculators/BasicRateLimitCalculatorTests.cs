using Application.Calculators;

namespace UnitTests.Application.Calculators
{
    public class BasicRateLimitCalculatorTests
    {
        [Theory]
        [InlineData(37500, 5000, 2000, 44500)] // Base threshold + pension + gift aid
        [InlineData(50000, 0, 0, 50000)]       // No contributions
        [InlineData(12570, 3000, 700, 16270)]  // Mixed values
        public void Calculate_WithValidInputs_ReturnsCorrectBasicRateLimit(
            decimal basicRateThreshold,
            decimal pensionContributions,
            decimal giftAidContributions,
            decimal expectedLimit)
        {
            // Arrange
            var calculator = new BasicRateLimitCalculator();

            // Act
            var result = calculator.Calculate(basicRateThreshold, pensionContributions, giftAidContributions);

            // Assert
            Assert.Equal(expectedLimit, result.Value);
        }

        [Fact]
        public void Calculate_WithNegativeContributions_ThrowsArgumentException()
        {
            // Arrange
            var calculator = new BasicRateLimitCalculator();
            decimal basicRateThreshold = 37500;
            decimal pensionContributions = -5000;
            decimal giftAidContributions = 2000;

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                calculator.Calculate(basicRateThreshold, pensionContributions, giftAidContributions));
        }
    }
}