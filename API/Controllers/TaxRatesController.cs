using Microsoft.AspNetCore.Mvc;
using Application.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxRatesController : ControllerBase
    {
        private readonly TaxRatesCacheService _taxRatesCacheService;

        // Constructor injection
        public TaxRatesController(TaxRatesCacheService taxRatesCacheService)
        {
            _taxRatesCacheService = taxRatesCacheService ?? throw new ArgumentNullException(nameof(taxRatesCacheService));
        }

        [HttpGet("GetTaxRates")]
        public IActionResult GetTaxRates()
        {
            try
            {
                var allRates = _taxRatesCacheService.GetAllCachedRates();
                if (allRates == null || allRates.Count == 0)
                    return NoContent();

                return Ok(allRates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetTaxRate")]
        public IActionResult GetTaxRate([FromQuery] int year, [FromQuery] string? region, [FromQuery] string property)
        {
            try
            {
                var taxRate = _taxRatesCacheService.GetTaxRateValue(year, region, property);

                if (taxRate is null)
                {
                    return NotFound($"Tax rate not found for year: {year}, region: {region}, property: {property}.");
                }

                return Ok(taxRate);
            }
            catch (Exception ex)
            {
                // Log exception here if you have a logger
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
