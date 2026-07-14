using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedSale_API_Core.Models;

namespace MedSale_API_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private AuroraContext _context;

        public ClientsController(AuroraContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            var clients = await _context.Clients.ToListAsync();

            if (clients == null || !clients.Any())
                return NotFound();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
                return NotFound();
            return Ok(client);
        }

        [HttpPost]
        public async Task<ActionResult<Client>> AddClient(Client client)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditClient(Client client, int id)
        {
            if (id != client.Id)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ClientExistsAsync(id))
                    return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
                return NotFound();

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return Ok(client);
        }

        private async Task<bool> ClientExistsAsync(int id)
        {
            return await _context.Clients.AnyAsync(c => c.Id == id);
        }
    }
}
