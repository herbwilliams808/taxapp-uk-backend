Feature: As a higher rate taxpayer,

I want to submit my income and Giftaid contributions,

To get an estimate of how much tax relief I’m entitled to

Scenario: Higher Income Giftaid Tax Relief Calculation
    Given the income is 50
    And the charitable donations is 70
    When the relief is calculated
    Then the result should be 40

Scenario: Lower Income Giftaid Tax Relief Calculation
    Given the income is 40
    And the charitable donations is 70
    When the relief is calculated
    Then the result should be 0