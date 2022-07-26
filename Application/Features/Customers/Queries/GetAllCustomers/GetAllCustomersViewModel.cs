﻿using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Customers.Queries.GetAllCustomers
{
  public class GetAllCustomersViewModel
  {
    public int Id { get; set; }
    public string IdentityId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public List<Address> Addresses { get; set; }
  }
}
