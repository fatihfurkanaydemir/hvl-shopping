// Categories
using Application.Features.Categories.Commands.CreateCategory;
using Application.Features.Categories.Commands.UpdateCategory;
using Application.Features.Categories.Queries.GetAllCategories;
using Application.Features.Categories.Queries.GetCategoryById;

// Products
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
      CreateMap<Product, GetAllProductsViewModel>();
      CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
      CreateMap<Image, GetAllProductsImageViewModel>();
      // Temporary usage of DTO
      CreateMap<Image, Features.Products.Commands.CreateProduct.ImageDTO>().ReverseMap();
      CreateMap<Image, Features.Products.Commands.UpdateProduct.ImageDTO>().ReverseMap();
      CreateMap<Product, GetProductByIdViewModel>();
      CreateMap<Image, GetProductByIdImageViewModel>();

      // Temporary usage of DTO
      CreateMap<Category, CreateCategoryCommand>().ReverseMap();
      CreateMap<Category, UpdateCategoryCommand>().ReverseMap();
      CreateMap<Category, GetCategoryByIdViewModel>();
      CreateMap<Category, GetAllCategoriesViewModel>();
      CreateMap<Category, GetProductByIdCategoryViewModel>();
      CreateMap<GetAllCategoriesQuery, GetAllCategoriesParameter>();
      CreateMap<Category, GetAllProductsCategoryViewModel>();
    }
  }
}
