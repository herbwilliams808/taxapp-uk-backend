using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxEstimationController(TaxEstimationService taxEstimationService) : ControllerBase
    {
        [HttpPost]
        public ActionResult<TaxEstimationResponse> CalculateTax(TaxEstimationRequest request)
        {
            var totalIncome = request.EmploymentIncome + request.PropertyIncome;
            var taxOwed = taxEstimationService.CalculateTax(totalIncome);

            var response = new TaxEstimationResponse
            {
                TotalIncome = totalIncome,
                TaxOwed = taxOwed,
                NetIncome = totalIncome - taxOwed - request.TaxDeducted
            };

            return Ok(response);
        }
    }
}