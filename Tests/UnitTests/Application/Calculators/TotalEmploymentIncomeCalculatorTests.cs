using Moq;
using Shared.Models.Incomes;
using Shared.Models.Incomes.NonSavingsIncomes.Employments;
using Shared.Models.Incomes.NonSavingsIncomes.OtherEmploymentIncome.LumpSum;
using Shared.Models.OtherDeductions;
using Shared.Models.IndividualsForeignIncome;
using Shared.Models.IndividualsReliefs.ForeignReliefs;
using Application.Calculators;
using Shared.Models.Incomes.NonSavingsIncomes.NonPayeEmploymentIncome;
using Shared.Models.Incomes.NonSavingsIncomes.OtherEmploymentIncome;

namespace UnitTests.Application.Calculators
{
    public class TotalEmploymentIncomeCalculatorTests
    {
        [Fact]
        public void Calculate_ReturnsCorrectTotalEmploymentIncome()
        {
            // Arrange
            var incomes = new Incomes
            {
                Employments = new()
                {
                    new Employment
                    {
                        Pay = new Pay { TaxablePayToDate = 1500m },
                        Employer = new Employer { EmployerName = "Employer1" }
                    },
                    new Employment
                    {
                        Pay = new Pay { TaxablePayToDate = 2500m },
                        Employer = new Employer { EmployerName = "Employer2" }
                    }
                },
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

            var otherDeductions = new OtherDeductions();
            var individualsForeignIncome = new IndividualsForeignIncome
            {
                ForeignEarnings = new ForeignEarnings(earningsNotTaxableUk: 400m)
            };
            var foreignReliefs = new ForeignReliefs
            {
                ForeignTaxForFtcrNotClaimed = new ForeignTaxForFtcrNotClaimed { Amount = 50m }
            };

            // Mock dependencies
            var mockBenefitsCalculator = new Mock<TotalEmploymentBenefitsCalculator>();
            mockBenefitsCalculator.Setup(m => m.Calculate(incomes)).Returns(800m);

            var mockExpensesCalculator = new Mock<TotalEmploymentExpensesCalculator>();
            mockExpensesCalculator.Setup(m => m.Calculate(incomes)).Returns(600m);

            var mockOtherDeductionsCalculator = new Mock<TotalOtherDeductionsCalculator>();
            mockOtherDeductionsCalculator.Setup(m => m.Calculate(otherDeductions)).Returns(200m);

            var calculator = new TotalEmploymentIncomeCalculator(
                mockBenefitsCalculator.Object,
                mockExpensesCalculator.Object,
                mockOtherDeductionsCalculator.Object);

            // Act
            var result = calculator.Calculate(incomes, otherDeductions, individualsForeignIncome, foreignReliefs);

            // Expected calculation:
            // Sum Payable: 1500 + 2500 = 4000
            // Tips: 500
            // Taxable Lump Sums: 300 + 700 = 1000
            // Employer Financed Retirement Benefits: 100 + 200 = 300
            // Benefits in Kind (mocked): 800
            // Expenses (mocked): 600
            // Other Deductions (mocked): 200
            // Earnings Not Taxable UK: 400
            // Foreign Tax For FTCR Not Claimed: 50
            //
            // Total = 4000 + 500 + 1000 + 300 + 800 - 600 - 200 - 400 - 50 = 5350

            // Assert
            Assert.Equal(5350m, result);
        }
    }
}
