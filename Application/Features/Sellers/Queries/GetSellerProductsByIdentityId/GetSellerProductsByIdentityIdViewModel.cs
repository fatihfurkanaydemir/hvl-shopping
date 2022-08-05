﻿using Application.Features.SharedViewModels;

namespace Application.Features.Sellers.Queries.GetSellerProductsByIdentityId
{
  public class GetSellerProductsByIdentityIdViewModel
  {
    public int Id { get; set; }
    public string IdentityId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string ShopName { get; set; }
    public AddressViewModel Address { get; set; }
    public List<ProductViewModel> Products { get; set; }
  }
}
