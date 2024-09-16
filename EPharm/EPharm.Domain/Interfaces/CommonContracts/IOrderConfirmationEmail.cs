using EPharm.Infrastructure.Entities.Identity;
using EPharm.Infrastructure.Entities.ProductEntities;

namespace EPharm.Domain.Interfaces.CommonContracts;

public interface IOrderConfirmationEmail
{
    public string GenerateEmail(Order order, AppIdentityUser customer);
 
}