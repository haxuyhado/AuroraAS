using Microsoft.AspNetCore.Mvc;
using MedSale_API_Core.Models;
using Microsoft.EntityFrameworkCore;

namespace MedSale_API_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private AuroraContext _context;

        public EmployeesController(AuroraContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var emp = await _context.Employees.ToListAsync();

            if (emp == null || !emp.Any())
                return NotFound();
            return Ok(emp);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var emp = await _context.Employees.FindAsync(id);

            if (emp == null)
                return NotFound();
            return Ok(emp);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(Employee emp)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.Employees.AddAsync(emp);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = emp.Id }, emp);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditEmployee(int id, Employee emp)
        {
            if (id != emp.Id)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(emp).State = EntityState.Modified; 
            
            try 
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EmployeeExistAsync(id))
                    return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var emp = await _context.Employees.FindAsync(id);

            if (emp == null)
                return NotFound();

            _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();

            return Ok(emp);
        }

        private async Task<bool> EmployeeExistAsync(int id)
        {
            return await _context.Employees.AnyAsync(e => e.Id == id);
        }
    }
}
