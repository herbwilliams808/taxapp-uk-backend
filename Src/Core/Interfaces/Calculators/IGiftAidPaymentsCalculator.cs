using Core.Models.IndividualsReliefs;

namespace Core.Interfaces.Calculators;

public interface IGiftAidPaymentsCalculator
{
    decimal Calculate(IndividualsReliefsDetails? individualsReliefs);
}