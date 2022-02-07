using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace PaymentGateway.Web.Api.ComponentTests.Steps;

[Binding]
public class HealthCheckSteps
{
    private DataContainer _dataContainer;
    private HttpResponseMessage _httpResponse;

    [Given(@"all dependencies are healthy")]
    public void GivenAllDependenciesAreHealthy()
    {
        _dataContainer = new DataContainer();
    }

    [When(@"a health check request is performed")]
    public async Task WhenAHealthCheckRequestIsPerformed()
    {
        using (var client = TestHelper.CreateHttpClient(_dataContainer))
        {
            const string requestUri = $"http://localhost/gateway/v1/healthcheck";
            _httpResponse = await client.GetAsync(requestUri);
        }
    }

    [Then(@"an (.*) health check response is returned")]
    public void ThenAHealthCheckResponseIsReturned(HttpStatusCode httpStatusCode)
    {
        Assert.That(_httpResponse.StatusCode, Is.EqualTo(httpStatusCode));
    }
}