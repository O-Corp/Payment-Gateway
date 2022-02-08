using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Web.Api.Models;

namespace PaymentGateway.Web.Api.Controllers;

[Route("gateway/v1/payments")]
[ApiController]
public class CapturePaymentController : ControllerBase
{
    private readonly PaymentRequestValidator _paymentRequestValidator;

    public CapturePaymentController()
    {
        _paymentRequestValidator = new PaymentRequestValidator();
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Put(
        [FromRoute] string id,
        [FromBody] CardCaptureRequest request)
    {
        request.PaymentReference = id;
            
        var errors = _paymentRequestValidator.Validate(request);
        if (errors.Any())
        {
            return this.BadRequest(errors);
        }

        return this.Ok();
    }
}

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