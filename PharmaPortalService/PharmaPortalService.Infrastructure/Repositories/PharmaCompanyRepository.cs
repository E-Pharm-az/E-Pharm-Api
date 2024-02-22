using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities;
using PharmaPortalService.Infrastructure.Interfaces;

namespace PharmaPortalService.Infrastructure.Repositories;

public class PharmaCompanyRepository : Repository<PharmaCompany>, IPharmaCompanyRepository
{
    protected PharmaCompanyRepository(AppDbContext context) : base(context)
    {
    }
}
