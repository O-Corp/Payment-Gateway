using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PaymentGateway.Testing.Shared;
using PaymentGateway.Web.Api.Models;

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

}

public class CardValidator
{
    public List<ApiError> Validate(Card card)
    {
        var errors = new List<ApiError>();

        if (!IsCvvNumberValid(card))
        {
            errors.Add(ApiError.Create("InvalidCvv", "Invalid CVV"));
        }
        
        return errors;
    }
    
    private static bool IsCvvNumberValid(Card card)
    {
        if (string.IsNullOrWhiteSpace(card.Cvv))
        {
            return false;
        }

        return int.TryParse(card.Cvv, out _);
    }

}