using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.Incomes.NonSavingsIncomes.Employments
{
    public class BenefitsInKind
    {
        [SwaggerSchema(Description = "Medical insurance benefits.")]
        [Range(0, double.MaxValue, ErrorMessage = "MedicalInsurance must be non-negative.")]
        public decimal? MedicalInsurance { get; set; }

        [SwaggerSchema(Description = "Car benefits.")]
        [Range(0, double.MaxValue, ErrorMessage = "CarBenefits must be non-negative.")]
        public decimal? CarBenefits { get; set; }

        [SwaggerSchema(Description = "Fuel benefits.")]
        [Range(0, double.MaxValue, ErrorMessage = "FuelBenefits must be non-negative.")]
        public decimal? FuelBenefits { get; set; }

        [SwaggerSchema(Description = "Educational services benefits.")]
        [Range(0, double.MaxValue, ErrorMessage = "EducationalServices must be non-negative.")]
        public decimal? EducationalServices { get; set; }

        [SwaggerSchema(Description = "Non-cash vouchers.")]
        [Range(0, double.MaxValue, ErrorMessage = "NonCashVouchers must be non-negative.")]
        public decimal? NonCashVouchers { get; set; }

        [SwaggerSchema(Description = "Accommodation benefits.")]
        [Range(0, double.MaxValue, ErrorMessage = "Accommodation must be non-negative.")]
        public decimal? Accommodation { get; set; }

        [SwaggerSchema(Description = "Assets benefits.")]
        [Range(0, double.MaxValue, ErrorMessage = "Assets must be non-negative.")]
        public decimal? Assets { get; set; }

        [SwaggerSchema(Description = "Asset transfers.")]
        [Range(0, double.MaxValue, ErrorMessage = "AssetTransfers must be non-negative.")]
        public decimal? AssetTransfers { get; set; }

        [SwaggerSchema(Description = "Credit tokens.")]
        [Range(0, double.MaxValue, ErrorMessage = "CreditTokens must be non-negative.")]
        public decimal? CreditTokens { get; set; }

        [SwaggerSchema(Description = "Debit cards.")]
        [Range(0, double.MaxValue, ErrorMessage = "DebitCards must be non-negative.")]
        public decimal? DebitCards { get; set; }

        [SwaggerSchema(Description = "Employer provided services.")]
        [Range(0, double.MaxValue, ErrorMessage = "EmployerProvidedServices must be non-negative.")]
        public decimal? EmployerProvidedServices { get; set; }

        [SwaggerSchema(Description = "Other items.")]
        [Range(0, double.MaxValue, ErrorMessage = "OtherItems must be non-negative.")]
        public decimal? OtherItems { get; set; }

        [SwaggerSchema(Description = "Payments on employees' behalf.")]
        [Range(0, double.MaxValue, ErrorMessage = "PaymentsOnEmployeesBehalf must be non-negative.")]
        public decimal? PaymentsOnEmployeesBehalf { get; set; }

        [SwaggerSchema(Description = "Personal incurrred expenses.")]
        [Range(0, double.MaxValue, ErrorMessage = "PersonalIncurrredExpenses must be non-negative.")]
        public decimal? PersonalIncurrredExpenses { get; set; }

        [SwaggerSchema(Description = "Qualifying relocation expenses.")]
        [Range(0, double.MaxValue, ErrorMessage = "QualifyingRelocationExpenses must be non-negative.")]
        public decimal? QualifyingRelocationExpenses { get; set; }

        [SwaggerSchema(Description = "Employer provided professional subscriptions.")]
        [Range(0, double.MaxValue, ErrorMessage = "EmployerProvidedProfessionalSubscriptions must be non-negative.")]
        public decimal? EmployerProvidedProfessionalSubscriptions { get; set; }

        [SwaggerSchema(Description = "Mileage allowance.")]
        [Range(0, double.MaxValue, ErrorMessage = "MileageAllowance must be non-negative.")]
        public decimal? MileageAllowance { get; set; }

        [SwaggerSchema(Description = "Vehicles and vehicle fuel benefits.")]
        [Range(0, double.MaxValue, ErrorMessage = "VehiclesAndVehicleFuel must be non-negative.")]
        public decimal? VehiclesAndVehicleFuel { get; set; }
    }
}
