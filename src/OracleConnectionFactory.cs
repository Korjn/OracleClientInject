using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;

namespace Korjn.OracleClientInject;

internal class OracleConnectionFactory : IOracleConnectionFactory
{
    private readonly OracleConnectionOptions _options;
    private readonly Lazy<string> _connectionString = new(() => throw new InvalidOperationException());

    public OracleConnectionFactory(IOptions<OracleConnectionOptions> options)
    {
        _options = options.Value;
        _connectionString = new Lazy<string>(BuildConnectionString);
    }

    private string BuildConnectionString()
    {
        return new OracleConnectionStringBuilder
        {
            DataSource = _options.DataSource,
            Pooling = true,
            MinPoolSize = 1,
            MaxPoolSize = _options.MaxPoolSize,
            IncrPoolSize = 1,
            DecrPoolSize = 1,
            UserID = _options.UserName,
            Password = _options.Password
        }.ToString();
    }

    public OracleConnection CreateConnection()
    {
        var conn = new OracleConnection(_connectionString.Value);

        try
        {
            conn.Open();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to open Oracle connection.", ex);
        }

        if (!string.IsNullOrEmpty(_options.DefaultSchema))
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "alter session set current_schema=:schema";
            cmd.Parameters.Add(new OracleParameter("schema", _options.DefaultSchema));
            cmd.ExecuteNonQuery();
        }

        return conn;
    }
}