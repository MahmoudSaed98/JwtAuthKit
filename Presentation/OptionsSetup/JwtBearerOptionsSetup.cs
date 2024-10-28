using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Presentation.OptionsSetup;

public sealed class JwtBearerOptionsSetup : IPostConfigureOptions<JwtBearerOptions>
{
    private readonly JwtSettings _options;

    public JwtBearerOptionsSetup(IOptions<JwtSettings> options)
    {
        this._options = options.Value;
    }

    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        options.SaveToken = true;
        options.TokenValidationParameters.ValidateIssuer = true;
        options.TokenValidationParameters.ValidateLifetime = true;
        options.TokenValidationParameters.ValidateAudience = true;
        options.TokenValidationParameters.ValidateIssuerSigningKey = true;
        options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
        options.TokenValidationParameters.ValidIssuer = _options.Issuer;
        options.TokenValidationParameters.ValidAudience = _options.Audience;
        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_options.SecretKey));

    }
}