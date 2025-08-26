// API/SwaggerExamples/TaxEstimationRequestExample.cs

using Core.Models.HttpMessages;
using Core.Models.Incomes;
using Core.Models.IndividualsEmploymentIncomes.Employments;
using Core.Models.IndividualsEmploymentIncomes.NonPayeEmploymentIncome;
using Core.Models.IndividualsReliefs;
using Core.Models.IndividualsReliefs.Pensions;
using Core.Models.PropertyBusiness;
using Swashbuckle.AspNetCore.Filters;
// Namespace for TaxEstimationRequest
// Namespace for IncomeSourcesDetails
// Namespace for EmploymentAndFinancialDetails, Employer, Pay
// Namespace for NonPayeEmploymentIncome
// Namespace for UkPropertyBusinessIncome
// Namespace for IndividualsReliefsDetails

// Namespace for PensionReliefs

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
                IncomeSources = new IncomeSourcesDetails
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
                    UkPropertyBusinessIncome = new UkPropertyBusinessIncome // Corrected class name based on your IncomeSourcesDetails
                    {
                        Income = 20000,
                        AllowablePropertyLettingExpenses = 5000,
                        PropertyLettingLoanInterestAndFinanceCosts = 10000
                    },
                    // Initialize other IncomeSourcesDetails properties if they have default values or are required
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