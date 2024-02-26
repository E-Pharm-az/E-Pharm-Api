using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces;

namespace EPharm.Infrastructure.Repositories;

public class PharmaCompanyRepository : Repository<PharmaCompany>, IPharmaCompanyRepository
{
    protected PharmaCompanyRepository(AppDbContext context) : base(context)
    {
    }
}
