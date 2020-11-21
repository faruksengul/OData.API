using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OData.API.Model;

namespace OData.API.Controllers
{
    [ODataRoutePrefix("Products")]
    public class ProductsController : ODataController
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [EnableQuery(PageSize =2)]
        public IActionResult GetProducts()
        {
            return Ok(_context.Products.ToList());
        }

        [ODataRoute("({Item})")]
        [EnableQuery]
        public IActionResult GetProduct([FromODataUri]int Item)
        {
            return Ok(_context.Products.Where(f => f.Id == Item));
        }

        [HttpPost]
        public IActionResult PostProduct([FromBody] Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok(product);
        }

        [HttpPut]
        public IActionResult PutProduct([FromODataUri] int key,[FromBody] Product product)
        {
            product.Id = key;
            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteProduct([FromODataUri]int key)
        {
            var product = _context.Products.Find(key);
            if (product == null)
                return NotFound();
            _context.Products.Remove(product);
            _context.SaveChanges();
            return NoContent();

        }


    }
}
