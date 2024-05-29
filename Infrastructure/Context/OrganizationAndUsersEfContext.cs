using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context
{
    public class OrganizationAndUsersEfContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }

        public OrganizationAndUsersEfContext(DbContextOptions<OrganizationAndUsersEfContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Organizations)
                .WithMany(e => e.Users);
        }
    }
}
