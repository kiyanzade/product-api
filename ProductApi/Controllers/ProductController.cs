using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Data;
using ProductAPI.Models;
using ProductProject.Service.ProductService;
using ProductProject.Service.ProductService.Dto;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/Products?owenr=username
        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(
         Summary = "Get all products or filter by owner",
         Description = "If you provide the 'owner' parameter, only products created by that owner will be returned."
            )]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProducts(string? owner=null)
        {
            var productList = await _productService.GetProducts(owner);
            return Ok(productList);

        }

        // GET: api/Products/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductModel>> GetProduct(int id)
        {
            var product = await _productService.GetProduct(id);
            return Ok(product);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<ProductModel>> PostProduct(AddProductDto product)
        {
            // try
            // {
            //     product.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User is not authorized.");
            //     _productService.AddProduct(product)
            // }
            // catch (Exception ex)
            // { 
            //     if(ex is DbUpdateException) // TODO: fix
            //     {
            //         return Conflict("A product with the same ManufactureEmail and ProduceDate already exists.");
            //     }
            //     else
            //     {
            //        return BadRequest(ex);
            //     }
            // }
            throw new NotImplementedException();

        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, EditProductDto product)
        {
            
            await  _productService.EditProduct(id, product);
            return Ok();
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProduct(id);

            return Ok();
        }

    }
}
