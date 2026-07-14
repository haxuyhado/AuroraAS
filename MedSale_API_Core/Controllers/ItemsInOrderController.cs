using MedSale_API_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedSale_API_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsInOrderController : ControllerBase
    {
        private readonly AuroraContext _context;

        public ItemsInOrderController(AuroraContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemsInOrder>>> GetItemsInOrder()
        {
            var items = await _context.ItemsInOrders.ToListAsync();
            if (items == null || !items.Any())
                return NotFound();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemsInOrder>> GetItemInOrder(int id)
        {
            var item = await _context.ItemsInOrders.FindAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ItemsInOrder>> PostItemInOrder(ItemsInOrder item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.ItemsInOrders.AddAsync(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemInOrder), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutItemInOrder(int id, ItemsInOrder item)
        {
            if (id != item.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ItemInOrderExistsAsync(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemsInOrder>> DeleteItemInOrder(int id)
        {
            var item = await _context.ItemsInOrders.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.ItemsInOrders.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }

        private async Task<bool> ItemInOrderExistsAsync(int id)
        {
            return await _context.ItemsInOrders.AnyAsync(e => e.Id == id);
        }
    }
}