using Shared.Models.IndividualsEmploymentIncomes.Employments;
using Shared.Models.IndividualsEmploymentIncomes.NonPayeEmploymentIncome;
using Shared.Models.IndividualsEmploymentIncomes.OtherEmploymentIncome;
using Shared.Models.PropertyBusiness;

namespace Shared.Models.Incomes;

public class IncomeSources
{
    public List<EmploymentAndFinancialDetails>? EmploymentsAndFinancialDetails { get; set; } = [];
    public NonPayeEmploymentIncome? NonPayeEmploymentIncome { get; set; } = new();
    public OtherEmploymentIncome? OtherEmploymentIncome { get; set; } = new();
    public UkPropertyBusinessIncome? UkPropertyBusinessIncome { get; set; } = new();

}