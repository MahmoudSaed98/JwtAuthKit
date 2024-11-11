using Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Presentation.OptionsSetup;

public class JwtOptionsSetup : IConfigureOptions<JwtSettings>
{
    private const string SectionName = "Jwt";

    private readonly IConfiguration _configuration;

    public JwtOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(JwtSettings options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}
