using Shared.Models.Expenses;

namespace UnitTests.Application.Calculators
{
    public class TotalEmploymentExpensesCalculatorTests
    {
        [Fact]
        public void CalculateTotalExpenses_WithAllPropertiesSet_ReturnsCorrectSum()
        {
            // Arrange
            var expenses = new ExpensesDetails
            {
                BusinessTravelCosts = 100,
                JobExpenses = 200,
                FlatRateJobExpenses = 300,
                ProfessionalSubscriptions = 400,
                HotelAndMealExpenses = 500,
                OtherAndCapitalAllowances = 600,
                VehicleExpenses = 700,
                MileageAllowanceRelief = 800
            };

            // Act
            var total = CalculateTotalExpenses(expenses);

            // Assert
            Assert.Equal(3600, total); // Sum of all property values
        }

        [Fact]
        public void CalculateTotalExpenses_WithSomePropertiesSetToNull_ReturnsCorrectSum()
        {
            // Arrange
            var expenses = new ExpensesDetails
            {
                BusinessTravelCosts = 100,
                JobExpenses = null,
                FlatRateJobExpenses = 300,
                ProfessionalSubscriptions = null,
                HotelAndMealExpenses = 500,
                OtherAndCapitalAllowances = 600,
                VehicleExpenses = 700,
                MileageAllowanceRelief = null
            };

            // Act
            var total = CalculateTotalExpenses(expenses);

            // Assert
            Assert.Equal(2200, total); // Sum of non-null property values
        }

        [Fact]
        public void CalculateTotalExpenses_WithAllPropertiesNull_ReturnsZero()
        {
            // Arrange
            var expenses = new ExpensesDetails();

            // Act
            var total = CalculateTotalExpenses(expenses);

            // Assert
            Assert.Equal(0, total); // All properties are null, so the sum is 0
        }

        private decimal CalculateTotalExpenses(ExpensesDetails expensesDetails)
        {
            if (expensesDetails == null)
                return 0;

            return expensesDetails.GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(decimal?))
                .Select(p => (decimal?)p.GetValue(expensesDetails) ?? 0)
                .Sum();
        }
    }
}
