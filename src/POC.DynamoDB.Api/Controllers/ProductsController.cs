using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using POC.DynamoDB.Api.Infra.Contracts;
using POC.DynamoDB.Api.Models.Entities;
using POC.DynamoDB.Api.Models.Request;

namespace POC.DynamoDB.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetAsync([FromQuery] ProductFilterRequest filters)
        {
            return await _repository.FindAsync(filters, CancellationToken.None);
        }

        // POST: api/Products

        [HttpPost]
        public async Task<IActionResult> PostAsync(ProductRequest request)
        {
            await _repository.SaveOrUpdate((Product) request);

            return StatusCode((int) HttpStatusCode.Created);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, ProductRequest request)
        {
            request.Id = id;

            await _repository.SaveOrUpdate((Product) request);

            return Ok();
        }
    }
}