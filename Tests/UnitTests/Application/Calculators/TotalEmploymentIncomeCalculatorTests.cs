using Shared.Models.Incomes;
using Shared.Models.IndividualsEmploymentIncomes.Employments;
using Shared.Models.IndividualsEmploymentIncomes.OtherEmploymentIncome.LumpSum;
using Shared.Models.OtherDeductions;
using Shared.Models.IndividualsForeignIncome;
using Shared.Models.IndividualsReliefs.ForeignReliefs;
using Application.Calculators;
using Shared.Models.IndividualsEmploymentIncomes.NonPayeEmploymentIncome;
using Shared.Models.IndividualsEmploymentIncomes.OtherEmploymentIncome;

namespace UnitTests.Application.Calculators
{
    public class TotalEmploymentIncomeCalculatorTests
    {
        [Fact]
        public void Calculate_ReturnsCorrectTotalEmploymentIncome()
        {
            // Arrange
            var incomes = new IncomeDetails
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

            var otherDeductions = new OtherDeductionsDetails();
            var individualsForeignIncome = new IndividualsForeignIncomeDetails
            {
                ForeignEarnings = new ForeignEarnings(earningsNotTaxableUk: 400m)
            };
            var foreignReliefs = new ForeignReliefsDetails
            {
                ForeignTaxForFtcrNotClaimed = new ForeignTaxForFtcrNotClaimed { Amount = 50m }
            };

            var calculator = new TotalEmploymentIncomeCalculator();

            // Act
            var result = calculator.Calculate(incomes, individualsForeignIncome, foreignReliefs);

            // Expected calculation:
            // Sum Payable: 1500 + 2500 = 4000
            // Tips: 500
            // Taxable Lump Sums: 300 + 700 = 1000
            // Employer Financed Retirement Benefits: 100 + 200 = 300
            // Benefits in Kind (mocked): 800
            // ExpensesDetails (mocked): 600
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
