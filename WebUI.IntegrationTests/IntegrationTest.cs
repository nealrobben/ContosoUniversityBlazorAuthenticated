
using Microsoft.AspNetCore.Mvc.Testing;
using WebUI.Server;

namespace WebUI.IntegrationTests;

public class IntegrationTest
{
    protected readonly HttpClient _client;
    protected readonly WebApplicationFactory<Program> _appFactory;

    protected IntegrationTest()
    {
        Environment.SetEnvironmentVariable("UseInMemoryDatabase", "true");

        _appFactory = new WebApplicationFactory<Program>();

        _client = _appFactory.CreateClient();
    }
}