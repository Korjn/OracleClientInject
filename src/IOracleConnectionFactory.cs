using Oracle.ManagedDataAccess.Client;

namespace Korjn.OracleClientInject;

/// <summary>
/// Provides a factory abstraction for creating Oracle database connections.
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
    /// Creates and opens a configured <see cref="OracleConnection"/>.
    /// </summary>
    /// <returns>An open <see cref="OracleConnection"/> instance.</returns>
    OracleConnection CreateConnection();

    /// <summary>
    /// Creates and opens a configured <see cref="OracleConnection"/> with the specified user credentials.
    /// </summary>
    /// <param name="userName">The Oracle user name.</param>
    /// <param name="password">The Oracle password.</param>
    /// <returns>An open <see cref="OracleConnection"/> instance.</returns>
    OracleConnection CreateConnection(string userName, string password);

    /// <summary>
    /// Asynchronously creates and opens a configured <see cref="OracleConnection"/>.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The result contains an open <see cref="OracleConnection"/> instance.</returns>
    Task<OracleConnection> CreateConnectionAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Asynchronously creates and opens a configured <see cref="OracleConnection"/> with the specified user credentials.
    /// </summary>
    /// <param name="userName">The Oracle user name.</param>
    /// <param name="password">The Oracle password.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The result contains an open <see cref="OracleConnection"/> instance.</returns>
    Task<OracleConnection> CreateConnectionAsync(string userName, string password, CancellationToken cancellationToken);
}