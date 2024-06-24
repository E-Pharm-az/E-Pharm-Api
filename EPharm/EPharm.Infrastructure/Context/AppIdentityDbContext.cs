using EPharm.Infrastructure.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EPharm.Infrastructure.Context;

public class AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
    : IdentityDbContext<AppIdentityUser>(options);
