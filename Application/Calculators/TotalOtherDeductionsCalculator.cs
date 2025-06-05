using Application.Interfaces.Calculators;
using Shared.Models.OtherDeductions;

namespace Application.Calculators;

public class TotalOtherDeductionsCalculator : ITotalOtherDeductionsCalculator
{
   public decimal Calculate(OtherDeductionsDetails? otherDeductionsDetails)
   {
      var seafarersDeductions = otherDeductionsDetails?.Seafarers?.Select(seafarer => seafarer.AmountDeducted);
      return seafarersDeductions?.Sum() ?? 0;
   } 
}