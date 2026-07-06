using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity.Infrastructure;

namespace MedSalesAS_API.Controllers
{
    public class ProductsController : ApiController
    {
        private MSDataEntities db = new MSDataEntities();

        [HttpGet]
        [Route("api/products")]
        public IHttpActionResult GetProducts()
        {
            var products = db.Products.ToList();
            if (products == null || !products.Any())
                return NotFound();
            return Ok(products);
        }

        [HttpGet]
        [Route("api/products/{id}")]
        public IHttpActionResult GetProduct(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        [Route("api/products")]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.id }, product);
        }

        [HttpPut]
        [Route("api/products/{id}")]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (id != product.id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Entry(product).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                    return NotFound();
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("api/products/{id}")]
        public IHttpActionResult DeleteProduct(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
                return NotFound();

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.id == id) > 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}