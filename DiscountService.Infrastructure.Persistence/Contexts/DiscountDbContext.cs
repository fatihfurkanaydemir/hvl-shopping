﻿using Microsoft.EntityFrameworkCore;
using DiscountService.Domain.Entities;
using Common.Entities;

namespace DiscountService.Infrastructure.Persistence.Contexts;

public class DiscountDbContext: DbContext
{
  public DiscountDbContext(DbContextOptions<DiscountDbContext> options) : base(options)
  {
    ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
  }

  public DbSet<Coupon> Coupons { get; set; }
  public DbSet<UsedCoupon> UsedCoupons { get; set; }

  public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
  {
    foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
    {
      switch (entry.State)
      {
        case EntityState.Added:
          entry.Entity.Created = DateTime.UtcNow;
          break;
        case EntityState.Modified:
          entry.Entity.LastModified = DateTime.UtcNow;
          break;
      }
    }
    return base.SaveChangesAsync(cancellationToken);
  }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    //All Decimals will have 18,6 Range
    foreach (var property in builder.Model.GetEntityTypes()
      .SelectMany(t => t.GetProperties())
      .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
      {
        property.SetColumnType("decimal(18,6)");
      }

    base.OnModelCreating(builder);
  }
}
