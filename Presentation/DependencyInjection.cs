using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddSwaggerDocumentationWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(swaggerGenOptions =>
        {
            swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Employees Management API.",
                Version = "v1",
                Description = "API for managing employee data, roles, and departments",
                Contact = new OpenApiContact
                {
                    Email = "saedm896@gmail.com",
                    Name = "eng.Mahmoud Saed"
                }
            });

            swaggerGenOptions.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter 'Bearer' followed by a space and your JWT token.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = JwtBearerDefaults.AuthenticationScheme
            };

            swaggerGenOptions.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
              {
                  new OpenApiSecurityScheme
                  {
                     Reference = new OpenApiReference
                     {
                         Type = ReferenceType.SecurityScheme,
                         Id = JwtBearerDefaults.AuthenticationScheme
                     }
                  },
                  Array.Empty<string>()
              },
            };

            swaggerGenOptions.AddSecurityRequirement(securityRequirement);
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management API");
        });

        return app;
    }
}
