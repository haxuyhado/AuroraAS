using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity.Infrastructure;

namespace MedSalesAS_API.Controllers
{
    public class ClientsController : ApiController
    {
        private MSDataEntities db = new MSDataEntities();

        [HttpGet]
        [Route("api/clients")]
        public IHttpActionResult GetClients()
        {
            var clients = db.Clients.ToList();
            if (clients == null || !clients.Any())
                return NotFound();
            return Ok(clients);
        }

        [HttpGet]
        [Route("api/clients/{id}")]
        public IHttpActionResult GetClient(int id)
        {
            var client = db.Clients.Find(id);
            if (client == null)
                return NotFound();
            return Ok(client);
        }

        [HttpPost]
        [Route("api/clients")]
        public IHttpActionResult PostClient(Client client)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Clients.Add(client);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = client.id }, client);
        }

        [HttpPut]
        [Route("api/clients/{id}")]
        public IHttpActionResult PutClient(int id, Client client)
        {
            if (id != client.id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Entry(client).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                    return NotFound();
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("api/clients/{id}")]
        public IHttpActionResult DeleteClient(int id)
        {
            var client = db.Clients.Find(id);
            if (client == null)
                return NotFound();

            db.Clients.Remove(client);
            db.SaveChanges();

            return Ok(client);
        }

        private bool ClientExists(int id)
        {
            return db.Clients.Count(e => e.id == id) > 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}