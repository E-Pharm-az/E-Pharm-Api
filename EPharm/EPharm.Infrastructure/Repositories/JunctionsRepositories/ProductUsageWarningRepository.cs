using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.Junctions;
using EPharm.Infrastructure.Interfaces.JunctionsRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;

namespace EPharm.Infrastructure.Repositories.JunctionsRepositories;

public class ProductUsageWarningRepository(AppDbContext context)
    : Repository<ProductUsageWarning>(context), IProductUsageWarningRepository;
