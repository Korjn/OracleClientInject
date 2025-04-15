using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Korjn.OracleClientInject.DependencyInjection;

/// <summary>
/// Extension methods to register the Oracle connection factory and its configuration
/// into the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the <see cref="IOracleConnectionFactory"/> and binds <see cref="OracleConnectionOptions"/> using the <see cref="IOptions{TOptions}"/> pattern.
    /// Validates options on application startup.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configure">A delegate to configure the <see cref="OracleConnectionOptions"/>.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddOracleClient(this IServiceCollection services, Action<OracleConnectionOptions> configure)
    {
        services.AddOptions<OracleConnectionOptions>()
                .Configure(configure);

        services.AddSingleton<IOracleConnectionFactory, OracleConnectionFactory>();

        return services;
    }

    /// <summary>
    /// Registers the <see cref="IOracleConnectionFactory"/> and binds <see cref="OracleConnectionOptions"/>
    /// using a delegate that has access to the <see cref="IServiceProvider"/>.
    /// This is useful when options need to be configured based on other services, such as <c>ILogger</c>, <c>IHostEnvironment</c>,
    /// or secret providers.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configureOptions">
    /// A delegate that configures <see cref="OracleConnectionOptions"/>, with access to the current <see cref="IServiceProvider"/>.
    /// </param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    /// <remarks>
    /// This method registers <see cref="OracleConnectionOptions"/> with validation on startup,
    /// and adds <see cref="OracleConnectionFactory"/> as a singleton service.
    /// </remarks>
    public static IServiceCollection AddOracleClient(this IServiceCollection services, Action<OracleConnectionOptions, IServiceProvider> configureOptions)
    {
        services.AddOptions<OracleConnectionOptions>();

        services.AddSingleton<IPostConfigureOptions<OracleConnectionOptions>>(sp
            => new PostConfigureOptions<OracleConnectionOptions, IServiceProvider>(Options.DefaultName, sp, configureOptions));        

        services.AddSingleton<IOracleConnectionFactory, OracleConnectionFactory>();

        return services;
    }
}