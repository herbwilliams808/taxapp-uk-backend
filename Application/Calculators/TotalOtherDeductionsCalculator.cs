using Shared.Models.OtherDeductions;

namespace Application.Calculators;

public class TotalOtherDeductionsCalculator
{
   public decimal Calculate(OtherDeductionsDetails otherDeductionsDetails)
   {
      var seafarersdeductions = otherDeductionsDetails?.Seafarers?.Select(seafarer => seafarer.AmountDeducted);
      return seafarersdeductions?.Sum() ?? 0;
   } 
}