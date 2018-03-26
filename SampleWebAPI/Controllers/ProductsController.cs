using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebAPI.DataProviders;
using SampleWebAPI.Models;

namespace SampleWebAPI.Controllers
{
    // [Produces("application/json")]
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

        [HttpPost("{id}")]
        public IActionResult BlockProductCreation(int id)
        {
            if(_productsProvider.GetItemById(id)==null)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }
        }

        //Get the product information using POST and create Product Objects
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
                //return Created("api/Products/", product);
                return CreatedAtRoute("GetSpecificProduct", new { id = Id_Responce }, product);
            }


        }

    //// Return all product list using GET ; GET: api/<controller>  
        [HttpGet]
        public IActionResult GetAllProducts()
        {
                //throw new Exception("Just a test");
                return Ok(_productsProvider.GetAllProducts());
        }

        //Return a specific Product matched by ID
        [HttpGet("{id}",Name ="GetSpecificProduct")]
       // [Produces("application/json")]
        public IActionResult GetProduct(int id)
        {
            var product = _productsProvider.GetItemById(id);
            if (product == null)
            {

                return NotFound();
            }
            //return Ok(new JsonResult(product));
            return Ok(product);
        }


        //DELETE RESOURCE
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            if(_productsProvider.GetItemById(id)==null)
            {
                return NotFound();
            }
            else
            {
                _productsProvider.Delete(id);
                return NoContent();
            }
        }

        //FULL UPDATE ON RESOURCE
        [HttpPut("{id}")]
        public IActionResult FullUpdate (int id, [FromBody] Products product)
        {
            if (_productsProvider.GetItemById(id) == null)
            {

                return NotFound();
            }
            else
            {
                _productsProvider.Update(id, product);
                return NoContent();
            }
        }

        //PARTIAL UPDATE RESOURCE
        [HttpPatch("{id}")]
        public IActionResult PartialUpdate (int id,[FromBody] JsonPatchDocument<Products> patchDoc)
        {
            if(patchDoc==null)
            {
                return BadRequest();
            }

            var product=_productsProvider.GetItemById(id);

            if (product==null)
            {
                return NotFound();
            }


            patchDoc.ApplyTo(product);

            _productsProvider.Update(id, product);

            return NoContent();

        }


    }
}