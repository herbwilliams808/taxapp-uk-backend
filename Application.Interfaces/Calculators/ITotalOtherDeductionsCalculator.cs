using Shared.Models.OtherDeductions;

namespace Application.Interfaces.Calculators;

public interface ITotalOtherDeductionsCalculator
{
    decimal Calculate(OtherDeductionsDetails? otherDeductionsDetails);
}