using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Products.Especifications
{
    public class ListProductSpec : Specification<Product>
    {
        public ListProductSpec()
        {
            Query.OrderBy(p => p.Name);
        }
    }
}
