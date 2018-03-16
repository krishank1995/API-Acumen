using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleWebAPI.DataProviders;
using SampleWebAPI.Models;

namespace SampleWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        static public int Id_Responce { set; get; }

        //Object for accessing  data provider
        private readonly IProductsProvider _productsProvider;
        public ProductsController(IProductsProvider productsProvider) //Constuctor for Controller
        {
            _productsProvider = productsProvider;

        }

        //Get the product information using POST and create Product Objects
        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody] Products product) //What is IActionResult ? // public IActionResult Post([FromBody]values value) ---> [From Body] --> Fetches it from HTML Body 
        {
            if (!ModelState.IsValid) //If Invalid   
            {
                return (BadRequest(ModelState));  // What is Model State
            }

            else if (product.Name == null || product.Price == 0)
            {
                return BadRequest("Price or Name not supplied");
            }

            else
            {
                Id_Responce = _productsProvider.AddItem(product);
                product.Id = Id_Responce;
                return Created("api/Products/", product);
            }


        }

        //// Return all product list using GET ; GET: api/<controller>  
        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetAllProducts()
        {

            try
            {

                return Ok(_productsProvider.GetAllProducts());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //Return a specific Product matched by ID
        [HttpGet("{id}")]
        [Produces("application/json")]
        public IActionResult GetProduct(int id)
        {
            var product = _productsProvider.GetItemById(id);
            if (product == null)
            {

                return NotFound();
            }

            return Ok(product);
        }
    }
}