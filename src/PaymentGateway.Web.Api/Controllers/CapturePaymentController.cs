using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Web.Api.Models;
using PaymentGateway.Web.Api.Validators;

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