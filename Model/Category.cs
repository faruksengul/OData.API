using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OData.API.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
