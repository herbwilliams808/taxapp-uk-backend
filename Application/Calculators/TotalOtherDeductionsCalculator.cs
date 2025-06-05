using Shared.Models.OtherDeductions;

namespace Application.Calculators;

public class TotalOtherDeductionsCalculator
{
   // TODO: Added virtual. Implement interface
   public virtual decimal Calculate(OtherDeductionsDetails? otherDeductionsDetails)
   {
      var seafarersDeductions = otherDeductionsDetails?.Seafarers?.Select(seafarer => seafarer.AmountDeducted);
      return seafarersDeductions?.Sum() ?? 0;
   } 
}