using Core.Models.IndividualsEmploymentIncomes.Employments;
using Core.Models.IndividualsEmploymentIncomes.NonPayeEmploymentIncome;
using Core.Models.IndividualsEmploymentIncomes.OtherEmploymentIncome;
using Core.Models.PropertyBusiness;

namespace Core.Models.Incomes;

public class IncomeSourcesDetails
{
    public List<EmploymentAndFinancialDetails>? EmploymentsAndFinancialDetails { get; set; } = [];
    public NonPayeEmploymentIncome? NonPayeEmploymentIncome { get; set; } = new();
    public OtherEmploymentIncome? OtherEmploymentIncome { get; set; } = new();
    public UkPropertyBusinessIncome? UkPropertyBusinessIncome { get; set; } = new();

}