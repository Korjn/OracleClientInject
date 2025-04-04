using System.ComponentModel.DataAnnotations;

namespace Korjn.OracleClientInject;

/// <summary>
/// Configuration options for connecting to an Oracle database via ODP.NET.
/// </summary>
public record OracleConnectionOptions
{
    /// <summary>
    /// Path to the tnsnames.ora file used by Oracle client.
    /// </summary>
    [Required]
    public string? TnsnamesFile { get; init; }

    /// <summary>
    /// Oracle data source name (TNS alias).
    /// </summary>
    [Required]
    public string? DataSource { get; init; }

    /// <summary>
    /// Schema to use for the current Oracle session.
    /// </summary>
    public string? DefaultSchema { get; init; }

    /// <summary>
    /// Maximum number of connections allowed in the pool.
    /// </summary>
    public int MaxPoolSize { get; init; } = 10;

    /// <summary>
    /// Oracle database username.
    /// </summary>
    [Required]
    public string? UserName { get; set; }

    /// <summary>
    /// Oracle database password.
    /// </summary>
    [Required]
    public string? Password { get; set; }
}
