
Feature: Tax Calculation Service J

As a taxpayer,

I want to calculate my tax due and reliefs based on my financial details,

So that I can understand my tax obligations and potential savings.

The Tax Calculation API computes tax liabilities and reliefs for users based on their financial data. It returns accurate results in a consistent format.

Scenario: API Error Handling
*Given* an unexpected error occurs during tax calculation

*When* the user submits data to the `/api/tax/calculate` endpoint

*Then* the API should:

Return a `500 Internal Server Error` response
Include a generic error message












Example:










*Response*:

```json
{
"error": "An unexpected error occurred. Please try again later."
}
```



Scenario: Tax Relief with Optional Fields
*Given* a user provides financial data with optional fields (e.g., child benefits, rental income details)

*When* the user submits the data to the `/api/tax/calculate` endpoint

*Then* the API should:

Include optional fields in the calculation logic
Return a comprehensive response with accurate results












Example:










*Request Payload*:

```json
Unknown macro: { "income"}
```
*Response*:

```json
Unknown macro: { "taxDue"}
```



Scenario: Tax Due Results in Zero
*Given* a user provides financial data where the total tax relief exceeds the tax due

*When* the user submits the data to the `/api/tax/calculate` endpoint

*Then* the API should:

Calculate the tax due as zero
Return the relief amounts and total tax after reliefs as zero












Example:










*Request Payload*:

```json
{
"income": 30000,
"taxPaid": 10000,
"pensionContributions": 20000,
"giftAidContributions": 15000
}
```
*Response*:

```json
Unknown macro: { "taxDue"}
```



Scenario: Invalid or Missing Input Data
*Given* a user provides incomplete or invalid financial data

*When* the user submits the data to the `/api/tax/calculate` endpoint

*Then* the API should:

Return a `400 Bad Request` response
Include an error message indicating the validation failure












Example:










*Request Payload*:

```json
{
"income": -10000,
"taxPaid": "notANumber"
}
```
*Response*:

```json
{
"error": "Invalid input: Income must be positive and Tax Paid must be a number."
}
```



Scenario: Successful Tax Calculation
*Given* a user provides valid income, tax paid, pension contributions, and gift aid contributions

*When* the user submits the financial data to the `/api/tax/calculate` endpoint

*Then* the API should:


Calculate the total tax due
Calculate tax reliefs for pension and gift aid contributions
Return the tax due, relief details, and total tax after applying reliefs





Example:


*Request Payload*:

```json
{
"income": 75000,
"taxPaid": 20000,
"pensionContributions": 5000,
"giftAidContributions": 2000
}
```
*Response*:

```json
Unknown macro: { "taxDue"}
```



