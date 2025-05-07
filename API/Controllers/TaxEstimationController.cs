using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxEstimationController : ControllerBase
    {
        private readonly TaxCalculationService _taxCalculationService;

        public TaxEstimationController()
        {
            _taxCalculationService = new TaxCalculationService();
        }

        [HttpPost]
        public ActionResult<TaxCalculationResponse> CalculateTax(TaxCalculationRequest request)
        {
            var totalIncome = request.EmploymentIncome + request.PropertyIncome;
            var taxOwed = _taxCalculationService.CalculateTax(totalIncome);

            var response = new TaxCalculationResponse
            {
                TotalIncome = totalIncome,
                TaxOwed = taxOwed,
                NetIncome = totalIncome - taxOwed - request.TaxDeducted
            };

            return Ok(response);
        }
    }
}