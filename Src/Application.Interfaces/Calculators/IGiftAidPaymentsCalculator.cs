using Shared.Models.IndividualsReliefs;

namespace Application.Interfaces.Calculators;

public interface IGiftAidPaymentsCalculator
{
    /// <summary>
    /// Calculates the total Gift Aid payments based on the provided individual's reliefs details.
    /// </summary>
    /// <param name="individualsReliefs">An object containing details about an individual's reliefs, including Gift Aid payment amounts for the current, previous, and next year.</param>
    /// <returns>
    /// The calculated total Gift Aid payments as a decimal value.
    /// This includes payments for the current year, adjusted by subtracting the amount treated as the previous year and adding the amount treated as the next year.
    /// </returns>
    decimal Calculate(IndividualsReliefsDetails? individualsReliefs);
}