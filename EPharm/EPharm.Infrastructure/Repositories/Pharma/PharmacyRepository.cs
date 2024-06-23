using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.PharmaEntities;
using EPharm.Infrastructure.Interfaces;
using EPharm.Infrastructure.Interfaces.PharmaRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.Base;

namespace EPharm.Infrastructure.Repositories.PharmaRepositories;

public class PharmacyRepository(AppDbContext context)
    : Repository<Pharmacy>(context), IPharmacyRepository;
