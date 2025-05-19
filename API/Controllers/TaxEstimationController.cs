using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Models.HttpMessages;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxEstimationController(TaxEstimationService taxEstimationService) : ControllerBase
    {
        [HttpPost]
        public ActionResult<TaxEstimationResponse> CalculateTax(TaxEstimationRequest request)
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

            // Perform tax calculation
            var response = taxEstimationService.CalculateTax(
                request.Incomes,
                taxYearEnding,
                region,
                request.Reliefs

            );

            return Ok(response);
        }
    }
}