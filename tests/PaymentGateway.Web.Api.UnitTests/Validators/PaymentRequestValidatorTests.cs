using System.Linq;
using NUnit.Framework;
using PaymentGateway.Testing.Shared;
using PaymentGateway.Web.Api.Validators;

namespace PaymentGateway.Web.Api.UnitTests.Validators;

public class PaymentRequestValidatorTests
{
    private PaymentRequestValidator _subject;

    [SetUp]
    public void Setup()
    {
        _subject = new PaymentRequestValidator();
    }

    [TestCase(0)]
    [TestCase(501)]
    [TestCase(-10)]
    public void Given_Invalid_Amount_Then_Return_Error(long amount)
    {
        var request = new RequestBuilder()
            .WithValidRequest()
            .WithAmount(amount)
            .Build();
        var error = _subject.Validate(request).First();
            
        Assert.That(error.ErrorCode, Is.EqualTo("InvalidAmount"));
        Assert.That(error.Message, Is.EqualTo("Amount is Invalid"));
    }
        
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("XXX")]
    [TestCase("123")]
    public void Given_Invalid_Currency_Then_Return_Error(string currency)
    {
        var request = new RequestBuilder()
            .WithValidRequest()
            .WithCurrency(currency)
            .Build();
        var error = _subject.Validate(request).First();
            
        Assert.That(error.ErrorCode, Is.EqualTo("InvalidCurrency"));
        Assert.That(error.Message, Is.EqualTo("Currency is Invalid"));
    }
        
    [Test]
    public void Given_Invalid_Card_Then_Return_Error()
    {
        var request = new RequestBuilder()
            .WithValidRequest()
            .WithCard(null)
            .Build();
        var error = _subject.Validate(request).First();
            
        Assert.That(error.ErrorCode, Is.EqualTo("InvalidCard"));
        Assert.That(error.Message, Is.EqualTo("Card Details are Invalid"));
    }
}