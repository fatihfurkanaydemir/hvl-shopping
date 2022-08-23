using Application.Wrappers;
using Domain.Common;
using Newtonsoft.Json;
using System.Text;
using Domain.Entities;
using Stripe.Checkout;
using Stripe;

namespace Application.Services;

public class PaymentService
{
  public async Task<Session> CreateCheckoutSession(CustomerBasket basket, decimal shipmentPrice, string? couponCode = null, decimal couponAmount = 0)
  {
    var items = new List<SessionLineItemOptions>();
    var discounts = new List<SessionDiscountOptions>();

    foreach (var item in basket.Items)
    {
      var images = new List<string> {};
      if(item.PictureUrl != null && item.PictureUrl.Trim() != "")
        images.Add(item.PictureUrl);
      else
        images.Add("https://www.pngitem.com/pimgs/m/27-272007_transparent-product-icon-png-product-vector-icon-png.png");
      
      items.Add(new SessionLineItemOptions
      {
        PriceData = new SessionLineItemPriceDataOptions
        {
          UnitAmountDecimal = item.Price * 100,
          Currency = "try",
          ProductData = new SessionLineItemPriceDataProductDataOptions
          {
            Name = item.ProductName,
            Images = images,
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

    if(couponCode != null)
    {
      var couponOptions = new CouponCreateOptions { AmountOff = (long)couponAmount * 100, Duration = "once", Currency = "try", Id = couponCode };
      var couponService = new CouponService();
      var coupon = couponService.Create(couponOptions);

      discounts.Add(new SessionDiscountOptions
      {
        Coupon = coupon.Id,
      });
    }
    

    var options = new SessionCreateOptions
    {
      LineItems = items,
      Discounts = discounts.Count == 0 ? null : discounts,
      Mode = "payment",
      SuccessUrl = "http://localhost:4200/payment-success?session_id={CHECKOUT_SESSION_ID}",
      CancelUrl = "http://localhost:4200/payment-cancel",
    };

    var service = new SessionService();
    Session session = service.Create(options);

    return session;
  }
}
