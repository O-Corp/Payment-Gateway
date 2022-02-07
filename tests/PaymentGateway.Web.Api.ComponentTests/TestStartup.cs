using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PaymentGateway.Web.Api.ComponentTests;

internal class TestStartup : Startup
{
    private readonly DataContainer _dataContainer;

    public TestStartup(
        IConfiguration configuration,
        DataContainer dataContainer = null) : base(configuration)
    {
        _dataContainer = dataContainer;
    }
        
    protected override void ConfigureDependencies(IServiceCollection services)
    {
    }
}