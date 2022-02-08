using System;
using System.Linq;
using NUnit.Framework;
using PaymentGateway.Testing.Shared;
using PaymentGateway.Web.Api.Validators;

namespace PaymentGateway.Web.Api.UnitTests.Validators;

[TestFixture]
public class CardValidatorTests
{
    private CardValidator _subject;

    [SetUp]
    public void Setup()
    {
        _subject = new CardValidator();
    }
        
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    [TestCase("xXx")]
    public void Given_Invalid_CVV_Then_Errors_Are_Returned(string cvv)
    {
        var request = new RequestBuilder()
            .WithValidRequest()
            .WithCvv(cvv)
            .Build();
        var result = _subject.Validate(request.Card).Single();
            
        Assert.That(result.ErrorCode, Is.EqualTo("InvalidCvv"));
        Assert.That(result.Message, Is.EqualTo("Invalid CVV"));
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    [TestCase("123")]
    public void Given_Invalid_Cardholder_Name_Then_Errors_Are_Returned(string name)
    {
        var request = new RequestBuilder()
            .WithValidRequest()
            .WithCardHolderName(name)
            .Build();
        var result = _subject.Validate(request.Card).Single();
            
        Assert.That(result.ErrorCode, Is.EqualTo("InvalidCardHolderName"));
        Assert.That(result.Message, Is.EqualTo("Invalid Cardholder Name"));
    }
    
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    [TestCase("4444")]
    [TestCase("55554444333322221111")]
    [TestCase("abC")]
    public void Given_Invalid_Card_Number_Then_Errors_Are_Returned(string cardNumber)
    {
        var request = new RequestBuilder()
            .WithValidRequest()
            .WithCardNumber(cardNumber)
            .Build();
        var result = _subject.Validate(request.Card).Single();
            
        Assert.That(result.ErrorCode, Is.EqualTo("InvalidCardNumber"));
        Assert.That(result.Message, Is.EqualTo("Invalid Card Number"));
    }
    
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    [TestCase("15")]
    [TestCase("123")]
    [TestCase("ABC")]
    public void Given_Invalid_Expiry_Month_Then_Errors_Are_Returned(string expiryMonth)
    {
        var request = new RequestBuilder()
            .WithValidRequest()
            .WithExpiryMonth(expiryMonth)
            .Build();
        var result = _subject.Validate(request.Card).Single();
            
        Assert.That(result.ErrorCode, Is.EqualTo("InvalidExpiryMonth"));
        Assert.That(result.Message, Is.EqualTo("Invalid Expiry Month"));
    }
        
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    [TestCase("ABC")]
    public void Given_Invalid_Expiry_Year_Then_Errors_Are_Returned(string expiryYear)
    {
        var request = new RequestBuilder()
            .WithValidRequest()
            .WithExpiryYear(expiryYear)
            .Build();
        var result = _subject.Validate(request.Card).Single();
            
        Assert.That(result.ErrorCode, Is.EqualTo("InvalidExpiryYear"));
        Assert.That(result.Message, Is.EqualTo("Invalid Expiry Year"));
    }

    [Test]
    public void Given_Expiry_Date_In_The_Past_Then_Errors_Are_Returned()
    {
        var lastMonth = DateTime.Now.AddMonths(-1);
        var month = lastMonth.Month.ToString("00");
        var year = lastMonth.ToString("yy");
        var request = new RequestBuilder()
            .WithValidRequest()
            .WithExpiryMonth(month)
            .WithExpiryYear(year)
            .Build();
        var result = _subject.Validate(request.Card).Single();
            
        Assert.That(result.ErrorCode, Is.EqualTo("InvalidExpiryDate"));
        Assert.That(result.Message, Is.EqualTo("Card Has Expired"));
    }    
}