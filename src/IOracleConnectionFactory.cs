using Oracle.ManagedDataAccess.Client;

namespace Korjn.OracleClientInject;

/// <summary>
/// Provides methods for creating Oracle database connections
/// </summary>
public interface IOracleConnectionFactory
{
    /// <summary>
    /// Gets the current configuration options used for creating Oracle database connections.
    /// </summary>
    OracleConnectionOptions ConnectionOptions { get; }

    /// <summary>
    /// Gets the Oracle connection string attributes without credentials.
    /// </summary>
    string ConnectionStringAttributes { get; }

    /// <summary>
    /// Gets the full Oracle connection string including credentials.
    /// </summary>
    string ConnectionString { get; }

    /// <summary>
    /// Asynchronously creates and opens an Oracle database connection using the default user credentials.
    /// </summary>
    /// <param name="connectionOpen">An optional callback invoked when the connection is successfully opened.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, containing an open <see cref="OracleConnection"/> instance.</returns>
    Task<OracleConnection> CreateConnectionAsync(Action<OracleConnectionOpenEventArgs>? connectionOpen = default,
                                                 CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously creates and opens an Oracle database connection using the provided user name and password.
    /// </summary>
    /// <param name="userName">The Oracle user name.</param>
    /// <param name="password">The Oracle user password.</param>
    /// <param name="connectionOpen">An optional callback invoked when the connection is successfully opened.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation, containing an open <see cref="OracleConnection"/> instance.</returns>
    Task<OracleConnection> CreateConnectionAsync(string userName, string password,
                                                 Action<OracleConnectionOpenEventArgs>? connectionOpen = default,
                                                 CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates and opens a synchronous Oracle database connection using the default user credentials.
    /// </summary>
    /// <param name="connectionOpen">An optional callback invoked when the connection is successfully opened.</param>
    /// <returns>An open <see cref="OracleConnection"/> instance.</returns>
    OracleConnection CreateConnection(Action<OracleConnectionOpenEventArgs>? connectionOpen = default);

    /// <summary>
    /// Creates and opens a synchronous Oracle database connection using the provided user name and password.
    /// </summary>
    /// <param name="userName">The Oracle user name.</param>
    /// <param name="password">The Oracle user password.</param>
    /// <param name="connectionOpen">An optional callback invoked when the connection is successfully opened.</param>
    /// <returns>An open <see cref="OracleConnection"/> instance.</returns>
    OracleConnection CreateConnection(string userName, string password, Action<OracleConnectionOpenEventArgs>? connectionOpen = default);
}