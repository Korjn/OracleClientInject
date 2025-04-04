using Oracle.ManagedDataAccess.Client;

namespace Korjn.OracleClientInject;

/// <summary>
/// Provides a factory abstraction for creating Oracle database connections.
/// </summary>
public interface IOracleConnectionFactory
{
    /// <summary>
    /// Creates and opens a configured <see cref="OracleConnection"/>.
    /// </summary>
    /// <returns>An open <see cref="OracleConnection"/> instance.</returns>
    OracleConnection CreateConnection();
}