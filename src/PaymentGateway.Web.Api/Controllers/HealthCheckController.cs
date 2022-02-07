using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.Web.Api.Controllers;

[Route("gateway/v1/healthcheck")]
public class HealthCheckController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return this.Ok();
    }
}