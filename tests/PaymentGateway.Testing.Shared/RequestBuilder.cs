using PaymentGateway.Web.Api.Models;

namespace PaymentGateway.Testing.Shared;

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