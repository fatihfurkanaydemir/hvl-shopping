using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;

namespace Application.Features.Customers.Queries.GetAllCustomers
{
  public class GetAllCustomersQuery : IRequest<PagedResponse<IEnumerable<GetAllCustomersViewModel>>>
  {
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
  }

  public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, PagedResponse<IEnumerable<GetAllCustomersViewModel>>>
  {
    private readonly ICustomerRepositoryAsync _customerRepository;
    private readonly IMapper _mapper;
    public GetAllCustomersQueryHandler(ICustomerRepositoryAsync customerRepository, IMapper mapper)
    {
      _customerRepository = customerRepository;
      _mapper = mapper;
    }

    public async Task<PagedResponse<IEnumerable<GetAllCustomersViewModel>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
      var validFilter = _mapper.Map<GetAllCustomersParameter>(request);
      var dataCount = await _customerRepository.GetDataCount();
      var customers = await _customerRepository.GetPagedReponseAsync(request.PageNumber, request.PageSize);

      var customerViewModels = new List<GetAllCustomersViewModel>();

      foreach (var c in customers)
      {
        var customer = _mapper.Map<GetAllCustomersViewModel>(c);

        customerViewModels.Add(customer);
      }

      return new PagedResponse<IEnumerable<GetAllCustomersViewModel>>(customerViewModels, validFilter.PageNumber, validFilter.PageSize, dataCount);
    }
  }
}
