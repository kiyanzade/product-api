using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
         Summary = "Get all products or filter by ownerId",
         Description = "If you provide the 'owner' parameter, only products created by that owner will be returned.\nex: api/Products?owenr=510a8a45-1b88-4012-94a6-a21c094ed2f9"
            )]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetProducts(string? owner=null)
        {
            var productList = await _productService.GetProducts(owner);
            return Ok(productList);

        }

        // GET: api/Products/5
        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Get product by productId",
            Description = "ex: api/Products/5"
        )]
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
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
            }
         
        }

        // POST: api/Products
        [HttpPost]
        [SwaggerOperation(
            Summary = "Add A product"
        )]
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
        [SwaggerOperation(
            Summary = "Edit product by productId"
        )]
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
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
            }
           
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Deleted product by productId"
        )]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return Ok("Product deleted successfully.");
            }
            catch (IOException ex)
            {
                if (ex is FileNotFoundException)
                {
                    return NotFound(ex.Message);
                }
                return BadRequest(ex.Message);
            }
            
        }

    }
}
