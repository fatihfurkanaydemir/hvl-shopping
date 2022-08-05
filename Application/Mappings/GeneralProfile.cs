// Categories
using Application.Features.Categories.Commands.CreateCategory;
using Application.Features.Categories.Commands.UpdateCategory;
using Application.Features.Categories.Queries.GetAllCategories;
using Application.Features.Categories.Queries.GetCategoryById;
using Application.Features.Categories.Queries.GetCategoryProductsById;

// Products
using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Commands.UpdateProduct;
using Application.Features.Products.Queries.GetAllProducts;
using Application.Features.Products.Queries.GetProductById;

// Customers
using Application.Features.Customers.Commands.CreateCustomer;
using Application.Features.Customers.Queries.GetAllCustomers;

// Sellers
using Application.Features.Sellers.Commands.CreateSeller;
using Application.Features.Sellers.Queries.GetAllSellers;
using Application.Features.Sellers.Queries.GetSellerProductsByIdentityId;

using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Application.Features.SharedViewModels;

namespace Application.Mappings
{
  public class GeneralProfile: Profile
  {
    public GeneralProfile()
    {
      CreateMap<Product, CreateProductCommand>().ReverseMap();
      CreateMap<Product, UpdateProductCommand>().ReverseMap();
      CreateMap<Product, GetAllProductsViewModel>();
      CreateMap<Product, GetProductByIdViewModel>();
      CreateMap<Product, GetCategoryProductsByIdProductViewModel>();
      CreateMap<GetAllProductsQuery, GetAllProductsParameter>();


      CreateMap<Image, GetAllProductsImageViewModel>();
      // Temporary usage of DTO
      CreateMap<Image, Features.Products.Commands.CreateProduct.ImageDTO>().ReverseMap();
      CreateMap<Image, Features.Products.Commands.UpdateProduct.ImageDTO>().ReverseMap();
      CreateMap<Image, GetProductByIdImageViewModel>();
      CreateMap<Image, GetCategoryProductsByIdImageViewModel>();

      CreateMap<Category, CreateCategoryCommand>().ReverseMap();
      CreateMap<Category, UpdateCategoryCommand>().ReverseMap();
      CreateMap<Category, GetCategoryByIdViewModel>();
      CreateMap<Category, GetAllCategoriesViewModel>();
      CreateMap<Category, GetProductByIdCategoryViewModel>();
      CreateMap<Category, GetAllProductsCategoryViewModel>();
      CreateMap<Category, GetCategoryProductsByIdViewModel>();
      CreateMap<GetAllCategoriesQuery, GetAllCategoriesParameter>();
      CreateMap<GetCategoryProductsByIdQuery, GetCategoryProductsByIdParameter>();

      CreateMap<Customer, CreateCustomerCommand>().ReverseMap();
      CreateMap<Customer, GetAllCustomersViewModel>();
      CreateMap<GetAllCustomersQuery, GetAllCustomersParameter>();

      CreateMap<Seller, CreateSellerCommand>().ReverseMap();
      CreateMap<Seller, GetAllSellersViewModel>();
      CreateMap<Seller, GetSellerProductsByIdentityIdViewModel>();
      CreateMap<GetAllSellersQuery, GetAllSellersParameter>();
      CreateMap<GetSellerProductsByIdentityIdQuery, GetSellerProductsByIdentityIdParameter>();

      CreateMap<Address, AddressDTO>().ReverseMap();
      CreateMap<Address, AddressViewModel>().ReverseMap();
      CreateMap<Product, ProductViewModel>().ReverseMap();
      CreateMap<Category, CategoryViewModel>().ReverseMap();
      CreateMap<Image, ImageViewModel>().ReverseMap();
      CreateMap<Seller, SellerViewModel>().ReverseMap();
    }
  }
}
