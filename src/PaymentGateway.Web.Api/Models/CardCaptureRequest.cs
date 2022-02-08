namespace PaymentGateway.Web.Api.Models;

public class CardCaptureRequest
{
    public string PaymentReference { get; set; }
    
    public decimal Amount { get; set; }

    public string Currency { get; set; }

    public Card Card { get; set; }

    public string MerchantId { get; set; }
}