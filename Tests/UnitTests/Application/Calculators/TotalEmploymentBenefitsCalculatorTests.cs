using Shared.Models.Incomes;
using Shared.Models.Incomes.NonSavingsIncomes.Employments;

namespace UnitTests.Application.Calculators
{
    public class TotalEmploymentBenefitsCalculatorTests
    {
        private readonly TotalEmploymentBenefitsCalculator _calculator;

        public TotalEmploymentBenefitsCalculatorTests()
        {
            _calculator = new TotalEmploymentBenefitsCalculator();
        }

        [Fact]
        public void Calculate_WhenIncomesIsNull_ReturnsZero()
        {
            // Act
            var result = _calculator.Calculate(incomes: null!);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void Calculate_WhenEmploymentsIsNull_ReturnsZero()
        {
            // Arrange
            var incomes = new Incomes();

            // Act
            var result = _calculator.Calculate(incomes);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void Calculate_WhenEmploymentsIsEmpty_ReturnsZero()
        {
            // Arrange
            var incomes = new Incomes 
            { 
                Employments = new List<Employment>() 
            };

            // Act
            var result = _calculator.Calculate(incomes);

            // Assert
            Assert.Equal(0m, result);
        }

        [Fact]
        public void Calculate_WithSingleEmploymentAndAllBenefits_ReturnsTotalSum()
        {
            // Arrange
            var incomes = new Incomes
            {
                Employments = new List<Employment>
                {
                    new()
                    {
                        Pay = new Pay(),
                        BenefitsInKind = new BenefitsInKind
                        {
                            MedicalInsurance = 100m,
                            CarBenefits = 200m,
                            FuelBenefits = 300m,
                            EducationalServices = 400m,
                            NonCashVouchers = 500m,
                            Accommodation = 600m,
                            Assets = 700m,
                            AssetTransfers = 800m,
                            CreditTokens = 900m,
                            DebitCards = 1000m,
                            EmployerProvidedServices = 1100m,
                            OtherItems = 1200m,
                            PaymentsOnEmployeesBehalf = 1300m,
                            PersonalIncurrredExpenses = 1400m,
                            QualifyingRelocationExpenses = 1500m,
                            EmployerProvidedProfessionalSubscriptions = 1600m,
                            MileageAllowance = 1700m,
                            VehiclesAndVehicleFuel = 1800m
                        }
                    }
                }
            };

            // Act
            var result = _calculator.Calculate(incomes);

            // Assert
            Assert.Equal(17100m, result);
        }

        [Fact]
        public void Calculate_WithMultipleEmployments_ReturnsTotalSum()
        {
            // Arrange
            var incomes = new Incomes
            {
                Employments = new List<Employment>
                {
                    new()
                    {
                        Pay = new Pay(),
                        BenefitsInKind = new BenefitsInKind
                        {
                            MedicalInsurance = 100m,
                            CarBenefits = 200m
                        }
                    },
                    new()
                    {
                        Pay = new Pay(),
                        BenefitsInKind = new BenefitsInKind
                        {
                            FuelBenefits = 300m,
                            EducationalServices = 400m
                        }
                    }
                }
            };

            // Act
            var result = _calculator.Calculate(incomes);

            // Assert
            Assert.Equal(1000m, result);
        }

        [Fact]
        public void Calculate_WithSomeNullBenefits_ReturnsSumOfNonNullValues()
        {
            // Arrange
            var incomes = new Incomes
            {
                Employments = new List<Employment>
                {
                    new()
                    {
                        Pay = new Pay(),
                        BenefitsInKind = new BenefitsInKind
                        {
                            MedicalInsurance = 100m,
                            CarBenefits = null,
                            FuelBenefits = 300m
                        }
                    }
                }
            };

            // Act
            var result = _calculator.Calculate(incomes);

            // Assert
            Assert.Equal(400m, result);
        }

        [Fact]
        public void Calculate_WithNullBenefitsInKind_ReturnsZero()
        {
            // Arrange
            var incomes = new Incomes
            {
                Employments = new List<Employment>
                {
                    new()
                    {
                        Pay = new Pay(),
                        BenefitsInKind = null
                    }
                }
            };

            // Act
            var result = _calculator.Calculate(incomes);

            // Assert
            Assert.Equal(0m, result);
        }
    }
}