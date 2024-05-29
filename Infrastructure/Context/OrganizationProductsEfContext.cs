using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context
{
    public class OrganizationProductsEfContext : DbContext
    {
        public virtual DbSet<Organization> Users { get; set; }

        public OrganizationProductsEfContext(DbContextOptions<OrganizationProductsEfContext> options)
        : base(options)
        {
        }
    }
}
