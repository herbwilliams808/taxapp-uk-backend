using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System.Globalization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxEstimationController(TaxEstimationService taxEstimationService) : ControllerBase
    {
        [HttpPost]
        public ActionResult<TaxEstimationResponse> CalculateTax(TaxEstimationRequest request)
        {
            // Determine the tax year ending
            int currentYear = DateTime.UtcNow.Year;
            int currentMonth = DateTime.UtcNow.Month;
            int defaultTaxYearEnding = (currentMonth < 4 || (currentMonth == 4 && DateTime.UtcNow.Day < 6)) ? currentYear : currentYear + 1;
            int taxYearEnding = string.IsNullOrEmpty(request.TaxYearEnding) 
                ? defaultTaxYearEnding 
                : int.TryParse(request.TaxYearEnding, out var parsedYear) ? parsedYear : defaultTaxYearEnding;

            // Default region to "england" if not provided
            string region = string.IsNullOrEmpty(request.Region) ? "england" : request.Region.ToLower(CultureInfo.InvariantCulture);

            // Validate region
            var validRegions = new[] { "england", "scotland", "wales", "northern ireland" };
            if (!validRegions.Contains(region))
            {
                return BadRequest("Invalid region provided. Valid values are: england, scotland, wales, northern ireland.");
            }

            // Calculate tax using the service
            var taxResult = taxEstimationService.CalculateTax(request.Incomes, taxYearEnding, region);

            return Ok(taxResult);
        }
    }
}