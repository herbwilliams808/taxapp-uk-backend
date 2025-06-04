// Sample valid JSON content for testing

namespace UnitTests.TestHelpers;

public class ValidTaxRates
{
    public const string ValidTaxRatesJson = @"
        {
          ""taxRates"": {
            ""year_2024_25"": {
              ""personalAllowance"": 12570,
              ""basicRateThreshold"": 37700,
              ""higherRateThreshold"": 125140,
              ""additionalRateThreshold"": 125140,
              ""basicRatePercentage"": 20,
              ""higherRatePercentage"": 40,
              ""additionalRatePercentage"": 45,
              ""dividendAllowance"": 1000,
              ""dividendBasicRatePercentage"": 8.75,
              ""dividendHigherRatePercentage"": 33.75,
              ""dividendAdditionalRatePercentage"": 39.35,
              ""england"": {
                ""basicRatePercentage"": 20,
                ""higherRatePercentage"": 40,
                ""additionalRatePercentage"": 45
              },
              ""scotland"": {
                ""starterRateThreshold"": 2160,
                ""starterRatePercentage"": 19,
                ""basicRateThreshold"": 13118,
                ""basicRatePercentage"": 20,
                ""intermediateRateThreshold"": 31430,
                ""intermediateRatePercentage"": 21,
                ""higherRateThreshold"": 125140,
                ""higherRatePercentage"": 41,
                ""topRateThreshold"": 125140,
                ""topRatePercentage"": 46
              },
              ""wales"": {
                ""basicRatePercentage"": 20,
                ""higherRatePercentage"": 40,
                ""additionalRatePercentage"": 45
              },
              ""northernireland"": {
                ""basicRatePercentage"": 20,
                ""higherRatePercentage"": 40,
                ""additionalRatePercentage"": 45
              }
            },
            ""year_2025_26"": {
              ""personalAllowance"": 12570,
              ""basicRateThreshold"": 37700,
              ""higherRateThreshold"": 125140,
              ""additionalRateThreshold"": 125140,
              ""basicRatePercentage"": 20,
              ""higherRatePercentage"": 40,
              ""additionalRatePercentage"": 45,
              ""dividendAllowance"": 500,
              ""dividendBasicRatePercentage"": 8.75,
              ""dividendHigherRatePercentage"": 33.75,
              ""dividendAdditionalRatePercentage"": 39.35,
              ""england"": {
                ""basicRatePercentage"": 20,
                ""higherRatePercentage"": 40,
                ""additionalRatePercentage"": 45
              },
              ""scotland"": {
                ""starterRateThreshold"": 2160,
                ""starterRatePercentage"": 19,
                ""basicRateThreshold"": 13118,
                ""basicRatePercentage"": 20,
                ""intermediateRateThreshold"": 31430,
                ""intermediateRatePercentage"": 21,
                ""higherRateThreshold"": 125140,
                ""higherRatePercentage"": 41,
                ""topRateThreshold"": 125140,
                ""topRatePercentage"": 46
              },
              ""wales"": {
                ""basicRatePercentage"": 20,
                ""higherRatePercentage"": 40,
                ""additionalRatePercentage"": 45
              },
              ""northernireland"": {
                ""basicRatePercentage"": 20,
                ""higherRatePercentage"": 40,
                ""additionalRatePercentage"": 45
              }
            }
          }
        }";
}