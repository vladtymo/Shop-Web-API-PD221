using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using DataAccess.Data;
using DataAccess.Data.Entities;
using DataAccess.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    internal class ProductsService : IProductsService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Product> productsRepo;
        private readonly IRepository<Category> categoriesRepo;
        private readonly IValidator<CreateProductModel> validator;

        //private readonly ShopDbContext context;

        public ProductsService(IMapper mapper,
                                IRepository<Product> productsRepo,
                                IRepository<Category> categoriesRepo/*ShopDbContext context*/,
                                IValidator<CreateProductModel> validator)
        {
            this.mapper = mapper;
            this.productsRepo = productsRepo;
            this.categoriesRepo = categoriesRepo;
            this.validator = validator;
        }

        public void Create(CreateProductModel model)
        {
            //if (validator.Validate(model).) throw new HttpException(HttpStatusCode.BadRequest, validator.);
            validator.ValidateAndThrow(model);

            // convert model to entity type
            // 1 - manually
            //var entity = new Product()
            //{
            //    Name = model.Name,
            //    Description = model.Description,
            //    CategoryId = model.CategoryId,
            //    Discount = model.Discount,
            //    ImageUrl = model.ImageUrl,
            //    InStock = model.InStock,
            //    Price = model.Price
            //};
            // 2 - using AutoMapper
            var entity = mapper.Map<Product>(model);

            // create product in the db
            productsRepo.Insert(entity);
            productsRepo.Save();
        }

        public void Delete(int id)
        {
            if (id < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

            // delete by id
            var product = productsRepo.GetByID(id);
            if (product == null) throw new HttpException("Product not found.", HttpStatusCode.NotFound);

            productsRepo.Delete(product);
            productsRepo.Save();
        }

        public void Edit(EditProductModel model)
        {
            var entity = mapper.Map<Product>(model);

            // update product in the db
            productsRepo.Update(entity);
            productsRepo.Save();
        }

        public ProductDto? Get(int id)
        {
            if (id < 0) throw new HttpException("Id can not be negative.", HttpStatusCode.BadRequest);

            // with JOIN operators
            //var product = context.Products.Include(x => x.Category).FirstOrDefault(i => i.Id == id);
            // without JOIN operators
            var product = productsRepo.GetByID(id);
            if (product == null) throw new HttpException("Product not found.", HttpStatusCode.NotFound);

            // TODO: add include properties

            // load product related entity
            //productsRepo.Entry(product).Reference(x => x.Category).Load();

            return mapper.Map<ProductDto>(product);
        }

        public IEnumerable<ProductDto> Get(IEnumerable<int> ids)
        {
            //return mapper.Map<List<ProductDto>>(context.Products
            //    .Include(x => x.Category)
            //    .Where(x => ids.Contains(x.Id))
            //    .ToList());

            return mapper.Map<List<ProductDto>>(productsRepo.Get(x => ids.Contains(x.Id), includeProperties: "Category"));
        }

        public IEnumerable<ProductDto> GetAll()
        {
            return mapper.Map<List<ProductDto>>(productsRepo.Get(includeProperties: "Category"));
        }

        public IEnumerable<CategoryDto> GetAllCategories()
        {
            return mapper.Map<List<CategoryDto>>(categoriesRepo.Get());
        }

        public int GetCount()
        {
            return productsRepo.Get().Count();
        }
    }
}
