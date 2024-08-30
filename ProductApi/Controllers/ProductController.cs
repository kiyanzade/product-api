using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductAPI.Data;
using ProductAPI.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Security.Claims;

namespace ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext dbContext)
        {
            _context = dbContext;
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
            IEnumerable<ProductModel> productList;
            if (string.IsNullOrEmpty(owner))
            {
                productList = await _context.Products.ToListAsync();
            }
            else
            {
                productList = await _context.Products
                .Where(p => p.OwnerId == owner)
                .ToListAsync();
            }
            return Ok(productList);

        
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<ProductModel>> PostProduct(ProductModel product)
        {
            try
            {
                product.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User is not authorized.");
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }
            catch (Exception ex)
            { 
                if(ex is DbUpdateException) // TODO: fix
                {
                    return Conflict("A product with the same ManufactureEmail and ProduceDate already exists.");
                }
                else
                {
                   return BadRequest(ex);
                }
            }
         
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductModel product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            if (existingProduct.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existingProduct = await _context.Products.FindAsync(id);
           
            if (existingProduct == null)
            {
                return NotFound();
            }

            if (existingProduct.OwnerId != User.FindFirstValue(ClaimTypes.NameIdentifier)) 
            {
                return Forbid();
            }

            _context.Products.Remove(existingProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
