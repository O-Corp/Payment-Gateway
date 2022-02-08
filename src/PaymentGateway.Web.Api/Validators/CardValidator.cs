using System.Globalization;
using System.Text.RegularExpressions;
using PaymentGateway.Web.Api.Models;

namespace PaymentGateway.Web.Api.Validators;

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