﻿using Application.Interfaces;
using Domain.Abstractions;
using Infrastructure.Authentication;
using Infrastructure.Authorization.Handlers;
using Infrastructure.Data.DbContexts;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var emailSettings = new EmailSettings();

        configuration.GetSection(EmailSettings.SectionName).Bind(emailSettings);

        var connectionString = configuration.GetConnectionString("Default");

        ArgumentNullException.ThrowIfNull(nameof(connectionString));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, config =>
            {
                config.MigrationsAssembly(nameof(Infrastructure));
            });

        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccessTokenService, AccessTokenService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IConfirmationTokenRepository, EmailVerificationTokenRepository>();
        services.AddScoped<ILinkService, LinkService>();

        services.AddFluentEmail(emailSettings.SenderEmail, emailSettings.SenderName)
                 .AddSmtpSender(emailSettings.SmtpServer, emailSettings.Port);

        return services;
    }
}