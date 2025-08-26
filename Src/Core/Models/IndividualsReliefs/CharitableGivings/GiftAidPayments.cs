namespace Core.Models.IndividualsReliefs.CharitableGivings;

/// <summary>
/// Represents a request model for capturing different forms of Gift Aid payments
/// and charitable giving to both UK and Non-UK charities.
/// </summary>
public class GiftAidPayments
{
    /// <summary>
    /// Represents a collection of the names of non-UK charities for which gift aid payments are being submitted.
    /// This property can hold a list of charity names, or it may be null if no such charities are applicable.
    /// </summary>
    public List<string>? NonUkCharitiesCharityNames { get; set; }

    /// <summary>
    /// Represents the total amount of gift aid payments made in the current tax year.
    /// (TR4: box 5)
    /// </summary>
    public decimal? CurrentYear { get; set; }

    /// <summary>
    /// Represents the total amount of one-off gift aid payments made in the current tax year.
    /// This property is optional and may be null.
    /// (TR4: box 6)
    /// </summary>
    public decimal? OneOffCurrentYear { get; set; }

    /// <summary>
    /// Gets or sets the amount of financial contributions made in the current tax year
    /// that are treated as if they were made in the previous tax year.
    /// This may be applicable in scenarios where contributions are carried back
    /// to utilize tax reliefs or other financial allowances from the prior year.
    /// (TR4: box 7)
    /// </summary>
    public decimal? CurrentYearTreatedAsPreviousYear { get; set; }

    /// <summary>
    /// Represents the amount of gift aid payments for the next tax year that are being treated as pertaining
    /// to the current tax year.
    /// (TR4: box 8)
    /// </summary>
    public decimal? NextYearTreatedAsCurrentYear { get; set; }

    /// <summary>
    /// Represents the total amount of charitable donations made to non-UK charities.
    /// This property is optional and can include donations that do not fall under UK-based charity organizations.
    /// When provided, the value contributes to the calculation of the grossed-up amount for Gift Aid.
    /// </summary>
    public decimal? NonUkCharities { get; set; }

    /// <summary>
    /// Represents the total grossed-up amount calculated by summing the values of
    /// current year donations, one-off donations for the current year, donations
    /// for the current year treated as previous year, donations for the next year treated
    /// as current year, and donations to non-UK charities.
    /// This property aggregates the relevant amounts, defaulting to zero if any value is null.
    /// </summary>
    public decimal GrossedUpAmount => (CurrentYear ?? 0) + (OneOffCurrentYear ?? 0) + (CurrentYearTreatedAsPreviousYear ?? 0) + (NextYearTreatedAsCurrentYear ?? 0) + (NonUkCharities ?? 0);
}
