using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity.Infrastructure;

namespace MedSalesAS_API.Controllers
{
    public class PositionsController : ApiController
    {
        private MSDataEntities db = new MSDataEntities();

        // GET: api/positions
        [HttpGet]
        [Route("api/positions")]
        public IHttpActionResult GetPositions()
        {
            var positions = db.Positions.ToList();
            if (positions == null || !positions.Any())
            {
                return NotFound();
            }
            return Ok(positions);
        }

        // GET: api/positions/{id}
        [HttpGet]
        [Route("api/positions/{id}")]
        public IHttpActionResult GetPositions(int id)
        {
            var positions = db.Positions.Find(id);
            if (positions == null)
            {
                return NotFound(); // Возвращаем 404, если сотрудник не найден
            }
            return Ok(positions); // Возвращаем 200 и сотрудника
        }
    }
}