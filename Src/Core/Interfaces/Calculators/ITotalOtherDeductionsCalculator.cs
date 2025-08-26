using Core.Models.OtherDeductions;

namespace Core.Interfaces.Calculators;

public interface ITotalOtherDeductionsCalculator
{
    decimal Calculate(OtherDeductionsDetails? otherDeductionsDetails);
}