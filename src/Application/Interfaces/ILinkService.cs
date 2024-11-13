namespace Application.Interfaces;

public interface ILinkService
{
    string Generate(string endpointName, object routeValue);
}
