using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities;
using PharmaPortalService.Infrastructure.Interfaces;

namespace PharmaPortalService.Infrastructure.Repositories;

public class AddressRepository : Repository<Address>, IAddressRepository
{
    protected AddressRepository(AppDbContext context) : base(context)
    {
    }
}
