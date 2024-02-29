using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces;
using EPharm.Infrastructure.Interfaces.PharmaRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;

namespace EPharm.Infrastructure.Repositories.PharmaRepositories;

public class PharmaCompanyRepository(AppDbContext context)
    : Repository<PharmaCompany>(context), IPharmaCompanyRepository;
