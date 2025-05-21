using Oracle.ManagedDataAccess.Client;

namespace Korjn.OracleClientInject;

/// <summary>
/// Provides extension methods for Oracle connection.
/// </summary>
public static class OracleConnectionExtensions
{
    /// <summary>
    /// Asynchronously sets the current schema for the given Oracle connection session.
    /// </summary>
    /// <param name="connection">The <see cref="OracleConnection"/> on which to set the current schema.</param>
    /// <param name="schemaName">The name of the schema to set as the current schema.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="schemaName"/> is null or empty.</exception>
    /// <exception cref="OracleException">Thrown when the SQL command fails.</exception>
    public static async Task SetCurrentSchemaAsync(this OracleConnection connection, string? schemaName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(schemaName);

        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"ALTER SESSION SET CURRENT_SCHEMA = {schemaName}";

        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    /// <summary>
    /// Synchronously sets the current schema for the given Oracle connection session.
    /// </summary>
    /// <param name="connection">The <see cref="OracleConnection"/> on which to set the current schema.</param>
    /// <param name="schemaName">The name of the schema to set as the current schema.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="schemaName"/> is null or empty.</exception>
    /// <exception cref="OracleException">Thrown when the SQL command fails.</exception>
    public static void SetCurrentSchema(this OracleConnection connection, string? schemaName)
    {
        ArgumentException.ThrowIfNullOrEmpty(schemaName);

        using var cmd = connection.CreateCommand();
        cmd.CommandText = $"ALTER SESSION SET CURRENT_SCHEMA = {schemaName}";

        cmd.ExecuteNonQuery();
    }
}
