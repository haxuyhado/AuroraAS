using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity.Infrastructure;

namespace MedSalesAS_API.Controllers
{
    public class EmployeesController : ApiController
    {
        private MSDataEntities db = new MSDataEntities(); 

        // GET: api/employees
        [HttpGet]
        [Route("api/employees")]
        public IHttpActionResult GetEmployees()
        {
            var employees = db.Employees.ToList();
            if (employees == null || !employees.Any())
            {
                return NotFound(); 
            }
            return Ok(employees);
        }

        // GET: api/employees/{id}
        [HttpGet]
        [Route("api/employees/{id}")]
        public IHttpActionResult GetEmployee(int id)
        {
            var employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound(); // Возвращаем 404, если сотрудник не найден
            }
            return Ok(employee); // Возвращаем 200 и сотрудника
        }

        // POST: api/employees
        [HttpPost]
        [Route("api/employees")]
        public IHttpActionResult PostEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Возвращаем 400, если модель не валидна
            }

            db.Employees.Add(employee);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = employee.id }, employee); // Возвращаем 201 и созданного сотрудника
        }

        // PUT: api/employees/{id}
        [HttpPut]
        [Route("api/employees/{id}")]
        public IHttpActionResult PutEmployee(int id, Employee employee)
        {
            if (id != employee.id)
            {
                return BadRequest(); // Возвращаем 400, если ID не совпадает
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Возвращаем 400, если модель не валидна
            }

            db.Entry(employee).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound(); // Возвращаем 404, если сотрудник не найден
                }
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent); // Возвращаем 204, если обновление прошло успешно
        }

        // DELETE: api/employees/{id}
        [HttpDelete]
        [Route("api/employees/{id}")]
        public IHttpActionResult DeleteEmployee(int id)
        {
            var employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound(); // Возвращаем 404, если сотрудник не найден
            }

            db.Employees.Remove(employee);
            db.SaveChanges();

            return Ok(employee); // Возвращаем 200 и удаленного сотрудника
        }

        // Проверка существования сотрудника
        private bool EmployeeExists(int id)
        {
            return db.Employees.Count(e => e.id == id) > 0;
        }

        // Dispose method
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
