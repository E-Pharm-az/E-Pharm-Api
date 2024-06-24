using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Interfaces.Pharma;
using EPharm.Infrastructure.Repositories.Base;

namespace EPharm.Infrastructure.Repositories.Pharma;

public class PharmacyRepository(AppDbContext context)
    : Repository<Infrastructure.Entities.PharmaEntities.Pharmacy>(context), IPharmacyRepository;
