using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EPharmApi.Attributes;

public class RequirePharmacyIdAttribute : Attribute, IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;

        if (user.IsInRole(IdentityData.Admin))
            return;
        
        var pharmacyIdClaim = context.HttpContext.User.FindFirst("PharmacyId");
        if (pharmacyIdClaim == null || !int.TryParse(pharmacyIdClaim.Value, out var pharmacyId))
        {
            context.Result = new BadRequestObjectResult("Invalid or missing PharmacyId");
            return;
        }

        context.HttpContext.Items["PharmacyId"] = pharmacyId;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
