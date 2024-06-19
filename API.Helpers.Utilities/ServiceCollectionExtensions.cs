using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers.Utilities;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAPIHelpersUtilities(this IServiceCollection services)
    {
        services.AddScoped<IFunctionUtility, FunctionUtility>();
        services.AddScoped<IJwtUtility, JwtUtility>();
        services.AddScoped<IMailUtility, MailUtility>();
        
        return services;
    }
}