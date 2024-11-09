using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddSwaggerDocumentationWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(swaggerGenOptions =>
        {
            swaggerGenOptions.SwaggerDoc("1.0", new OpenApiInfo
            {
                Title = "Employees Management API.",
                Version = "1.0",
                Description = "API for managing employee data, roles, and departments",
                Contact = new OpenApiContact
                {
                    Email = "saedm896@gmail.com",
                    Name = "eng.Mahmoud Saed"
                }
            });

            swaggerGenOptions.SwaggerDoc("2.0", new()
            {
                Title = "Employees Management API.",
                Version = "2.0",
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


            //swaggerGenOptions.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            //swaggerGenOptions.DocInclusionPredicate((version, apiDesc) =>
            //{

            //    if (!apiDesc.TryGetMethodInfo(out MethodInfo method))
            //        return false;
            //    var methodVersions = method.GetCustomAttributes(true)
            //        .OfType<ApiVersionAttribute>()
            //        .SelectMany(attr => attr.Versions);

            //    var controllerVersions = method.DeclaringType?
            //        .GetCustomAttributes(true)
            //        .OfType<ApiVersionAttribute>()
            //        .SelectMany(attr => attr.Versions);

            //    var allVersions = methodVersions.Union(controllerVersions!).Distinct();

            //    return allVersions.Any(v => v.ToString() == version);
            //});

        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/1.0/swagger.json", "Employee Management API v1.0");
            // options.SwaggerEndpoint("/swagger/2.0/swagger.json", "Employee Management API v2.0");
        });

        return app;
    }
}
