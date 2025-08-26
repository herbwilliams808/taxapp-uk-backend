using Core.Models.Incomes;

namespace Core.Interfaces.Calculators;

public interface ITotalEmploymentExpensesCalculator
{
    decimal Calculate(IncomeSourcesDetails? incomeSources);
}