using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Commands.UpdateProduct;
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
      CreateMap<Product, UpdateProductCommand>().ReverseMap();
      CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
      CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
      CreateMap<Image, GetAllProductsImageViewModel>();
      // Temporary usage of DTO
      CreateMap<Image, Features.Products.Commands.CreateProduct.ImageDTO>().ReverseMap();
      CreateMap<Image, Features.Products.Commands.UpdateProduct.ImageDTO>().ReverseMap();
      CreateMap<Product, GetProductByIdViewModel>().ReverseMap();
      CreateMap<Image, GetProductByIdImageViewModel>();

      // Temporary usage of DTO
      CreateMap<Category, Features.Products.Commands.CreateProduct.CategoryDTO>().ReverseMap();
      CreateMap<Category, Features.Products.Commands.UpdateProduct.CategoryDTO>().ReverseMap();
    }
  }
}
