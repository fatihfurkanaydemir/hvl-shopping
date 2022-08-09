using Application.Features.SharedViewModels;
using Domain.Entities;
using Domain.Enums;

namespace Application.Features.Customers.Queries.GetCustomerByIdentityId
{
  public class GetCustomerByIdentityIdViewModel
  {
    public int Id { get; set; }
    public string IdentityId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public List<AddressViewModel> Addresses { get; set; }
  }
}
