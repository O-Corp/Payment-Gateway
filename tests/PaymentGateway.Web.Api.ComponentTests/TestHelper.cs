using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace PaymentGateway.Web.Api.ComponentTests;

internal static class TestHelper
{
    public static HttpClient CreateHttpClient(DataContainer dataContainer)
    {
        var hostBuilder = new WebHostBuilder()
            .ConfigureServices(s => s.AddSingleton(dataContainer))
            .UseStartup<TestStartup>();

        var server = new TestServer(hostBuilder);
        return server.CreateClient();
    }
}