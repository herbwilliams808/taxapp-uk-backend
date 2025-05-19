using Shared.Models.Incomes;

public class TotalEmploymentBenefitsCalculator
{
    public decimal Calculate(Incomes incomes)
    {
        if (incomes?.Employments == null)
            return 0;

        return incomes.Employments
            .SelectMany(employment => new[]
            {
                employment.BenefitsInKind?.MedicalInsurance ?? 0,
                employment.BenefitsInKind?.CarBenefits ?? 0,
                employment.BenefitsInKind?.FuelBenefits ?? 0,
                employment.BenefitsInKind?.EducationalServices ?? 0,
                employment.BenefitsInKind?.NonCashVouchers ?? 0,
                employment.BenefitsInKind?.Accommodation ?? 0,
                employment.BenefitsInKind?.Assets ?? 0,
                employment.BenefitsInKind?.AssetTransfers ?? 0,
                employment.BenefitsInKind?.CreditTokens ?? 0,
                employment.BenefitsInKind?.DebitCards ?? 0,
                employment.BenefitsInKind?.EmployerProvidedServices ?? 0,
                employment.BenefitsInKind?.OtherItems ?? 0,
                employment.BenefitsInKind?.PaymentsOnEmployeesBehalf ?? 0,
                employment.BenefitsInKind?.PersonalIncurrredExpenses ?? 0,
                employment.BenefitsInKind?.QualifyingRelocationExpenses ?? 0,
                employment.BenefitsInKind?.EmployerProvidedProfessionalSubscriptions ?? 0,
                employment.BenefitsInKind?.MileageAllowance ?? 0,
                employment.BenefitsInKind?.VehiclesAndVehicleFuel ?? 0
            })
            .Sum();
    }
}