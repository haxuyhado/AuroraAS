using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity.Infrastructure;

namespace MedSalesAS_API.Controllers
{
    public class ItemsInOrderController : ApiController
    {
        private MSDataEntities db = new MSDataEntities();

        [HttpGet]
        [Route("api/itemsinorder")]
        public IHttpActionResult GetItemsInOrder()
        {
            var items = db.ItemsInOrders.ToList();
            if (items == null || !items.Any())
                return NotFound();
            return Ok(items);
        }

        [HttpGet]
        [Route("api/itemsinorder/{id}")]
        public IHttpActionResult GetItemInOrder(int id)
        {
            var item = db.ItemsInOrders.Find(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [Route("api/itemsinorder")]
        public IHttpActionResult PostItemInOrder(ItemsInOrder item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.ItemsInOrders.Add(item);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = item.id }, item);
        }

        [HttpPut]
        [Route("api/itemsinorder/{id}")]
        public IHttpActionResult PutItemInOrder(int id, ItemsInOrder item)
        {
            if (id != item.id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Entry(item).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemInOrderExists(id))
                    return NotFound();
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("api/itemsinorder/{id}")]
        public IHttpActionResult DeleteItemInOrder(int id)
        {
            var item = db.ItemsInOrders.Find(id);
            if (item == null)
                return NotFound();

            db.ItemsInOrders.Remove(item);
            db.SaveChanges();

            return Ok(item);
        }

        private bool ItemInOrderExists(int id)
        {
            return db.ItemsInOrders.Count(e => e.id == id) > 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}