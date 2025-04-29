Feature: Estimate My Tax
    
Scenario: Estimate my tax based on my salary
    Given I have a salary of 50000
    When I calculate my tax
    Then the estimated tax should be 12500