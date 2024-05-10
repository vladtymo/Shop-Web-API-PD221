using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.Specifications;
using FluentValidation;
using System.Net;

namespace Core.Services
{
    internal class ProductsService : IProductsService
    {
        private readonly IMapper mapper;
        private readonly IRepository<Product> productsRepo;
        private readonly IRepository<Category> categoriesRepo;
        private readonly IValidator<CreateProductModel> validator;
        private readonly IFileService fileService;

        //private readonly ShopDbContext context;

        public ProductsService(IMapper mapper,
                                IRepository<Product> productsRepo,
                                IRepository<Category> categoriesRepo/*ShopDbContext context*/,
                                IValidator<CreateProductModel> validator,
                                IFileService fileService)
        {
            this.mapper = mapper;
            this.productsRepo = productsRepo;
            this.categoriesRepo = categoriesRepo;
            this.validator = validator;
            this.fileService = fileService;
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
            if (id < 0) throw new HttpException(Errors.IdCanNotBeNegative, HttpStatusCode.BadRequest);

            // delete by id
            var product = productsRepo.GetById(id);
            if (product == null) throw new HttpException(Errors.ProductNotFound, HttpStatusCode.NotFound);

            fileService.DeleteProductImage(product.ImageUrl);

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

        public async Task<ProductDto?> Get(int id)
        {
            if (id < 0) throw new HttpException(Errors.IdCanNotBeNegative, HttpStatusCode.BadRequest);

            // with JOIN operators
            //var product = context.Products.Include(x => x.Category).FirstOrDefault(i => i.Id == id);
            // without JOIN operators
            var product = await productsRepo.GetItemBySpec(new ProductSpecs.ById(id));
            if (product == null) throw new HttpException(Errors.ProductNotFound, HttpStatusCode.NotFound);

            // TODO: add include properties

            // load product related entity
            //productsRepo.Entry(product).Reference(x => x.Category).Load();

            return mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> Get(IEnumerable<int> ids)
        {
            //return mapper.Map<List<ProductDto>>(context.Products
            //    .Include(x => x.Category)
            //    .Where(x => ids.Contains(x.Id))
            //    .ToList());

            return mapper.Map<List<ProductDto>>(await productsRepo.GetListBySpec(new ProductSpecs.ByIds(ids))); // productsRepo.Get(x => ids.Contains(x.Id), includeProperties: "Category")
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            return mapper.Map<List<ProductDto>>(await productsRepo.GetListBySpec(new ProductSpecs.All()));
        }

        public IEnumerable<CategoryDto> GetAllCategories()
        {
            return mapper.Map<List<CategoryDto>>(categoriesRepo.GetAll());
        }

        public int GetCount()
        {
            return productsRepo.GetAll().Count();
        }
    }
}
