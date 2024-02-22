using PharmaPortalService.Infrastructure.Context;
using PharmaPortalService.Infrastructure.Context.Entities;
using PharmaPortalService.Infrastructure.Interfaces;

namespace PharmaPortalService.Infrastructure.Repositories;

public class PharmaCompanyManagerRepository : Repository<PharmaCompanyManager>, IPharmaCompanyManagerRepository
{
    protected PharmaCompanyManagerRepository(AppDbContext context) : base(context)
    {
    }
}
