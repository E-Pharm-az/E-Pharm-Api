using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class IndicationProductRepository(AppDbContext context)
    : Repository<IndicationProduct>(context), IIndicationProductRepository;
