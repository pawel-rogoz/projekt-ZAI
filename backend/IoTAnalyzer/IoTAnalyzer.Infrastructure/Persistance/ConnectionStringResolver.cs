using Npgsql;

namespace IoTAnalyzer.Infrastructure.Persistance;

public static class ConnectionStringResolver
{
    public static string? Resolve(string? connectionString, string? databaseUrl)
    {
        if (!string.IsNullOrWhiteSpace(connectionString))
            return connectionString;
        if (string.IsNullOrWhiteSpace(databaseUrl))
            return null;

        var uri = new Uri(databaseUrl);
        var credentials = uri.UserInfo.Split(':', 2);
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port > 0 ? uri.Port : 5432,
            Database = Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/')),
            Username = Uri.UnescapeDataString(credentials[0]),
            Password = credentials.Length > 1 ? Uri.UnescapeDataString(credentials[1]) : null,
        };

        foreach (var parameter in uri.Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var pair = parameter.Split('=', 2);
            if (pair.Length == 2 && pair[0].Equals("sslmode", StringComparison.OrdinalIgnoreCase)
                && Enum.TryParse<SslMode>(pair[1], ignoreCase: true, out var sslMode))
                builder.SslMode = sslMode;
        }

        return builder.ConnectionString;
    }
}
