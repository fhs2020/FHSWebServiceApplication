using FHSWebServiceApplication.Interfaces;
using FHSWebServiceApplication.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FHSWebServiceApplication.Controllers
{
    [Authorize]
    [Route("api/products")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _repository;

        public ProductController(IRepository<Product> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Get all Products
        /// </summary>
        /// <returns>A list of Products</returns>
        /// <response code="200">All itens found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Product> Get()
        {
            return _repository.GetAll();
        }

        /// <summary>
        /// Get an especific Product
        /// </summary>
        /// <param name="id">The Product id</param>
        /// <returns>The Product</returns>
        /// <response code="200">If the Product is found</response>
        /// <response code="404">if the Product is not found</response>
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = null)]
        public ActionResult<Product> Get(int id)
        {
            var product = _repository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        /// <summary>
        /// Create a new Prodcut
        /// </summary>
        /// <param name="product">The Product object</param>
        /// <returns>A newly created Product</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Product
        ///     {
        ///        "id": 0,
        ///        "name": "Item #1",
        ///        "price": 10
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created product</response>
        /// <response code="400">If the product is invalid</response>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public ActionResult<Product> Post([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            var createdProduct = _repository.Create(product);

            return CreatedAtRoute("Get", new { id = createdProduct.Id }, createdProduct);
        }

        /// <summary>
        /// Updates a Product resource
        /// </summary>
        /// <param name="id">The product id</param>
        /// <param name="product">The Product object</param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Prodcut
        ///     {
        ///        "id": 1,
        ///        "name": "Item #1",
        ///        "price": 20
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Product updated</response>
        /// <response code="400">If the product is invalid</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = null)]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            if (product == null || id != product.Id)
            {
                return BadRequest("Invalid product id");
            }

            var existingProduct = _repository.GetById(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            _repository.Update(product);

            return NoContent();
        }

        /// <summary>
        /// Deletes a specific Product.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="204">Product deleted</response>
        /// <response code="404">If the product is not found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = null)]
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
