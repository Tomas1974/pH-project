namespace api;

public class Utilities
{
    /*
     * Private static readonly Uri instance initialized with the PostgreSQL connection string.
     */
    private static readonly Uri Uri = new(Environment.GetEnvironmentVariable("pgconn")!);

    /*
     * Publicly accessible properly formatted PostgreSQL connection string.
     */
    public static readonly string
        ProperlyFormattedConnectionString = string.Format(
            "Server={0};Database={1};User Id={2};Password={3};Port={4};Pooling=false;",
            Uri.Host,
            Uri.AbsolutePath.Trim('/'),
            Uri.UserInfo.Split(':')[0],
            Uri.UserInfo.Split(':')[1],
            Uri.Port > 0 ? Uri.Port : 5432);
}