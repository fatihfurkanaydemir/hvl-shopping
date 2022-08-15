using Application.Wrappers;
using Domain.Common;
using Newtonsoft.Json;
using System.Text;
using Domain.Entities;
using Stripe.Checkout;

namespace Application.Services;

public class PaymentService
{
  public async Task<string> CreateCheckoutSession(CustomerBasket basket)
  {
    var items = new List<SessionLineItemOptions>();
    foreach(var item in basket.Items)
    {
      items.Add(new SessionLineItemOptions
      {
        PriceData = new SessionLineItemPriceDataOptions
        {
          UnitAmount = (long)item.Price,
          Currency = "usd",
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

    var options = new SessionCreateOptions
    {
      LineItems = items,
      Mode = "payment",
      SuccessUrl = "http://localhost/payment-success",
      CancelUrl = "http://localhost/payment-cancel",
    };

    var service = new SessionService();
    Session session = service.Create(options);

    return session.Url;
  }
}
