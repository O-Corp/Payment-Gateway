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
        
        if (request?.Card == null)
        {
            errors.Add(ApiError.Create("InvalidCard", "Card Details are Invalid"));
        }

        return errors;
    }
    
    private static bool IsAmountValid(decimal amount)
    {
        const int minimumPaymentAmount = 1;
        const int maximumPaymentAmount = 500;

        return amount is >= minimumPaymentAmount and <= maximumPaymentAmount;
    }
}