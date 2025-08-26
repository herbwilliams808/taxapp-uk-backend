// Make sure IncomeSourcesDetails is here
using Application.Calculators;
using Core.Interfaces.Calculators;
using Core.Models.Incomes;
using Core.Models.IndividualsEmploymentIncomes.Employments;
using Core.Models.IndividualsEmploymentIncomes.NonPayeEmploymentIncome;
using Core.Models.IndividualsEmploymentIncomes.OtherEmploymentIncome;
using Core.Models.IndividualsEmploymentIncomes.OtherEmploymentIncome.LumpSum;
using Core.Models.IndividualsForeignIncome;
using Core.Models.IndividualsReliefs.ForeignReliefs;
using Core.Models.OtherDeductions; // For the concrete implementation
// For the interface

// Needed for Sum()

// For [Fact]

namespace UnitTests.Application.Calculators;

public class TotalEmploymentIncomeCalculatorTests
{
    [Fact]
    public void Calculate_ReturnsCorrectTotalEmploymentIncome()
    {
        // Arrange
        var incomes = new IncomeSourcesDetails
        {
            EmploymentsAndFinancialDetails =
            [
                new EmploymentAndFinancialDetails
                {
                    Pay = new Pay { TaxablePayToDate = 1500m },
                    Employer = new Employer { EmployerName = "Employer1" }
                },

                new EmploymentAndFinancialDetails
                {
                    Pay = new Pay { TaxablePayToDate = 2500m },
                    Employer = new Employer { EmployerName = "Employer2" }
                }
            ],
            NonPayeEmploymentIncome = new NonPayeEmploymentIncome
            {
                Tips = 500m
            },
            OtherEmploymentIncome = new OtherEmploymentIncome
            {
                LumpSums = new()
                {
                    new LumpSum
                    {
                        TaxableLumpSumsAndCertainIncome = new TaxableLumpSumsAndCertainIncome { Amount = 300m },
                        BenefitFromEmployerFinancedRetirementScheme = new BenefitFromEmployerFinancedRetirementScheme { Amount = 100m }
                    },
                    new LumpSum
                    {
                        TaxableLumpSumsAndCertainIncome = new TaxableLumpSumsAndCertainIncome { Amount = 700m },
                        BenefitFromEmployerFinancedRetirementScheme = new BenefitFromEmployerFinancedRetirementScheme { Amount = 200m }
                    }
                }
            }
        };

        var otherDeductions = new OtherDeductionsDetails(); // Still present in input, but not used by this calculator per its current scope.
        var individualsForeignIncome = new IndividualsForeignIncomeDetails
        {
            ForeignEarnings = new ForeignEarnings(earningsNotTaxableUk: 400m)
        };
        var foreignReliefs = new ForeignReliefsDetails
        {
            ForeignTaxForFtcrNotClaimed = new ForeignTaxForFtcrNotClaimed { Amount = 50m }
        };

        // Declare as interface, instantiate concrete class
        ITotalEmploymentIncomeCalculator calculator = new TotalEmploymentIncomeCalculator(); // <<< Changed type to interface

        // Act
        var result = calculator.Calculate(incomes, individualsForeignIncome, foreignReliefs);

        // Expected calculation logic remains the same based on the fixed inputs and calculator scope
        // (4000 + 500 + 1000 + 300) - (400 + 50) = 5800 - 450 = 5350

        // Assert
        Assert.Equal(5350m, result);
    }

    [Fact]
    public void Calculate_ReturnsZero_WhenAllInputsAreNullOrEmpty()
    {
        // Arrange
        ITotalEmploymentIncomeCalculator calculator = new TotalEmploymentIncomeCalculator();

        // Act
        var result = calculator.Calculate(null, null, null);

        // Assert
        Assert.Equal(0m, result);
    }

    [Fact]
    public void Calculate_HandlesPartialInputs_Correctly()
    {
        // Arrange
        var incomes = new IncomeSourcesDetails // <<< CORRECTED: Using IncomeSourcesDetails
        {
            EmploymentsAndFinancialDetails =
            [
                new EmploymentAndFinancialDetails
                {
                    Pay = new Pay { TaxablePayToDate = 1000m },
                    Employer = new Employer { EmployerName = "Employer1" }

                }
            ]
        };
        var individualsForeignIncome = new IndividualsForeignIncomeDetails(); // No foreign earnings
        var foreignReliefs = new ForeignReliefsDetails(); // No foreign reliefs

        ITotalEmploymentIncomeCalculator calculator = new TotalEmploymentIncomeCalculator();

        // Act
        var result = calculator.Calculate(incomes, individualsForeignIncome, foreignReliefs);

        // Assert
        Assert.Equal(1000m, result);
    }
}