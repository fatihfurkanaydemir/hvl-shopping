using Application.Features.Sellers.Commands.CreateSeller;
using Application.Features.Sellers.Commands.UpdateSeller;
using Application.Features.Sellers.Queries.GetAllSellers;
using Application.Features.Sellers.Queries.GetSellerProductsByIdentityId;
using Application.Features.Sellers.Queries.GetSellerByIdentityId;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Common.EventBus.Interfaces;
using Common.ApplicationEvents;
using Stripe;
using Stripe.Checkout;

namespace WebApi.Controllers.v1
{
  public class StripeWebhookController : BaseApiController
  {
    private readonly IEventBus _eventBus;
    private readonly IConfiguration _configuration;
    public StripeWebhookController(IEventBus eventBus, IConfiguration configuration)
    {
      _eventBus = eventBus;
      _configuration = new ConfigurationBuilder()
                      .AddJsonFile("appsettings.json")
                      .Build(); ;
    }
    // POST api/<controller>
    [HttpPost]
    public async Task<IActionResult> StripeWebhook()
    {
      var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
      try
      {
        var stripeEvent = EventUtility.ConstructEvent(json,
            Request.Headers["Stripe-Signature"], _configuration.GetSection("Stripe")["WebhookSecret"]);

        // Handle the event
        if (stripeEvent.Type == Events.CheckoutSessionCompleted)
        {
          var session = stripeEvent.Data.Object as Session;
          _eventBus.Publish(new OrderPaymentCompletedEvent
          {
            CheckoutSessionId = session.Id,
            PaymentIntentId = session.PaymentIntentId,
          });
        }
        else
        {
          Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
        }

        return Ok();
      }
      catch (StripeException e)
      {
        return BadRequest();
      }
    }
  }
}
