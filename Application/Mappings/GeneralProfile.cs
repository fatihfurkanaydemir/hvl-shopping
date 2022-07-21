using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Queries.GetAllProducts;
using Application.Features.Products.Queries.GetProductById;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
  public class GeneralProfile: Profile
  {
    public GeneralProfile()
    {
      CreateMap<Product, CreateProductCommand>().ReverseMap();
      CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
      CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
      CreateMap<Image, GetAllProductsImageViewModel>();
      CreateMap<Image, ImageDTO>().ReverseMap();
      CreateMap<Product, GetProductByIdViewModel>().ReverseMap();
      CreateMap<Image, GetProductByIdImageViewModel>();

      CreateMap<Category, CategoryDTO>().ReverseMap();
    }
  }
}
