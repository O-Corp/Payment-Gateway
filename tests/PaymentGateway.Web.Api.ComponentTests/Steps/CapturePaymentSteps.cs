using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NUnit.Framework;
using PaymentGateway.Testing.Shared;
using PaymentGateway.Web.Api.Models;
using TechTalk.SpecFlow;

namespace PaymentGateway.Web.Api.ComponentTests.Steps;

[Binding]
public class CapturePaymentSteps
{
    private string _paymentReference;
    private DataContainer _dataContainer;
    private CardCaptureRequest _payload;
    private HttpResponseMessage _httpResponse;
    private List<ApiError> _errors;

    [Before]
    public void Setup()
    {
        _paymentReference = Guid.NewGuid().ToString();
        _dataContainer = new DataContainer();
    }

    [Given(@"an invalid payment request")]
    public void GivenAnInvalidPaymentRequest()
    {
        _payload = new RequestBuilder()
            .WithValidRequest()
            .WithCard(null)
            .WithAmount(0)
            .Build();
    }

    [When(@"a payment request is sent")]
    public async Task WhenAPaymentRequestIsSent()
    {
        using (var client = TestHelper.CreateHttpClient(_dataContainer))
        {
            var requestUri = $"http://localhost/gateway/v1/payments/{_paymentReference}";
            _httpResponse = await client.PutAsJsonAsync(requestUri, _payload);
        }
    }

    [Then(@"a (.*) response is returned")]
    public void ThenAResponseIsReturned(HttpStatusCode httpStatusCode)
    {
        Assert.That(_httpResponse.StatusCode, Is.EqualTo(httpStatusCode));
    }

    [Then(@"the error code is (.*) and message is (.*)")]
    public async Task ThenTheErrorCodeIsAndMessageIs(string code, string message)
    {
        _errors ??= await _httpResponse.Content.ReadFromJsonAsync<List<ApiError>>();
        var error = _errors.First(x => x.ErrorCode == code);

        Assert.That(error.ErrorCode, Is.EqualTo(code));
        Assert.That(error.Message, Is.EqualTo(message));
    }
}