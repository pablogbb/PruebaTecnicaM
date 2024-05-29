using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context
{
    public class TenantEfContext : DbContext
    {
        public TenantEfContext(DbContextOptions<TenantEfContext> options) : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
    }
}
