using System.Net;
using TechTalk.SpecFlow;

namespace PaymentGateway.Web.Api.ComponentTests.Steps
{
    [Binding]
    public class HealthCheckSteps
    {
        [Given(@"all dependencies are healthy")]
        public void GivenAllDependenciesAreHealthy()
        {
        }

        [When(@"a health check request is performed")]
        public void WhenAHealthCheckRequestIsPerformed()
        {
        }

        [Then(@"an (.*) health check response is returned")]
        public void ThenAHealthCheckResponseIsReturned(HttpStatusCode httpStatusCode)
        {
        }
    }
}