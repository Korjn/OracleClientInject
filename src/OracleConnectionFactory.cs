using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;

namespace Korjn.OracleClientInject;

internal class OracleConnectionFactory : IOracleConnectionFactory
{
    private readonly ILogger<IOracleConnectionFactory> log;
    private readonly OracleConnectionOptions _options;
    private readonly Lazy<string> _connectionStringAttributes;
    private readonly Lazy<string> _connectionStringSecurity;

    public OracleConnectionFactory(ILogger<IOracleConnectionFactory> logger, IOptions<OracleConnectionOptions> options)
    {
        log = logger;
        _options = options.Value;

        _connectionStringAttributes = new Lazy<string>(BuildConnectionStringAttributes);
        _connectionStringSecurity = new Lazy<string>(() => BuildUserPasswordConnectionString(_options.UserName, _options.Password));
    }

    public OracleConnectionOptions ConnectionOptions => _options;
    public string ConnectionStringAttributes => _connectionStringAttributes.Value;
    public string ConnectionString => _connectionStringSecurity.Value;

    private string BuildUserPasswordConnectionString(string? userName, string? password) =>
    $"User Id={userName};Password={password};{_connectionStringAttributes.Value}";


    private string BuildConnectionStringAttributes()
    {
        if (!string.IsNullOrEmpty(_options.TnsnamesFile))
        {
            /*
                The tnsnames.ora precedence order is as follows
                File location specified by TNS_ADMIN setting       
                https://docs.oracle.com/cd/E85694_01/ODPNT/InstallConfig.htm#ODPNT8153    
                https://github.com/oracle/dotnet-db-samples/blob/master/session-demos/2021/cicqn-json/cicqn-json.cs                   
            */

            Environment.SetEnvironmentVariable("TNS_ADMIN", Path.GetDirectoryName(Path.GetFullPath(_options.TnsnamesFile)));
        }

        var builder = new OracleConnectionStringBuilder()
        {
            DataSource = _options.DataSource
        };

        if (_options.Pooling.HasValue)
        {
            builder.Pooling = _options.Pooling.Value;
        }

        if (_options.MinPoolSize.HasValue)
        {
            builder.MinPoolSize = _options.MinPoolSize.Value;
        }

        if (_options.MaxPoolSize.HasValue)
        {
            builder.MaxPoolSize = _options.MaxPoolSize.Value;
        }

        if (_options.IncrPoolSize.HasValue)
        {
            builder.IncrPoolSize = _options.IncrPoolSize.Value;
        }

        if (_options.DecrPoolSize.HasValue)
        {
            builder.DecrPoolSize = _options.DecrPoolSize.Value;
        }

        if (_options.ConnectionLifeTime.HasValue)
        {
            builder.ConnectionLifeTime = _options.ConnectionLifeTime.Value;
        }

        if (_options.ConnectionTimeout.HasValue)
        {
            builder.ConnectionTimeout = _options.ConnectionTimeout.Value;
        }

        var result = builder.ToString();

        log.LogTrace("Oracle ConnectionString={connectionString}", result);

        return result;
    }

    private void SetConnectionOpen(OracleConnection connection, Action<OracleConnectionOpenEventArgs>? connectionOpen = default)
    {
        if (!string.IsNullOrEmpty(_options.DefaultSchema) || _options.ConnectionOpen is not null || connectionOpen is not null)
        {
            connection.ConnectionOpen += args =>
            {
                if (!string.IsNullOrEmpty(_options.DefaultSchema))
                {
                    args.Connection.SetCurrentSchema(_options.DefaultSchema);
                }

                _options.ConnectionOpen?.Invoke(args);
                connectionOpen?.Invoke(args);
            };
        }
    }

    private async Task<OracleConnection> CreateConnectionAsync(string connectionString,
                                                               Action<OracleConnectionOpenEventArgs>? connectionOpen = default,
                                                               CancellationToken cancellationToken = default)
    {
        var result = new OracleConnection(connectionString);

        try
        {
            SetConnectionOpen(result, connectionOpen);

            await result.OpenAsync(cancellationToken);

            return result;
        }
        catch (Exception e)
        {
            log.LogError(e, "Failed to open Oracle connection <{connectionString}>", _connectionStringAttributes.Value);
            result.Dispose();
            throw new InvalidOperationException("Failed to open Oracle connection.", e);
        }
    }

    private OracleConnection CreateConnection(string connectionString, Action<OracleConnectionOpenEventArgs>? connectionOpen = default)
    {
        var result = new OracleConnection(connectionString);

        try
        {
            SetConnectionOpen(result, connectionOpen);

            result.Open();

            return result;
        }
        catch (Exception e)
        {
            log.LogError(e, "Failed to open Oracle connection <{connectionString}>", _connectionStringAttributes.Value);
            result.Dispose();
            throw new InvalidOperationException("Failed to open Oracle connection.", e);
        }
    }

    public async Task<OracleConnection> CreateConnectionAsync(Action<OracleConnectionOpenEventArgs>? connectionOpen = default,
                                                              CancellationToken cancellationToken = default) =>
        await CreateConnectionAsync(_connectionStringSecurity.Value, connectionOpen, cancellationToken);

    public async Task<OracleConnection> CreateConnectionAsync(string userName,
                                                              string password,
                                                              Action<OracleConnectionOpenEventArgs>? connectionOpen = default,
                                                              CancellationToken cancellationToken = default) =>
    await CreateConnectionAsync(BuildUserPasswordConnectionString(userName, password), connectionOpen, cancellationToken);

    public OracleConnection CreateConnection(Action<OracleConnectionOpenEventArgs>? connectionOpen = default) =>
        CreateConnection(_connectionStringSecurity.Value, connectionOpen);

    public OracleConnection CreateConnection(string userName,
                                             string password,
                                             Action<OracleConnectionOpenEventArgs>? connectionOpen = default) =>
        CreateConnection(BuildUserPasswordConnectionString(userName, password), connectionOpen);
}