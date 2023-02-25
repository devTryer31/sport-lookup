namespace SportLookup.Backend.Entities.Exceptions;

public class UserNotFoundException : ArgumentException
{
    public UserNotFoundException(string msg, string userName)
        : base($"Connot find user with name {userName}. {msg}.")
    { }

    public UserNotFoundException(string userName)
        : base($"Connot find user with name {userName}.")
    { }
}
