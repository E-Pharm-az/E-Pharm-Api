using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Entities.ProductEntities;
using EPharm.Infrastructure.Interfaces.Entities;
using EPharm.Infrastructure.Repositories.Base;

namespace EPharm.Infrastructure.Repositories.Entities;

public class AllergyRepository(AppDbContext context) : Repository<Allergy>(context), IAllergyRepository;
