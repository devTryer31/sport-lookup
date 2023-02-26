namespace SportLookup.Backend.Entities.Configuration;

public class JWTConfig
{
    public JWTConfig(string key, string apiAudience)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
        if (string.IsNullOrWhiteSpace(apiAudience))
            throw new ArgumentException($"'{nameof(apiAudience)}' cannot be null or whitespace.", nameof(apiAudience));

        Secret = System.Text.Encoding.UTF8.GetBytes(key);
        ApiAudience = apiAudience;
    }

    public string ApiAudience { get; }

    public byte[] Secret { get; }
}
