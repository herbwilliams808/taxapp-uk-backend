using Core.Interfaces.Calculators;
using Core.Models.IndividualsReliefs;

namespace Application.Calculators;

public class GiftAidPaymentsCalculator : IGiftAidPaymentsCalculator
{
    /// <summary>
    ///     Calculates the total Gift Aid payments based on the provided individual's reliefs details.
    /// </summary>
    /// <param name="individualsReliefs">
    ///     An object containing details about an individual's reliefs, including Gift Aid payment
    ///     amounts for the current, previous, and next year.
    /// </param>
    /// <returns>
    ///     The calculated total Gift Aid payments as a decimal value.
    ///     This includes payments for the current year, adjusted by subtracting the amount treated as the previous year and
    ///     adding the amount treated as the next year.
    /// </returns>
    public decimal Calculate(IndividualsReliefsDetails? individualsReliefs)
    {
        var giftAidCurrentYear = Math.Round(individualsReliefs?.GiftAidPayments?.CurrentYear ?? 0m, 0,
            MidpointRounding.ToPositiveInfinity);
        var giftAidCurrentYearTreatedAsPrevious =
            Math.Round(individualsReliefs?.GiftAidPayments?.CurrentYearTreatedAsPreviousYear ?? 0m, 0,
                MidpointRounding.ToPositiveInfinity);
        var giftAidNextYearTreatedAsCurrent =
            Math.Round(individualsReliefs?.GiftAidPayments?.NextYearTreatedAsCurrentYear ?? 0m, 0,
                MidpointRounding.ToPositiveInfinity);
        var giftAidPayments =
            giftAidCurrentYear - giftAidCurrentYearTreatedAsPrevious + giftAidNextYearTreatedAsCurrent;
        return giftAidPayments;
    }
}