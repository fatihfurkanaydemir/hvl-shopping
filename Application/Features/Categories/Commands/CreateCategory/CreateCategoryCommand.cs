using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Categories.Commands.CreateCategory
{
  public class CreateCategoryCommand : IRequest<Response<int>>
  {
    public string Name { get; set; }
  }
  public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Response<int>>
  {
    private readonly ICategoryRepositoryAsync _categoryRepository;
    private readonly IMapper _mapper;
    public CreateCategoryCommandHandler(ICategoryRepositoryAsync categoryRepository, IMapper mapper)
    {
      _categoryRepository = categoryRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
      var category = _mapper.Map<Category>(request);

      await _categoryRepository.AddAsync(category);

      return new Response<int>(category.Id);
    }
  }
}
