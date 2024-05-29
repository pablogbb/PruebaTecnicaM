using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repositories.Generic;
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
    public class ProductRepository : GenericRepository<Product, OrganizationProductsEfContext>, IProductRepository
    {
        public ProductRepository(OrganizationProductsEfContext dbContext) : base(dbContext)
        {
        }
    }
}
