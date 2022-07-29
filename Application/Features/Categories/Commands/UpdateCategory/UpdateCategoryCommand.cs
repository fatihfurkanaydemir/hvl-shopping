using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Categories.Commands.UpdateCategory
{
  public class UpdateCategoryCommand : IRequest<Response<int>>
  {
    public int Id { get; set; }
    public string Name { get; set; }
  }
  public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Response<int>>
  {
    private readonly ICategoryRepositoryAsync _categoryRepository;
    private readonly IMapper _mapper;
    public UpdateCategoryCommandHandler(ICategoryRepositoryAsync categoryRepository, IMapper mapper)
    {
      _categoryRepository = categoryRepository;
      _mapper = mapper;
    }

    public async Task<Response<int>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
      var category = await _categoryRepository.GetByIdAsync(request.Id);
      if (category == null) throw new ApiException("Category not found");

      var requestCategory = _mapper.Map<Category>(request);

      category.Name = requestCategory.Name;

      await _categoryRepository.UpdateAsync(category);

      return new Response<int>(category.Id);
    }
  }
}
