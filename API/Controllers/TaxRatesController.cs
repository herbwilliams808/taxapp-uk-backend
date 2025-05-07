using Microsoft.AspNetCore.Mvc;
using Application.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxRatesController(AzureBlobTaxRatesService taxRatesService) : ControllerBase
    {
        [HttpGet("GetTaxRates")]
        public async Task<IActionResult> GetTaxRates()
        {
            try
            {
                var taxRates = await taxRatesService.LoadTaxRatesAsync();
                return Ok(taxRates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}