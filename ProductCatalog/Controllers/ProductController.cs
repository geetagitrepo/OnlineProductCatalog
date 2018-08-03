using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.EF;
using ProductCatalog.Models;
using ProductCatalog.Repository;

namespace ProductCatalog.Controllers
{
    [Produces("application/json")]
    [Route("api/Product")]
    public class ProductController : Controller
    {
        private readonly IProductRepository repository;

        public ProductController(IProductRepository repository)
        {
            this.repository = repository;
        }

        //GET api/products
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(repository.GetAll());
        }

        // GET api/products/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await repository.FindAsync(id);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        // POST api/products
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Product model)
        {
            if (model == null)
                return BadRequest("Please provide a Product.");

            model.Photo = model.Photo == null ? Array.Empty<byte>() : model.Photo;

            var entity = await repository.AddAsync(model);

            return Ok(entity);

        }

        //PUT api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Product model)
        {
            if (!ModelState.IsValid || id != model.Id)
            {
                return BadRequest();
            }

            var entity = await repository.FindAsync(id);
            model.Photo = model.Photo == null ? entity.Photo : model.Photo;

            if (entity == null)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.NotFound)
                { ReasonPhrase = "Unable to find this record." };
                throw new HttpResponseException(msg);
            }

            int response = await repository.UpdateAsync(model);

            if (response == 0)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                { ReasonPhrase = "Error occured while updating data." };
                throw new HttpResponseException(msg);
            }

            return new NoContentResult();
        }

        // DELETE api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await repository.RemoveAsync(id);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                return BadRequest("No Product found with id " + id);
            }

            return new NoContentResult();
        }

        //public IActionResult Get(int pageNumber, int pageSize)
        //{
        //    return Ok(repository.GetAll(pageNumber, pageSize));
        //}
    }
}