using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Repositories.Base;

namespace EPharm.Infrastructure.Repositories.Entities;

public class RouteOfAdministrationRepository(AppDbContext context)
    : Repository<RouteOfAdministration>(context), IRouteOfAdministrationRepository;
