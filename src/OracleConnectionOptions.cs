using System.ComponentModel.DataAnnotations;

namespace Korjn.OracleClientInject;

/// <summary>
/// Configuration options for connecting to an Oracle database via ODP.NET.
/// </summary>
public record OracleConnectionOptions
{
    /// <summary>
    /// Path to the tnsnames.ora file used by the Oracle client.
    /// Not used unless explicitly specified.
    /// </summary>
    public string? TnsnamesFile { get; set; }

    /// <summary>
    /// Oracle data source name (TNS alias or host:[port]/[service_name]).
    /// Required.
    /// </summary>
    [Required]
    public string? DataSource { get; set; }

    /// <summary>
    /// Default schema to use for the current Oracle session.
    /// No default; optional.
    /// </summary>
    public string? DefaultSchema { get; set; }

    /// <summary>
    /// Enables or disables connection pooling.
    /// Default: true
    /// </summary>
    public bool? Pooling { get; set; }

    /// <summary>
    /// Minimum number of connections maintained in the pool.
    /// Default: 1
    /// </summary>
    public int? MinPoolSize { get; set; }

    /// <summary>
    /// Maximum number of connections allowed in the pool.
    /// Default: 100
    /// </summary>
    public int? MaxPoolSize { get; init; }

    /// <summary>
    /// Number of connections to be incremented when the pool is exhausted.
    /// Default: 1
    /// </summary>
    public int? IncrPoolSize { get; set; }

    /// <summary>
    /// Number of connections to be decremented when connections are unused.
    /// Default: 1
    /// </summary>
    public int? DecrPoolSize { get; set; }

    /// <summary>
    /// Lifetime (in seconds) of a pooled connection before it is destroyed.
    /// Default: 0 (infinite)
    /// </summary>
    public int? ConnectionLifeTime { get; set; }

    /// <summary>
    /// Time (in seconds) to wait for a connection before throwing an exception.
    /// Default: 15
    /// </summary>
    public int? ConnectionTimeout { get; set; }

    /// <summary>
    /// Oracle database username.
    /// No default; required if using authentication.
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Oracle database password.
    /// No default; required if using authentication.
    /// </summary>
    public string? Password { get; set; }
}