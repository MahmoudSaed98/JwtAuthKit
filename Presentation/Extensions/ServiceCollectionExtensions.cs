using Presentation.OptionsSetup;

namespace Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureJwtOptions(this IServiceCollection services)
    {
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        return services;
    }
}