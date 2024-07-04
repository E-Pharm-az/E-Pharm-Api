using EPharmApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EPharmApi.Attributes;

public class PharmacyOwnerAttribute() : TypeFilterAttribute(typeof(PharmacyOwnerFilter))
{
}