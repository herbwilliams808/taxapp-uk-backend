# Section 1: Add together non-savings income and lump sum payments
## Employment
___
### Pay
'Employment' pages: (boxes 1 + 3)
```
sum(
    Box     API
    EMP1    Individuals Employments Income	Create and Amend Employment Financial Details	/employment/pay/taxablePayToDate
    EMP3    Individuals Employments Income	Create and Amend Non-PAYE Employment Income	/tips
) = A1
```

'Ministers of religion' pages: (box 38)
```
value(
    Box     API
    ?       ?
) = A2
```

'Additional information' pages, page Ai 2 (Share schemes and employment lump sums : (boxes 3 + 4)
```
sum(
    Box     API
    ASE3	Create and Amend Other Employment Income	lumpSums > taxableLumpSumsAndCertainIncome > amount
    ASE4	Create and Amend Other Employment Income	lumpSums > benefitFromEmployerFinancedRetirementScheme > amount
) = A3
```
#### Box A4: Total pay
```
A4 = A1 + A2 + A3

A4(TotalPay) =
A1(
    TaxablePayToDate +
    Tips
) +
A2(Not available) +
A3(
    taxableLumpSumsAndCertainIncome > amount +
    benefitFromEmployerFinancedRetirementScheme > amount
)
```
### Employment Benefits
___
Benefits from your employment - 'Employment' pages: (boxes 9 to 16)

```
sum(
    Box     API
    EMP9	Create and Amend Employment Financial Details	benefitsInKind > car
    EMP9	Create and Amend Employment Financial Details	benefitsInKind > van
    EMP10	Create and Amend Employment Financial Details	benefitsInKind > carFuel
    EMP10	Create and Amend Employment Financial Details	benefitsInKind > vanFuel
    EMP11	Create and Amend Employment Financial Details	benefitsInKind > medicalInsurance
    EMP12	Create and Amend Employment Financial Details	benefitsInKind > vouchersAndCreditCards
    EMP12	Create and Amend Employment Financial Details	benefitsInKind > mileage
    EMP12	Create and Amend Employment Financial Details	benefitsInKind > nonCash
    EMP12	Create and Amend Employment Financial Details	benefitsInKind > nurseryPlaces
    EMP13	Create and Amend Employment Financial Details	benefitsInKind > assets
    EMP13	Create and Amend Employment Financial Details	benefitsInKind > assetsTransfer
    EMP14	Create and Amend Employment Financial Details	benefitsInKind > accommodation
    EMP15	Create and Amend Employment Financial Details	benefitsInKind > beneficialLoan
    EMP15	Create and Amend Employment Financial Details	benefitsInKind > educationalServices
    EMP15	Create and Amend Employment Financial Details	benefitsInKind > service
    EMP15	Create and Amend Employment Financial Details	benefitsInKind > qualifyingRelocationExpenses
    EMP15	Create and Amend Employment Financial Details	benefitsInKind > employerProvidedProfessionalSubscriptions
    EMP15	Create and Amend Employment Financial Details	benefitsInKind > employerProvidedServices
    EMP15	Create and Amend Employment Financial Details	benefitsInKind > paymentsOnEmployeesBehalf
    EMP15	Create and Amend Employment Financial Details	benefitsInKind > otherItems
    EMP15	Create and Amend Employment Financial Details	benefitsInKind > incomeTaxPaidByDirector
    EMP16	Create and Amend Employment Financial Details	benefitsInKind > expenses
    EMP16	Create and Amend Employment Financial Details	benefitsInKind > taxableExpenses
    EMP16	Create and Amend Employment Financial Details	benefitsInKind > nonQualifyingRelocationExpenses
    EMP16	Create and Amend Employment Financial Details	benefitsInKind > taxableExpenses
    EMP16	Create and Amend Employment Financial Details	benefitsInKind > travelAndSubsistence
    EMP16	Create and Amend Employment Financial Details	benefitsInKind > entertaining
    EMP16	Create and Amend Employment Financial Details	benefitsInKind > telephone
    EMP16	Create and Amend Employment Financial Details	benefitsInKind > personalIncidentalExpenses
) = A5
```
#### Box A5: Total Employment Benefits
```
TotalEmploymentBenefits =
Sum(employments.each(benefits.each))

Application.Calculators.TotalEmploymentBenefitsCalculator
```
### Employment Expenses
___
Expenses from your employment - 'Employment' pages: (boxes 17 to 20)
```
sum(
    Box     API
    EMP17	Create and Amend Employment Expenses	expenses > businessTravelCosts
    EMP17	Create and Amend Employment Expenses	expenses > hotelAndMealExpenses
    EMP17	Create and Amend Employment Expenses	expenses > jobExpenses
    EMP17	Create and Amend Employment Expenses	expenses > vehicleExpenses
    EMP17	Create and Amend Employment Expenses	expenses > mileageAllowanceRelief
    EMP18	Create and Amend Employment Expenses	expenses > flatRateJobExpenses
    EMP19	Create and Amend Employment Expenses	expenses > professionalSubscriptions
    EMP20	Create and Amend Employment Expenses	expenses > otherAndCapitalAllowances
    EMP20	Create and Amend Employment Expenses	expenses > jobExpenses
    EMP20	Create and Amend Employment Expenses	expenses > vehicleExpenses
    EMP20	Create and Amend Employment Expenses	expenses > mileageAllowanceRelief
) = A6
```
#### Box A6: Total Employment Expenses
```
TotalEmploymentExpenses =
Sum(employments.each(expenses.each))

Application.Calculators.TotalEmploymentExpensesCalculator
```

'Additional information' pages, page Ai 2 (Share schemes and employment lump sums: (boxes 11 to 13)
```
sum(
    Box     API
    ASE11	Other Deductions	Create and Amend Deductions	seafarers > amountDeducted
    ASE12	Individuals Foreign Income	Create and Amend Foreign Income	foreignEarnings > earningsNotTaxableUK
    ASE13	Individuals Reliefs	Create and Amend Foreign Reliefs	foreignTaxForFtcrNotClaimed > amount
) = A7
```
```A6 + A7 = A8```

### Pension contribution - payment from HMRC
___
Employment pages: (box 3.1)
```
value(
    Box     API
    ?       ?
) = A8a
```
### Total from all employments
```A9 = (
    A4(TotalPay) + 
    A5(TotalEmploymentBenefits) + 
    A8a(PensionContributionFromHmrc)) - 
    A8(ExpensesShareSchemesEmploymentLumpSums)```
```
TaxablePayToDate +
NonPayeEmploymentIncome.Tips +


```