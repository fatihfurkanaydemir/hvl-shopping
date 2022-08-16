using Application.Wrappers;
using Domain.Common;
using Newtonsoft.Json;
using System.Text;
using Domain.Entities;
using Stripe.Checkout;

namespace Application.Services;

public class PaymentService
{
  public async Task<Session> CreateCheckoutSession(CustomerBasket basket, decimal shipmentPrice)
  {
    var items = new List<SessionLineItemOptions>();
    foreach(var item in basket.Items)
    {
      items.Add(new SessionLineItemOptions
      {
        PriceData = new SessionLineItemPriceDataOptions
        {
          UnitAmountDecimal = item.Price * 100,
          Currency = "try",
          ProductData = new SessionLineItemPriceDataProductDataOptions
          {
            Name = item.ProductName,
            Images = new List<string> { item.PictureUrl },
            Description = item.CategoryName
          },
        },
        Quantity = item.Quantity,
      });
    }

    items.Add(new SessionLineItemOptions
    {
      PriceData = new SessionLineItemPriceDataOptions
      {
        UnitAmountDecimal = shipmentPrice * 100,
        Currency = "try",
        ProductData = new SessionLineItemPriceDataProductDataOptions
        {
          Name = "Kargo Ücreti",
          Description = "XXXXX kargo"
        },
      },
      Quantity = 1,
    });

    var options = new SessionCreateOptions
    {
      LineItems = items,
      Mode = "payment",
      SuccessUrl = "http://localhost:4200/payment-success?session_id={CHECKOUT_SESSION_ID}",
      CancelUrl = "http://localhost:4200/payment-cancel",
    };

    var service = new SessionService();
    Session session = service.Create(options);

    return session;
  }
}
