using Microsoft.AspNetCore.Mvc;
using MedSale_API_Core.Models;
using Microsoft.EntityFrameworkCore;

namespace MedSale_API_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly AuroraContext _context;
        
        public PositionsController(AuroraContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Position>>> GetPositions()
        {
            var positions = await _context.Positions.ToListAsync();
            return Ok(positions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Position>> GetPosition(int id)
        {
            var position = await _context.Positions.FindAsync(id);

            if (position == null)
                return NotFound();
            return Ok(position);
        }
    }
}
