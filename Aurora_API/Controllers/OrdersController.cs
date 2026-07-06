using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity.Infrastructure;

namespace MedSalesAS_API.Controllers
{
    public class OrdersController : ApiController
    {
        private MSDataEntities db = new MSDataEntities();

        [HttpGet]
        [Route("api/orders")]
        public IHttpActionResult GetOrders()
        {
            var orders = db.Orders.ToList();
            if (orders == null || !orders.Any())
                return NotFound();
            return Ok(orders);
        }

        [HttpGet]
        [Route("api/orders/{id}")]
        public IHttpActionResult GetOrder(int id)
        {
            var order = db.Orders.Find(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpPost]
        [Route("api/orders")]
        public IHttpActionResult PostOrder(Order order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Orders.Add(order);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = order.id }, order);
        }

        [HttpPut]
        [Route("api/orders/{id}")]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (id != order.id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Entry(order).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                    return NotFound();
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("api/orders/{id}")]
        public IHttpActionResult DeleteOrder(int id)
        {
            var order = db.Orders.Find(id);
            if (order == null)
                return NotFound();

            db.Orders.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.id == id) > 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}