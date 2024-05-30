using Domain.Entities.Products;
using Infrastructure.Context;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IProductRepository: IGenericRepository<Product>
    {

    }
    public class ProductRepository : GenericRepository<Product, TenantEfContext>, IProductRepository
    {
        public ProductRepository(TenantEfContext dbContext) : base(dbContext)
        {
        }

       
    }
}
