namespace Core.Models.IndividualsReliefs.Pensions;

/// <summary>
/// Represents a request model for capturing different forms of pension-related reliefs.
/// </summary>
public class PensionReliefs
{
    /// <summary>
    /// Gets or sets the value representing regular pension contributions
    /// made by an individual. This is typically a recurring payment
    /// made towards a pension scheme.
    /// (TR4: box 1)
    /// </summary>
    public decimal? RegularPensionContributions { get; set; }

    /// <summary>
    /// Gets or sets the amount representing one-off pension contributions paid.
    /// This typically includes non-recurring, lump-sum contributions made toward a pension scheme
    /// during the relevant tax period.
    /// (TR4: box 1.1)
    /// </summary>
    public decimal? OneOffPensionContributionsPaid { get; set; }

    /// <summary>
    /// Represents the amount of payments made towards retirement annuity contracts.
    /// This property is used to specify contributions that are eligible for
    /// relief under retirement annuity payment schemes.
    /// (TR4: box 2)
    /// </summary>
    public decimal? RetirementAnnuityPayments { get; set; }

    /// <summary>
    /// Represents the amount contributed to an employer's pension scheme where tax relief
    /// has not been applied.
    /// (TR4: box 3)
    /// </summary>
    public decimal? PaymentToEmployersSchemeNoTaxRelief { get; set; }

    /// <summary>
    /// Represents the amount of contributions made to an overseas pension scheme.
    /// This property allows specifying contributions that qualify for relief due to payments
    /// into pension schemes that are recognized as overseas-based.
    /// (TR4: box 4)
    /// </summary>
    public decimal? OverseasPensionSchemeContributions { get; set; }
}