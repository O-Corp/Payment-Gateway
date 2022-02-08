using System;
using System.Collections.Generic;
using System.Globalization;
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
        
        if (!IsCardNumberValid(card))
        {
            errors.Add(ApiError.Create("InvalidCardNumber", "Invalid Card Number"));
        }
        
        errors.AddRange(ValidateCardDate(card));
        
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
    
    private static bool IsCardNumberValid(Card card)
    {
        if (string.IsNullOrWhiteSpace(card.CardNumber))
        {
            return false;
        }

        const string pattern = @"^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$";
        var regex = new Regex(pattern);
        return regex.IsMatch(card.CardNumber);
    }
    
    private static IEnumerable<ApiError> ValidateCardDate(Card card)
    {
        if (!IsDatePartValid(card.ExpiryYear, out var expiryYear))
        {
            yield return ApiError.Create("InvalidExpiryYear", "Invalid Expiry Year");
            yield break;
        }

        if (!IsDatePartValid(card.ExpiryMonth, out var expiryMonth) || expiryMonth > 12)
        {
            yield return ApiError.Create("InvalidExpiryMonth", "Invalid Expiry Month");
            yield break;
        }

        if (HasCardExpired(expiryMonth, expiryYear))
        {
            yield return ApiError.Create("InvalidExpiryDate", "Card Has Expired");
        }
    }

    private static bool HasCardExpired(int expiryMonth, int expiryYear)
    {
        var dateTime = DateTime.UtcNow;
        var now = new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        var expiryDate = new DateTime(
            CultureInfo.CurrentCulture.Calendar.ToFourDigitYear(expiryYear),
            expiryMonth,
            DateTime.DaysInMonth(expiryYear, expiryMonth));

        return now > expiryDate;
    }

    private static bool IsDatePartValid(string datePart, out int parsedDatePart)
    {
        parsedDatePart = 0;

        if (string.IsNullOrWhiteSpace(datePart))
        {
            return false;
        }

        return int.TryParse(datePart, out parsedDatePart);
    }
}