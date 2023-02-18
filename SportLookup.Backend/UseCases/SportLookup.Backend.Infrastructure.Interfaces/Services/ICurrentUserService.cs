namespace SportLookup.Backend.Infrastructure.Interfaces.Services;

public interface ICurrentUserService
{
    Guid Id { get; }

    bool IsAuthenticated { get; }
}
