using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Models;
using System.Net;
using System.Net.Sockets;

namespace Core.Mapping
{
    public class ProductProfile : Profile
    {

        public ProductProfile(IFileService fileService, string host)
        {
            CreateMap<Product, ProductDto>()
                .ForMember(x => x.ImageUrl, opts => opts.MapFrom(p => ConvertToAbsolutePath(p.ImageUrl, host)));
            CreateMap<ProductDto, Product>()
                .ForMember(x => x.Category, opts => opts.Ignore());

            CreateMap<CreateProductModel, Product>()
                .ForMember(x => x.ImageUrl, opts => opts.MapFrom(p => fileService.SaveProductImage(p.Image).Result));

            CreateMap<EditProductModel, Product>().ReverseMap();
            CreateMap<EditProductModel, ProductDto>().ReverseMap();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
        }

        private string ConvertToAbsolutePath(string path, string host)
        {
            return path.Contains("://") ? path : Path.Combine(host + path);
        }
    }
}
