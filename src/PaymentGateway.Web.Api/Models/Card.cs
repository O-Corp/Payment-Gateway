namespace PaymentGateway.Web.Api.Models;

public class Card
{
    public string CardHolderName { get; set; }

    public string CardNumber { get; set; }

    public string Cvv { get; set; }

    public string ExpiryMonth { get; set; }

    public string ExpiryYear { get; set; }
}