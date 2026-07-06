using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity.Infrastructure;

namespace MedSalesAS_API.Controllers
{
    public class CreateProductController : ApiController
    {
        private MSDataEntities db = new MSDataEntities();

        [HttpGet]
        [Route("api/createproduct")]
        public IHttpActionResult GetCreateProducts()
        {
            var createProducts = db.CreateProducts.ToList();
            if (createProducts == null || !createProducts.Any())
                return NotFound();
            return Ok(createProducts);
        }

        [HttpGet]
        [Route("api/createproduct/{id}")]
        public IHttpActionResult GetCreateProduct(int id)
        {
            var createProduct = db.CreateProducts.Find(id);
            if (createProduct == null)
                return NotFound();
            return Ok(createProduct);
        }

        [HttpPost]
        [Route("api/createproduct")]
        public IHttpActionResult PostCreateProduct(CreateProduct createProduct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.CreateProducts.Add(createProduct);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = createProduct.id }, createProduct);
        }

        [HttpPut]
        [Route("api/createproduct/{id}")]
        public IHttpActionResult PutCreateProduct(int id, CreateProduct createProduct)
        {
            if (id != createProduct.id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Entry(createProduct).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CreateProductExists(id))
                    return NotFound();
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("api/createproduct/{id}")]
        public IHttpActionResult DeleteCreateProduct(int id)
        {
            var createProduct = db.CreateProducts.Find(id);
            if (createProduct == null)
                return NotFound();

            db.CreateProducts.Remove(createProduct);
            db.SaveChanges();

            return Ok(createProduct);
        }

        private bool CreateProductExists(int id)
        {
            return db.CreateProducts.Count(e => e.id == id) > 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}