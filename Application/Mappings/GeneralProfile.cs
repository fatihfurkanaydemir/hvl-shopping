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
using Application.Features.Products.Queries.GetProductsBySearchFilter;

// Customers
using Application.Features.Customers.Commands.CreateCustomer;
using Application.Features.Customers.Queries.GetAllCustomers;
using Application.Features.Customers.Queries.GetCustomerByIdentityId;

// Sellers
using Application.Features.Sellers.Commands.CreateSeller;
using Application.Features.Sellers.Queries.GetAllSellers;
using Application.Features.Sellers.Queries.GetSellerProductsByIdentityId;
using Application.Features.Sellers.Queries.GetSellerByIdentityId;

using AutoMapper;
using Domain.Entities;
using Application.DTOs;
using Application.Features.SharedViewModels;
using Application.Features.Orders.Commands.CreateOrder;
using Common.ApplicationEvents;
using Common.Entities;

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
      CreateMap<GetProductsBySearchFilterQuery, GetProductsBySearchFilterParameter>();


      CreateMap<Image, GetAllProductsImageViewModel>();
      // Temporary usage of DTO
      CreateMap<Image, ImageDTO>().ReverseMap();
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
      CreateMap<Customer, GetCustomerByIdentityIdViewModel>();
      CreateMap<GetAllCustomersQuery, GetAllCustomersParameter>();

      CreateMap<Seller, CreateSellerCommand>().ReverseMap();
      CreateMap<Seller, GetAllSellersViewModel>();
      CreateMap<Seller, GetSellerProductsByIdentityIdViewModel>();
      CreateMap<Seller, GetSellerByIdentityIdViewModel>();
      CreateMap<GetAllSellersQuery, GetAllSellersParameter>();
      CreateMap<GetSellerProductsByIdentityIdQuery, GetSellerProductsByIdentityIdParameter>();

      CreateMap<Address, AddressDTO>().ReverseMap();
      CreateMap<Address, AddressViewModel>().ReverseMap();
      CreateMap<Product, ProductViewModel>().ReverseMap();
      CreateMap<Category, CategoryViewModel>().ReverseMap();
      CreateMap<Image, ImageViewModel>().ReverseMap();
      CreateMap<Seller, SellerViewModel>().ReverseMap();

      CreateMap<CreateOrderCommand, CreateOrderEvent>().ReverseMap();
      CreateMap<OrderProductDTO, OrderProduct>().ReverseMap();
    }
  }
}
