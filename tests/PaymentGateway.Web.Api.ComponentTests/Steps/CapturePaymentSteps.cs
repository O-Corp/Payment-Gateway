using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NUnit.Framework;
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

public class RequestBuilder
{
    private CardCaptureRequest _request;

    public CardCaptureRequest Build(string paymentReference = null)
    {
        _request.PaymentReference = paymentReference;
        return _request;
    }

    public RequestBuilder WithValidRequest()
    {
        _request = new CardCaptureRequest
        {
            Amount = 10,
            Currency = "GBP",
            MerchantId = "OXXO",
            Card = new Card
            {
                Cvv = "123",
                CardNumber = "4444333322221111",
                ExpiryMonth = "12",
                ExpiryYear = "29",
                CardHolderName = "Mr Arsene Wenger"
            }
        };

        return this;
    }

    public RequestBuilder WithAmount(long amount)
    {
        _request.Amount = amount;
        return this;
    }

    public RequestBuilder WithCurrency(string currency)
    {
        _request.Currency = currency;
        return this;
    }

    public RequestBuilder WithCard(Card card)
    {
        _request.Card = card;
        return this;
    }

    public RequestBuilder WithCvv(string cvv)
    {
        _request.Card.Cvv = cvv;
        return this;
    }

    public RequestBuilder WithCardNumber(string cardNumber)
    {
        _request.Card.CardNumber = cardNumber;
        return this;
    }

    public RequestBuilder WithExpiryMonth(string expiryMonth)
    {
        _request.Card.ExpiryMonth = expiryMonth;
        return this;
    }

    public RequestBuilder WithExpiryYear(string expiryYear)
    {
        _request.Card.ExpiryYear = expiryYear;
        return this;
    }

    public RequestBuilder WithCardHolderName(string name)
    {
        _request.Card.CardHolderName = name;
        return this;
    }

    public RequestBuilder WithMerchantId(string merchantId)
    {
        _request.MerchantId = merchantId;
        return this;
    }
}