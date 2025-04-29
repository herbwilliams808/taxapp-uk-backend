Feature: Tax Calculation API

  As a user of the Tax Calculation API
  I want to calculate tax due based on various financial inputs
  So that I can determine my tax obligations accurately

  Scenario Outline: Calculate basic tax liability
    Given a user provides an income of <income> and has paid <taxPaid> in tax
    When the tax calculation is performed
    Then the tax due should be <expectedTaxDue>
    Examples:
      | income | taxPaid | expectedTaxDue |
      | 50000  | 10000   | 5000           |
      | 75000  | 20000   | 12500          |

  Scenario Outline: Include pension contributions in tax calculation
    Given a user provides an income of <income>, has paid <taxPaid> in tax, and has made <pensionContributions> in pension contributions
    When the tax calculation is performed
    Then the tax due should be <expectedTaxDue>
    Examples:
      | income | taxPaid | pensionContributions | expectedTaxDue |
      | 50000  | 10000   | 5000                 | 4500           |
      | 75000  | 20000   | 10000                | 11500          |

  Scenario Outline: Include gift aid contributions in tax calculation
    Given a user provides an income of <income>, has paid <taxPaid> in tax, and has made <giftAid> in gift aid contributions
    When the tax calculation is performed
    Then the tax due should be <expectedTaxDue>
    Examples:
      | income | taxPaid | giftAid | expectedTaxDue |
      | 50000  | 10000   | 2000    | 4800           |
      | 75000  | 20000   | 3000    | 12250          |

  Scenario Outline: Include child benefits in tax calculation
    Given a user provides an income of <income>, has paid <taxPaid> in tax, and receives <childBenefits> in child benefits
    When the tax calculation is performed
    Then the tax due should be <expectedTaxDue>
    Examples:
      | income | taxPaid | childBenefits | expectedTaxDue |
      | 50000  | 10000   | 2000          | 5200           |
      | 75000  | 20000   | 3000          | 12750          |

  Scenario Outline: Include property income and expenses in tax calculation
    Given a user provides rental income of <rentalIncome>, allowable expenses of <expenses>, and interest costs of <interestCosts>
    When the property tax calculation is performed
    Then the taxable property income should be <taxablePropertyIncome>
    Examples:
      | rentalIncome | expenses | interestCosts | taxablePropertyIncome |
      | 15000        | 5000     | 2000          | 8000                  |
      | 20000        | 7000     | 3000          | 10000                 |
