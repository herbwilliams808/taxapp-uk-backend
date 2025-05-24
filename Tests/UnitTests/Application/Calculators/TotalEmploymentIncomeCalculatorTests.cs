using Xunit;
using Application.Calculators;
using Shared.Models.Incomes;
using Shared.Models.Incomes.NonSavingsIncomes.Employments;
using Shared.Models.Incomes.NonSavingsIncomes.OtherEmploymentIncome.LumpSum;
using System.Collections.Generic;
using Shared.Models.Incomes.NonSavingsIncomes.NonPayeEmploymentIncome;
using Shared.Models.Incomes.NonSavingsIncomes.OtherEmploymentIncome;

namespace UnitTests.Application.Calculators
{
    public class TotalEmploymentIncomeCalculatorTests
    {
        private readonly TotalEmploymentIncomeCalculator _calculator = new();

        [Fact]
        public void Calculate_ShouldReturnZero_WhenIncomesAreNull()
        {
            // Arrange
            var incomes = new Incomes();

            // Act
            var result = _calculator.Calculate(incomes);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void Calculate_ShouldSumEmploymentIncomeCorrectly()
        {
            // Arrange
            var incomes = new Incomes
            {
                Employments =
                [
                    new Employment
                    {
                        Pay = new Pay
                        {
                            TaxablePayToDate = 1000m
                        },
                        Employer = new Employer
                        {
                            EmployerName = "Employer 1"
                        }
                    },
                    new Employment
                    {
                        Pay = new Pay
                        {
                            TaxablePayToDate = 2000m
                        },
                        Employer = new Employer
                        {
                            EmployerName = "Employer 2"
                        }
                    }
                ]
            };

            // Act
            var result = _calculator.Calculate(incomes);

            // Assert
            Assert.Equal(3000m, result);
        }

        [Fact]
        public void Calculate_ShouldIncludeNonPayeEmploymentIncome()
        {
            // Arrange
            var incomes = new Incomes
            {
                NonPayeEmploymentIncome = new NonPayeEmploymentIncome { Tips = 500m }
            };

            // Act
            var result = _calculator.Calculate(incomes);

            // Assert
            Assert.Equal(500m, result);
        }

        [Fact]
        public void Calculate_ShouldIncludeTaxableLumpSums()
        {
            // Arrange
            var incomes = new Incomes
            {
                OtherEmploymentIncome = new OtherEmploymentIncome
                {
                    LumpSums =
                    [
                        new LumpSum
                        {
                            TaxableLumpSumsAndCertainIncome = new TaxableLumpSumsAndCertainIncome { Amount = 300m }
                        },
                        new LumpSum
                        {
                            TaxableLumpSumsAndCertainIncome = new TaxableLumpSumsAndCertainIncome { Amount = 700m }
                        }
                    ]
                }
            };

            // Act
            var result = _calculator.Calculate(incomes);

            // Assert
            Assert.Equal(1000m, result);
        }

        [Fact]
        public void Calculate_ShouldIncludeEmployerFinancedRetirementSchemeBenefits()
        {
            // Arrange
            var incomes = new Incomes
            {
                OtherEmploymentIncome = new OtherEmploymentIncome
                {
                    LumpSums =
                    [
                        new LumpSum
                        {
                            BenefitFromEmployerFinancedRetirementScheme = new BenefitFromEmployerFinancedRetirementScheme { Amount = 400m }
                        },
                        new LumpSum
                        {
                            BenefitFromEmployerFinancedRetirementScheme = new BenefitFromEmployerFinancedRetirementScheme { Amount = 300m }
                        }
                    ]
                }
            };

            // Act
            var result = _calculator.Calculate(incomes);

            // Assert
            Assert.Equal(700m, result);
        }

        [Fact]
        public void Calculate_ShouldReturnSumOfAllComponents()
        {
            // Arrange
            var incomes = new Incomes
            {
                Employments =
                [
                    new Employment
                    {
                        Pay = new Pay
                        {
                            TaxablePayToDate = 1000m
                        },
                        Employer = new Employer
                        {
                            EmployerName = "Employer 1"
                        }
                    },
                    new Employment
                    {
                        Pay = new Pay
                        {
                            TaxablePayToDate = 2000m
                        },
                        Employer = new Employer
                        {
                            EmployerName = "Employer 2"
                        }
                    }
                ],
                NonPayeEmploymentIncome = new NonPayeEmploymentIncome { Tips = 500m },
                OtherEmploymentIncome = new OtherEmploymentIncome
                {
                    LumpSums =
                    [
                        new LumpSum
                        {
                            TaxableLumpSumsAndCertainIncome = new TaxableLumpSumsAndCertainIncome { Amount = 300m },
                            BenefitFromEmployerFinancedRetirementScheme = new BenefitFromEmployerFinancedRetirementScheme { Amount = 200m }
                        }
                    ]
                }
            };

            // Act
            var result = _calculator.Calculate(incomes);

            // Assert
            Assert.Equal(4000m, result);
        }
    }
}
