using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        
        if (!IsNameOnCardValid(card))
        {
            errors.Add(ApiError.Create("InvalidCardHolderName", "Invalid Cardholder Name"));
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

    private static bool IsNameOnCardValid(Card card)
    {
        if (string.IsNullOrWhiteSpace(card.CardHolderName))
        {
            return false;
        }

        const string pattern = "[A-Za-z]";
        var regex = new Regex(pattern);
        return regex.IsMatch(card.CardHolderName);
    }
}