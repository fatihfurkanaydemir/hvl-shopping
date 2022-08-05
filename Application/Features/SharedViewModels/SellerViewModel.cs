﻿namespace Application.Features.SharedViewModels
{
  public class SellerViewModel
  {
    public int Id { get; set; }
    public string IdentityId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string ShopName { get; set; }
    public AddressViewModel Address { get; set; }
    //public List<Product> Products { get; set; }
    //public List<Order> Orders { get; set; }
    //public List<Address> Addresses { get; set; }
  }
}
