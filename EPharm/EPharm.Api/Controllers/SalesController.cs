using EPharm.Domain.Interfaces.CommonContracts;
using EPharmApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EPharmApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalesController(ISalesService salesService) : ControllerBase
{
    [HttpGet]
    [RequirePharmacyId]
    public async Task<IActionResult> GetSales([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int? frequency)
    {
        var pharmacyId = (int)HttpContext.Items["PharmacyId"]!;

        try
        {
            var result = await salesService.GetSalesAsync(pharmacyId, startDate, endDate, frequency);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while getting sales");
            return BadRequest(ex.Message);
        }
    }
}

