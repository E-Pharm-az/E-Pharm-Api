using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities;
using EPharm.Infrastructure.Interfaces;

namespace EPharm.Infrastructure.Repositories;

public class PharmaCompanyManagerRepository : Repository<PharmaCompanyManager>, IPharmaCompanyManagerRepository
{
    protected PharmaCompanyManagerRepository(AppDbContext context) : base(context)
    {
    }
}
