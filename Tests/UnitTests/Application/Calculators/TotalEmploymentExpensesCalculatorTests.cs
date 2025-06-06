using Shared.Models.Incomes; // Contains IncomeSourcesDetails, EmploymentsAndFinancialDetails, BenefitsInKind
using Shared.Models.IndividualsEmploymentIncomes.Employments; // Contains BenefitsInKind (nested in EmploymentAndFinancialDetails)
using Application.Calculators; // To reference the concrete calculator
using Application.Interfaces.Calculators; // To reference the calculator interface

// For [Fact] and Assert

namespace UnitTests.Application.Calculators;

public class TotalEmploymentExpensesCalculatorTests
{
    private readonly ITotalEmploymentExpensesCalculator _calculator;

    public TotalEmploymentExpensesCalculatorTests()
    {
        _calculator = new TotalEmploymentExpensesCalculator();
    }

    [Fact]
    public void CalculateTotalExpenses_WithMultipleEmploymentsHavingExpenses_ReturnsCorrectSum()
    {
        // Arrange
        var incomeSources = new IncomeSourcesDetails
        {
            EmploymentsAndFinancialDetails =
            [
                new EmploymentAndFinancialDetails
                {
                    Employer = new Employer { EmployerName = "Employer A" },
                    BenefitsInKind = new BenefitsInKind { Expenses = 100m }
                },
                new EmploymentAndFinancialDetails
                {
                    Employer = new Employer { EmployerName = "Employer B" },
                    BenefitsInKind = new BenefitsInKind { Expenses = 200m }
                },
                new EmploymentAndFinancialDetails
                {
                    Employer = new Employer { EmployerName = "Employer C" },
                    BenefitsInKind = new BenefitsInKind { Expenses = 300m }
                }
            ]
        };

        // Act
        var total = _calculator.Calculate(incomeSources);

        // Assert: 100 + 200 + 300 = 600
        Assert.Equal(600m, total);
    }

    [Fact]
    public void CalculateTotalExpenses_WithSomeEmploymentsHavingNullExpenses_ReturnsCorrectSum()
    {
        // Arrange
        var incomeSources = new IncomeSourcesDetails
        {
            EmploymentsAndFinancialDetails =
            [
                new EmploymentAndFinancialDetails
                {
                    Employer = new Employer { EmployerName = "Employer A" },
                    BenefitsInKind = new BenefitsInKind { Expenses = 100m }
                },
                new EmploymentAndFinancialDetails
                {
                    Employer = new Employer { EmployerName = "Employer B" },
                    BenefitsInKind = new BenefitsInKind { Expenses = null } // Explicitly null expenses
                },
                new EmploymentAndFinancialDetails
                {
                    Employer = new Employer { EmployerName = "Employer C" },
                    BenefitsInKind = new BenefitsInKind { Expenses = 300m }
                }
            ]
        };

        // Act
        var total = _calculator.Calculate(incomeSources);

        // Assert: 100 + 0 (for null) + 300 = 400
        Assert.Equal(400m, total);
    }

    [Fact]
    public void CalculateTotalExpenses_WithSomeEmploymentsHavingNullBenefitsInKind_ReturnsCorrectSum()
    {
        // Arrange
        var incomeSources = new IncomeSourcesDetails
        {
            EmploymentsAndFinancialDetails =
            [
                new EmploymentAndFinancialDetails
                {
                    Employer = new Employer { EmployerName = "Employer A" },
                    BenefitsInKind = new BenefitsInKind { Expenses = 100m }
                },
                new EmploymentAndFinancialDetails
                {
                    Employer = new Employer { EmployerName = "Employer B" },
                    BenefitsInKind = null // Explicitly null BenefitsInKind
                },
                new EmploymentAndFinancialDetails
                {
                    Employer = new Employer { EmployerName = "Employer C" },
                    BenefitsInKind = new BenefitsInKind { Expenses = 300m }
                }
            ]
        };

        // Act
        var total = _calculator.Calculate(incomeSources);

        // Assert: 100 + 0 (for null BenefitsInKind) + 300 = 400
        Assert.Equal(400m, total);
    }


    [Fact]
    public void CalculateTotalExpenses_WithEmptyEmploymentsList_ReturnsZero()
    {
        // Arrange
        var incomeSources = new IncomeSourcesDetails
        {
            EmploymentsAndFinancialDetails = new() // Empty list
        };

        // Act
        var total = _calculator.Calculate(incomeSources);

        // Assert
        Assert.Equal(0m, total);
    }

    [Fact]
    public void CalculateTotalExpenses_WithNullEmploymentsList_ReturnsZero()
    {
        // Arrange
        var incomeSources = new IncomeSourcesDetails
        {
            EmploymentsAndFinancialDetails = null // Null list
        };

        // Act
        var total = _calculator.Calculate(incomeSources);

        // Assert
        Assert.Equal(0m, total);
    }

    [Fact]
    public void CalculateTotalExpenses_WithNullIncomeSources_ReturnsZero()
    {
        // Arrange
        IncomeSourcesDetails? incomeSources = null;

        // Act
        var total = _calculator.Calculate(incomeSources);

        // Assert
        Assert.Equal(0m, total);
    }

    [Fact]
    public void CalculateTotalExpenses_WithNoExpensesSet_ReturnsZero()
    {
        // Arrange
        var incomeSources = new IncomeSourcesDetails
        {
            EmploymentsAndFinancialDetails =
            [
                new EmploymentAndFinancialDetails
                {
                    Employer = new Employer { EmployerName = "Employer A" },
                    // No BenefitsInKind or Expenses property set, so it defaults to null
                },
                new EmploymentAndFinancialDetails
                {
                    Employer = new Employer { EmployerName = "Employer B" },
                    BenefitsInKind = new BenefitsInKind { Expenses = null } // Explicitly null
                }
            ]
        };

        // Act
        var total = _calculator.Calculate(incomeSources);

        // Assert
        Assert.Equal(0m, total);
    }
}