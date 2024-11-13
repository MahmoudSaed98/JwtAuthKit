using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Infrastructure.Services;

internal sealed class LinkService : ILinkService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly LinkGenerator _linkGenerator;
    public LinkService(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
    {
        _httpContextAccessor = httpContextAccessor;
        _linkGenerator = linkGenerator;
    }

    public string Generate(string endpointName, object routeValue)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        string? link = _linkGenerator.GetUriByName(httpContext!, endpointName, routeValue);

        return link ?? throw new InvalidOperationException("could not generate virification link.");
    }
}