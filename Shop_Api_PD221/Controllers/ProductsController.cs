using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.ADULT)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await productsService.GetAll());
        }

        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            return Ok(productsService.GetAllCategories());
        }

        //[Authorize] // based on cookies
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] // based on JWT

        [HttpGet("{id:int}")] // view/1
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await productsService.Get(id));
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Policies.PREMIUM_CLIENT)]
        public IActionResult Create([FromForm] CreateProductModel model)
        {
            productsService.Create(model);
            return Ok();
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.Admin)]
        public IActionResult Edit([FromBody] EditProductModel model)
        {
            productsService.Edit(model);
            return Ok();
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Roles.Admin)]
        [HttpDelete("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            productsService.Delete(id);
            return Ok();
        }
    }
}
