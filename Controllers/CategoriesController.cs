using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OData.API.Model;

namespace OData.API.Controllers
{
   
    public class CategoriesController : ODataController
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [EnableQuery]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Categories);
        }

        [EnableQuery]
        public IActionResult GetCategories([FromODataUri] int key)
        {
            return Ok(_context.Categories.Where(f => f.Id == key));
        }


        [HttpPost]
        public IActionResult TotalProductPrice([FromODataUri]int key)
        {
            var total = _context.Products.Where(f => f.CategoryId == key).Sum(x => x.Price);
            return Ok(total);
        }

        [HttpPost]
        public IActionResult TotalProductPrice2()
        {
            var total = _context.Products.Sum(x => x.Price);
            return Ok(total);

        }

        [HttpPost]
        public IActionResult TotalProductPriceWithParametre(ODataActionParameters parameters)
        {
            int categoryId = (int)parameters["categoryId"];
            var total = _context.Products.Where(f => f.CategoryId == categoryId).Sum(x => x.Price);
            return Ok(total);
        }
    }
}
