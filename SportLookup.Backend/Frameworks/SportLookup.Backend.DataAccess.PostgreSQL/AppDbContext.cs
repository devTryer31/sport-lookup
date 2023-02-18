using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportLookup.Backend.Entities.Models.Auth;
using SportLookup.Backend.Infrastructure.Interfaces.DataAccess;
using System.Reflection.Emit;

namespace SportLookup.Backend.DataAccess.PostgreSQL;

public class AppDbContext : IdentityDbContext<AppUser, AppUserRole, Guid>, IDbContext
{
    private readonly IDbSeeder? _seeder;

    public AppDbContext(DbContextOptions options, IDbSeeder? seeder = null) : base(options)
    {
        _seeder = seeder;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        _seeder?.Seed(builder);
        base.OnModelCreating(builder);
    }
}