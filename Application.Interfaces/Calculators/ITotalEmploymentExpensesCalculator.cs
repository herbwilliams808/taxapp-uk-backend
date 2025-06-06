using Shared.Models.Incomes;

namespace Application.Interfaces.Calculators;

public interface ITotalEmploymentExpensesCalculator
{
    decimal Calculate(IncomeSourcesDetails? incomeSources);
}