using Microsoft.AspNetCore.Mvc;
using Common.EventBus.Interfaces;
using Common.ApplicationEvents;
using Common.ApplicationRPCs;
using Stripe;
using Stripe.Checkout;
using Application.Features.Orders.Commands.CreateOrder;

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
        else if (stripeEvent.Type == Events.CheckoutSessionExpired)
        {
          var session = stripeEvent.Data.Object as Session;
          // Get products with rpc and restock them, then delete the order
          var orders = (await _eventBus.CallRP(new GetOrdersByCheckoutSessionIdRPC
          {
            CheckoutSessionId = session.Id,
          })).Data;

          foreach (var order in orders)
          {
            await Mediator.Send(new CancelOrderCommand { Products = order.Products, OrderId = order.Id });
          }
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
