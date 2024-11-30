using Application.Interfaces;
using Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPermissionManager, PermissionManager>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailContentGenerator, EmailContentGenerator>();
        services.AddScoped<IEmailConfirmationTokenProvider, EmailConfirmationTokenProvider>();
        services.AddValidatorsFromAssembly(ApplicationAssemblyReference.Assembly);
        return services;
    }
}