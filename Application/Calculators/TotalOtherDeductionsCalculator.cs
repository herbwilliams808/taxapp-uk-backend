using Shared.Models.OtherDeductions;

namespace Application.Calculators;

public class TotalOtherDeductionsCalculator
{
   // TODO: Added virtual. Implement interface
   public virtual decimal Calculate(OtherDeductionsDetails otherDeductionsDetails)
   {
      var seafarersdeductions = otherDeductionsDetails?.Seafarers?.Select(seafarer => seafarer.AmountDeducted);
      return seafarersdeductions?.Sum() ?? 0;
   } 
}