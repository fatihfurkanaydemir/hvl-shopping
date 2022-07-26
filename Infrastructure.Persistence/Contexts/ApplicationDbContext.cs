﻿using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts
{
  public class ApplicationDbContext: DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
      ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<Address> Addresses { get; set; }
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
}
