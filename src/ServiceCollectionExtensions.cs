using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Korjn.OracleClientInject;

/// <summary>
/// Extension methods to register Oracle connection factory and options into the dependency injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the OracleConnectionFactory and its configuration using the IOptions pattern.
    /// </summary>
    public static IServiceCollection AddOracleClient(this IServiceCollection services, Action<OracleConnectionOptions> configure)
    {
        services.AddOptionsWithValidateOnStart<OracleConnectionOptions>()
                .Configure(configure);

        services.AddSingleton<IOracleConnectionFactory, OracleConnectionFactory>();

        return services;
    }

    /// <summary>
    /// Registers the <see cref="IOracleConnectionFactory"/> and its configuration
    /// using a <see cref="PostConfigureOptions{TOptions,TDependency}"/> delegate with access to the <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to register services with.</param>
    /// <param name="configureOptions">
    /// A delegate used to configure <see cref="OracleConnectionOptions"/> with access to resolved dependencies.
    /// This is useful when you need to inject services such as <c>ILogger</c>, <c>IHostEnvironment</c>,
    /// or <c>ICredentialService</c> during options configuration.
    /// </param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    /// <remarks>
    /// This method registers <see cref="OracleConnectionOptions"/> with validation on startup,
    /// and adds the <see cref="OracleConnectionFactory"/> as a singleton service.
    /// </remarks>
    public static IServiceCollection AddOracleClient(this IServiceCollection services, Action<OracleConnectionOptions, IServiceProvider> configureOptions)
    {
        services.AddOptionsWithValidateOnStart<OracleConnectionOptions>();
        services.AddSingleton(sp => new PostConfigureOptions<OracleConnectionOptions, IServiceProvider>(Options.DefaultName, sp, configureOptions));

        services.AddSingleton<IOracleConnectionFactory, OracleConnectionFactory>();

        return services;
    }
}