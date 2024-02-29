using EPharm.Infrastructure.Context;
using EPharm.Infrastructure.Context.Entities.CommonEntities;
using EPharm.Infrastructure.Interfaces;
using EPharm.Infrastructure.Interfaces.CommonRepositoriesInterfaces;
using EPharm.Infrastructure.Repositories.BaseRepositories;

namespace EPharm.Infrastructure.Repositories.CommonRepositories;

public class AddressRepository(AppDbContext context) : Repository<Address>(context), IAddressRepository;
