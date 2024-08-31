using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductProject.Database.Entities;
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

        // GET: api/Products?owenr=UserId
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
            try
            {
                var product = await _productService.GetProduct(id);
                return Ok(product);
            }
            catch(IOException ex)
            {
                if (ex is FileNotFoundException)
                {
                    return NotFound(ex);
                }

                return BadRequest(ex);
            }
         
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult> PostProduct([FromBody] AddProductDto product)
        {
            try
            {
               await _productService.AddProduct(product);
               return Ok("Product added successfully.");
            }
            catch (IOException ex)
            { 
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, EditProductDto product)
        {
            try
            {
                await _productService.EditProduct(id, product);
                return Ok();
            }
            catch (IOException ex)
            {
                if (ex is FileNotFoundException)
                {
                    return NotFound(ex);
                }
                return BadRequest(ex);
            }
           
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return Ok();
            }
            catch (IOException ex)
            {
                if (ex is FileNotFoundException)
                {
                    return NotFound(ex);
                }
                return BadRequest(ex);
            }
            
        }

    }
}
