﻿using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Models;

namespace Core.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile(IFileService fileService)
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>()
                .ForMember(x => x.Category, opts => opts.Ignore());

            CreateMap<CreateProductModel, Product>()
                .ForMember(x => x.ImageUrl, opts => opts.MapFrom(p => fileService.SaveProductImage(p.Image).Result));

            CreateMap<EditProductModel, Product>().ReverseMap();
            CreateMap<EditProductModel, ProductDto>().ReverseMap();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
        }
    }
}
