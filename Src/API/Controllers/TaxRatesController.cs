// File: API/Controllers/TaxRatesController.cs
using Microsoft.AspNetCore.Mvc;
using Application.Services.HmrcIntegration.Auth; // Keep this if other concrete services are used here, otherwise remove
using Application.Interfaces.Services.HmrcIntegration.TestUser; // NEW: Add this using directive

namespace API.Controllers.HmrcIntegration.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxRatesController(ITaxRatesCacheService taxRatesCacheService) : ControllerBase
    {
        // CHANGED: Use the interface type for the private field
        private readonly ITaxRatesCacheService _taxRatesCacheService = taxRatesCacheService ?? throw new ArgumentNullException(nameof(taxRatesCacheService));

        [HttpGet("GetTaxRates")]
        public IActionResult GetTaxRates()
        {
            try
            {
                var allRates = _taxRatesCacheService.GetAllCachedRates(); // Uses the interface method
                if (allRates == null || allRates.Count == 0)
                    return NoContent();

                return Ok(allRates);
            }
            catch (Exception ex)
            {
                // You should consider injecting an ILogger here if you want to log exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetTaxRate")]
        public IActionResult GetTaxRate([FromQuery] int year, [FromQuery] string? region, [FromQuery] string property)
        {
            try
            {
                var taxRate = _taxRatesCacheService.GetTaxRateValue(year, region, property); // Uses the interface method

                if (taxRate is null)
                {
                    return NotFound($"Tax rate not found for year: {year}, region: {region}, property: {property}.");
                }

                return Ok(taxRate);
            }
            catch (Exception ex)
            {
                // Log exception here if you have a logger (and you should!)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}