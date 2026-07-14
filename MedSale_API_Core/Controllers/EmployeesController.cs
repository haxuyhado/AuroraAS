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

        [HttpGet("login/{id}/{password}")]
        public async Task<ActionResult<object>> AcceptLogin(int id, string password)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound(new { success = false, message = "Сотрудник не найден" });
            if (Cryptor.Encrypt(password) != employee.MyPassword)
                return BadRequest(new { success = false, message = "Неверный пароль" });

            return Ok(new { success = true, message = $"Добро пожаловать, {employee.FullName}!" });

        }

        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(Employee emp)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            emp.MyPassword = Cryptor.Encrypt(emp.MyPassword);

            await _context.Employees.AddAsync(emp);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = emp.Id }, emp);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditEmployee(int id, Employee emp)
        {
            var existingEmployee = await _context.Employees.FindAsync(id);
            if (existingEmployee == null)
                return NotFound();

            existingEmployee.FullName = emp.FullName;
            existingEmployee.PositionId = emp.PositionId;
            existingEmployee.Address = emp.Address;
            existingEmployee.Phone = emp.Phone;
            existingEmployee.Email = emp.Email;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("pass/{id}/{password}/{newPassword}")]
        public async Task<ActionResult> ChangePassword(int id, string password, string newPassword)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound(new { success = false, message = "Пользователь не найден" });

            if (Cryptor.Encrypt(password) != employee.MyPassword)
                return BadRequest(new { success = false, message = "Неверный старый пароль" });

            employee.MyPassword = Cryptor.Encrypt(newPassword);
            await _context.SaveChangesAsync();

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
