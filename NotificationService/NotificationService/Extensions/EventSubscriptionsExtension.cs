﻿using Common.EventBus.Interfaces;
using Common.ApplicationEvents;
using Common.ApplicationRPCs;
using Common.Wrappers;
using Common.SharedViewModels;
using NotificationService.Application.Features.DiscountCouponNotifications;

namespace NotificationService.Extensions;

public static class EventSubscriptionsExtension
{
  public static void SubscribeEvents(this IApplicationBuilder app)
  {
    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

    eventBus.Subscribe<DiscountCouponCreatedEvent, DiscountCouponCreatedEventHandler>();
  }
}
