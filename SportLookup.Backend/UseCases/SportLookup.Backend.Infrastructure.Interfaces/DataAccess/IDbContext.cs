using Microsoft.EntityFrameworkCore;
using SportLookup.Backend.Entities.Models.Auth;

namespace SportLookup.Backend.Infrastructure.Interfaces.DataAccess;

public interface IDbContext
{
    DbSet<AppUser> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    int SaveChanges();
}
