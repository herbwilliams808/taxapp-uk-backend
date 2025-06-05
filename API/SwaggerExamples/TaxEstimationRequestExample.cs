// API/SwaggerExamples/TaxEstimationRequestExample.cs
using Swashbuckle.AspNetCore.Filters;
using Shared.Models.HttpMessages; // Namespace for TaxEstimationRequest
using Shared.Models.Incomes; // Namespace for IncomeSources
using Shared.Models.IndividualsEmploymentIncomes.Employments; // Namespace for EmploymentAndFinancialDetails, Employer, Pay
using Shared.Models.IndividualsEmploymentIncomes.NonPayeEmploymentIncome; // Namespace for NonPayeEmploymentIncome
using Shared.Models.PropertyBusiness; // Namespace for UkPropertyBusinessIncome
using Shared.Models.IndividualsReliefs; // Namespace for IndividualsReliefsDetails
using Shared.Models.IndividualsReliefs.Pensions; // Namespace for PensionReliefs

namespace API.SwaggerExamples
{
    public class TaxEstimationRequestExample : IExamplesProvider<TaxEstimationRequest>
    {
        public TaxEstimationRequest GetExamples()
        {
            return new TaxEstimationRequest
            {
                TaxYearEnding = 2025,
                Region = "england",
                Incomes = new IncomeSources
                {
                    EmploymentsAndFinancialDetails = new List<EmploymentAndFinancialDetails>
                    {
                        new EmploymentAndFinancialDetails
                        {
                            // Populate only the relevant fields for the example
                            // Based on your example request, only Employer and Pay are populated within this item.
                            Employer = new Employer
                            {
                                EmployerRef = "999/A",
                                EmployerName = "ACME"
                            },
                            Pay = new Pay
                            {
                                TaxablePayToDate = 59000,
                                TotalTaxToDate = 5000
                            }
                        }
                    },
                    NonPayeEmploymentIncome = new NonPayeEmploymentIncome
                    {
                        Tips = 500
                    },
                    UkPropertyBusinessIncome = new UkPropertyBusinessIncome // Corrected class name based on your IncomeSources
                    {
                        Income = 20000,
                        AllowablePropertyLettingExpenses = 5000,
                        PropertyLettingLoanInterestAndFinanceCosts = 10000
                    },
                    // Initialize other IncomeSources properties if they have default values or are required
                    OtherEmploymentIncome = null // Initialize if not nullable and has defaults
                },
                IndividualsReliefs = new IndividualsReliefsDetails
                {
                    PensionReliefs = new PensionReliefs
                    {
                        RegularPensionContributions = 3000
                    }
                },
                // Other optional fields from TaxEstimationRequest can be set to null or default if not part of the example
                ForeignReliefs = null,
                OtherDeductions = null,
                IndividualsForeignIncome = null
            };
        }
    }
}