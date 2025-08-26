using Swashbuckle.AspNetCore.Annotations;

namespace Core.Models.Expenses
{
    /// <summary>
    /// Represents various types of expenses incurred.
    /// </summary>
    public class ExpensesDetails
    {
        [SwaggerSchema("Costs related to business travel.")]
        public decimal? BusinessTravelCosts { get; set; }

        [SwaggerSchema("ExpensesDetails related to the job, such as tools or equipment.")]
        public decimal? JobExpenses { get; set; }

        [SwaggerSchema("Flat rate expenses for the job.")]
        public decimal? FlatRateJobExpenses { get; set; }

        [SwaggerSchema("Costs related to professional subscriptions.")]
        public decimal? ProfessionalSubscriptions { get; set; }

        [SwaggerSchema("ExpensesDetails for hotels and meals while traveling.")]
        public decimal? HotelAndMealExpenses { get; set; }

        [SwaggerSchema("Other expenses, including capital allowances.")]
        public decimal? OtherAndCapitalAllowances { get; set; }

        [SwaggerSchema("Vehicle-related expenses.")]
        public decimal? VehicleExpenses { get; set; }

        [SwaggerSchema("Relief for mileage allowance.")]
        public decimal? MileageAllowanceRelief { get; set; }
    }
}