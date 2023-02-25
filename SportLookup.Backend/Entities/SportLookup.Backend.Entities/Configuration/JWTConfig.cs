namespace SportLookup.Backend.Entities.Configuration;

public class JWTConfig
{
    public JWTConfig(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));

        Secret = System.Text.Encoding.UTF8.GetBytes(key);
    }

    public byte[] Secret { get; }
}
