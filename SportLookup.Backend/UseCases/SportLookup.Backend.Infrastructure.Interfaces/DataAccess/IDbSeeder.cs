using Microsoft.EntityFrameworkCore;

namespace SportLookup.Backend.Infrastructure.Interfaces.DataAccess;

public interface IDbSeeder
{
    void Seed(ModelBuilder modelBuilder);
}
