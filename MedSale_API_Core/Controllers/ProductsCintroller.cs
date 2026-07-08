using MedSale_API_Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace MedSale_API_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private AuroraContext _context;

        public ProductsController(AuroraContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();

            if (products == null || !products.Any()) 
                return NotFound();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var products = await _context.Products.FindAsync(id);

            if (products == null)
                return NotFound();
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product prod)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.Products.AddAsync(prod);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = prod.Id }, prod);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditProduct(int id, Product prod)
        {
            if (id != prod.Id)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(prod).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductExistsAsync(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        private async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        private async Task<bool> ProductExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(e => e.Id == id);
        }
    }
}
