using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.HttpMessages;
using API.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxEstimationController(TaxEstimationService taxEstimationService) : ControllerBase
    {
        [HttpPost]
        [SwaggerRequestExample(typeof(TaxEstimationRequest), typeof(TaxEstimationRequestExample))]
        public async Task<ActionResult<TaxEstimationResponse>> CalculateTaxAsync(TaxEstimationRequest request)
        {
            // Validate region
            var region = string.IsNullOrWhiteSpace(request.Region) ? "england" : request.Region.ToLower();
            var validRegions = new[] { "england", "scotland", "wales", "northern ireland" };
            if (!validRegions.Contains(region))
            {
                return BadRequest($"Invalid region. Valid values are: {string.Join(", ", validRegions)}.");
            }

            // Determine tax year ending
            var currentDate = DateTime.UtcNow;
            var defaultTaxYearEnding = currentDate.Month > 4 || (currentDate.Month == 4 && currentDate.Day >= 6)
                ? currentDate.Year
                : currentDate.Year - 1;
            var taxYearEnding = request.TaxYearEnding ?? defaultTaxYearEnding;

            // Await the async tax calculation method
            var response = await taxEstimationService.CalculateTaxAsync(
                taxYearEnding,
                region,
                request.Incomes, 
                request.IndividualsReliefs,
                request.OtherDeductions,
                request.IndividualsForeignIncome,
                request.ForeignReliefs
            );

            return Ok(response);
        }
    }
}