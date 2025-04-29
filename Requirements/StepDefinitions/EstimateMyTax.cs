using Reqnroll;

namespace Requirements.StepDefinitions;

[Binding]
public class EstimateMyTax
{
    [Given("I have a salary of {int}")]
    public void GivenIHaveASalaryOf(int p0)
    {
        ScenarioContext.StepIsPending();
    }

    [When("I calculate my tax")]
    public void WhenICalculateMyTax()
    {
        ScenarioContext.StepIsPending();
    }

    [Then("the estimated tax should be {int}")]
    public void ThenTheEstimatedTaxShouldBe(int p0)
    {
        ScenarioContext.StepIsPending();
    }
}