using FHSWebServiceApplication.Interfaces;
using FHSWebServiceApplication.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FHSWebServiceApplication.Controllers
{
  [Route("api/products")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly IRepository<Product> _repository;

    public ProductController(IRepository<Product> repository)
    {
      _repository = repository;
    }

    // GET: api/products
    [HttpGet]
    public ActionResult<IEnumerable<Product>> Get()
    {
      var products = _repository.GetAll();
      return Ok(products);
    }

    // GET: api/products/5
    [HttpGet("{id}", Name = "Get")]
    public ActionResult<Product> Get(int id)
    {
      var product = _repository.GetById(id);
      if (product == null)
      {
        return NotFound();
      }
      return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public ActionResult<Product> Post([FromBody] Product product)
    {
      if (product == null)
      {
        return BadRequest();
      }

      var createdProduct = _repository.Create(product);

      return CreatedAtRoute("Get", new { id = createdProduct.Id }, createdProduct);
    }

    // PUT: api/products/5
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Product product)
    {
      if (product == null || id != product.Id)
      {
        return BadRequest();
      }

      var existingProduct = _repository.GetById(id);
      if (existingProduct == null)
      {
        return NotFound();
      }

      _repository.Update(product);

      return NoContent();
    }

    // DELETE: api/products/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      var existingProduct = _repository.GetById(id);
      if (existingProduct == null)
      {
        return NotFound();
      }

      _repository.Delete(id);

      return NoContent();
    }
  }
}
