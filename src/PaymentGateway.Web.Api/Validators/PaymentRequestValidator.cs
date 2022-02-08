using System.Text.RegularExpressions;
using PaymentGateway.Web.Api.Models;

namespace PaymentGateway.Web.Api.Validators;

public class PaymentRequestValidator
{
    public List<ApiError> Validate(CardCaptureRequest request)
    {
        var errors = new List<ApiError>();
        if (!IsAmountValid(request.Amount))
        {
            errors.Add(ApiError.Create("InvalidAmount", "Amount is Invalid"));
        }
        
        if (!IsCurrencyValid(request.Currency))
        {
            errors.Add(ApiError.Create("InvalidCurrency", "Currency is Invalid"));
        }
        
        if (request?.Card == null)
        {
            errors.Add(ApiError.Create("InvalidCard", "Card Details are Invalid"));
        }

        return errors;
    }
    
    private static bool IsCurrencyValid(string currency)
    {
        var currencyCodeRegex =
            new Regex(
                @"/^|AED|AUD|CAD|CDF|EUR|GBP|HKD|$/"); // Add more currency codes that we support as a business

        if (string.IsNullOrWhiteSpace(currency))
        {
            return false;
        }

        if (!currencyCodeRegex.IsMatch(currency.ToUpper()))
        {
            return false;
        }

        return true;
    }
    
    private static bool IsAmountValid(decimal amount)
    {
        const int minimumPaymentAmount = 1;
        const int maximumPaymentAmount = 500;

        return amount is >= minimumPaymentAmount and <= maximumPaymentAmount;
    }
}