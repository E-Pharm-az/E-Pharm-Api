using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities;
using EPharm.Infrastructure.Interfaces;

namespace EPharm.Infrastructure.Repositories;

public class AddressRepository : Repository<Address>, IAddressRepository
{
    protected AddressRepository(AppDbContext context) : base(context)
    {
    }
}
