using EPharm.Domain.Interfaces.PharmaContracts;
using EPharm.Domain.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace EPharmApi.Filters;

public class PharmacyOwnerFilter(IPharmacyService pharmacyService) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var pharmacyIdString = context.RouteData.Values["pharmacyId"] as string;
        if (!int.TryParse(pharmacyIdString, out var pharmacyId))
        {
            context.Result = new BadRequestObjectResult("Invalid pharmacy ID.");
            return;
        }

        var company = await pharmacyService.GetPharmacyByIdAsync(pharmacyId);
        if (company is null)
        {
            context.Result = new NotFoundObjectResult("Pharmaceutical company not found.");
            return;
        }

        var user = context.HttpContext.User;
        if (!user.IsInRole(IdentityData.Admin))
        {
            var userId = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            if (userId != company.Owner.Id)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}