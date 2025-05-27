using Swashbuckle.AspNetCore.Annotations;

namespace Shared.Models.IndividualsEmploymentIncomes.Employments
{
    /// <summary>
    /// Represents the benefits received by an employee.
    /// </summary>
    public class BenefitsInKind
    {
        [SwaggerSchema("Accommodation benefit.")]
        public decimal? Accommodation { get; set; }

        [SwaggerSchema("Assets benefit.")]
        public decimal? Assets { get; set; }

        [SwaggerSchema("Asset transfer benefit.")]
        public decimal? AssetTransfer { get; set; }

        [SwaggerSchema("Beneficial loan benefit.")]
        public decimal? BeneficialLoan { get; set; }

        [SwaggerSchema("Car benefit.")]
        public decimal? Car { get; set; }

        [SwaggerSchema("Car fuel benefit.")]
        public decimal? CarFuel { get; set; }

        [SwaggerSchema("Educational services benefit.")]
        public decimal? EducationalServices { get; set; }

        [SwaggerSchema("Entertaining benefit.")]
        public decimal? Entertaining { get; set; }

        [SwaggerSchema("Expenses benefit.")]
        public decimal? Expenses { get; set; }

        [SwaggerSchema("Medical insurance benefit.")]
        public decimal? MedicalInsurance { get; set; }

        [SwaggerSchema("Telephone benefit.")]
        public decimal? Telephone { get; set; }

        [SwaggerSchema("Service benefit.")]
        public decimal? Service { get; set; }

        [SwaggerSchema("Taxable expenses benefit.")]
        public decimal? TaxableExpenses { get; set; }

        [SwaggerSchema("Van benefit.")]
        public decimal? Van { get; set; }

        [SwaggerSchema("Van fuel benefit.")]
        public decimal? VanFuel { get; set; }

        [SwaggerSchema("Mileage benefit.")]
        public decimal? Mileage { get; set; }

        [SwaggerSchema("Non-qualifying relocation expenses benefit.")]
        public decimal? NonQualifyingRelocationExpenses { get; set; }

        [SwaggerSchema("Nursery places benefit.")]
        public decimal? NurseryPlaces { get; set; }

        [SwaggerSchema("Other items benefit.")]
        public decimal? OtherItems { get; set; }

        [SwaggerSchema("Payments on employees' behalf benefit.")]
        public decimal? PaymentsOnEmployeesBehalf { get; set; }

        [SwaggerSchema("Personal incidental expenses benefit.")]
        public decimal? PersonalIncidentalExpenses { get; set; }

        [SwaggerSchema("Qualifying relocation expenses benefit.")]
        public decimal? QualifyingRelocationExpenses { get; set; }

        [SwaggerSchema("Employer-provided professional subscriptions benefit.")]
        public decimal? EmployerProvidedProfessionalSubscriptions { get; set; }

        [SwaggerSchema("Employer-provided services benefit.")]
        public decimal? EmployerProvidedServices { get; set; }

        [SwaggerSchema("Income tax paid by director benefit.")]
        public decimal? IncomeTaxPaidByDirector { get; set; }

        [SwaggerSchema("Travel and subsistence benefit.")]
        public decimal? TravelAndSubsistence { get; set; }

        [SwaggerSchema("Vouchers and credit cards benefit.")]
        public decimal? VouchersAndCreditCards { get; set; }

        [SwaggerSchema("Non-cash benefit.")]
        public decimal? NonCash { get; set; }
    }
}
