using Shared.Models.OtherDeductions;

namespace Application.Calculators;

public class TotalOtherDeductionsCalculator
{
   public decimal Calculate(OtherDeductions otherDeductions)
   {
      var seafarersdeductions = otherDeductions?.Seafarers?.Select(seafarer => seafarer.AmountDeducted);
      return seafarersdeductions?.Sum() ?? 0;
   } 
}