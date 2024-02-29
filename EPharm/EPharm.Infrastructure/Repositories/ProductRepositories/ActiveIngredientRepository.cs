using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.ProductRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;

namespace EPharm.Infrastructure.Repositories.ProductRepositories;

public class ActiveIngredientRepository(AppDbContext context)
    : Repository<ActiveIngredient>(context), IActiveIngredientRepository;
