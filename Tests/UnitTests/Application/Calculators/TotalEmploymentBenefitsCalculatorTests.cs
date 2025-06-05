using Application.Calculators;
using Shared.Models.Incomes;
using Shared.Models.IndividualsEmploymentIncomes.Employments;

namespace UnitTests.Application.Calculators;

public class TotalEmploymentBenefitsCalculatorTests
{
    [Fact]
    public void CalculateTotalBenefits_ReturnsCorrectSum_WhenAllPropertiesArePopulated()
    {
        // Arrange
        var benefits = new BenefitsInKind
        {
            Accommodation = 1000,
            Assets = 200,
            AssetTransfer = 300,
            BeneficialLoan = 150,
            Car = 5000,
            CarFuel = 1000,
            EducationalServices = 800,
            Entertaining = 400,
            Expenses = 200,
            MedicalInsurance = 1200,
            Telephone = 150,
            Service = 300,
            TaxableExpenses = 700,
            Van = 600,
            VanFuel = 400,
            Mileage = 250,
            NonQualifyingRelocationExpenses = 150,
            NurseryPlaces = 350,
            OtherItems = 500,
            PaymentsOnEmployeesBehalf = 100,
            PersonalIncidentalExpenses = 50,
            QualifyingRelocationExpenses = 200,
            EmployerProvidedProfessionalSubscriptions = 150,
            EmployerProvidedServices = 300,
            IncomeTaxPaidByDirector = 400,
            TravelAndSubsistence = 800,
            VouchersAndCreditCards = 1000,
            NonCash = 600
        };

        var incomes = new IncomeSources
        {
            EmploymentsAndFinancialDetails = new List<EmploymentAndFinancialDetails>
            {
                new EmploymentAndFinancialDetails
                {
                    BenefitsInKind = benefits,
                    Employer = new Employer { EmployerName = "Test Employer" }
                }
            }
        };

        var calculator = new TotalEmploymentBenefitsCalculator();

        // Act
        var result = calculator.Calculate(incomes);

        // Assert
        Assert.Equal(17250, result); // Sum of all populated properties
    }

    [Fact]
    public void CalculateTotalBenefits_IgnoresNullValues()
    {
        // Arrange
        var benefits = new BenefitsInKind
        {
            Accommodation = null,
            Car = 5000,
            MedicalInsurance = null,
            OtherItems = 500
        };

        var incomes = new IncomeSources
        {
            EmploymentsAndFinancialDetails = new List<EmploymentAndFinancialDetails>
            {
                new EmploymentAndFinancialDetails
                {
                    BenefitsInKind = benefits,
                    Employer = new Employer { EmployerName = "Test Employer" }
                }
            }
        };

        var calculator = new TotalEmploymentBenefitsCalculator();

        // Act
        var result = calculator.Calculate(incomes);

        // Assert
        Assert.Equal(5500, result); // Only non-null values contribute
    }

    [Fact]
    public void CalculateTotalBenefits_HandlesMultipleEmploymentsWithDifferentProperties()
    {
        // Arrange
        var incomes = new IncomeSources
        {
            EmploymentsAndFinancialDetails = new List<EmploymentAndFinancialDetails>
            {
                new EmploymentAndFinancialDetails
                {
                    BenefitsInKind = new BenefitsInKind
                    {
                        Car = 3000,
                        CarFuel = 1000
                    },
                    Employer = new Employer { EmployerName = "First Employer" }
                },
                new EmploymentAndFinancialDetails
                {
                    BenefitsInKind = new BenefitsInKind
                    {
                        MedicalInsurance = 2000,
                        OtherItems = 700
                    },
                    Employer = new Employer { EmployerName = "Second Employer" }
                }
            }
        };

        var calculator = new TotalEmploymentBenefitsCalculator();

        // Act
        var result = calculator.Calculate(incomes);

        // Assert
        Assert.Equal(6700, result); // Sum of all valid benefits across employments
    }

    [Fact]
    public void CalculateTotalBenefits_ReturnsZero_WhenNoBenefitsArePresent()
    {
        // Arrange
        var incomes = new IncomeSources
        {
            EmploymentsAndFinancialDetails = new List<EmploymentAndFinancialDetails>
            {
                new EmploymentAndFinancialDetails
                {
                    BenefitsInKind = null,
                    Employer = new Employer { EmployerName = "First Employer" }
                }, // No benefits in this employment
                new EmploymentAndFinancialDetails
                {
                    BenefitsInKind = new BenefitsInKind(),
                    Employer = new Employer { EmployerName = "Second Employer" }
                } // Empty benefits
            }
        };

        var calculator = new TotalEmploymentBenefitsCalculator();

        // Act
        var result = calculator.Calculate(incomes);

        // Assert
        Assert.Equal(0, result); // No contributions
    }
}