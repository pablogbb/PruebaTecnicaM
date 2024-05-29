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
        public DbSet<UserOrganization> UserOrganizations { get; set; }

        public OrganizationAndUsersEfContext(DbContextOptions<OrganizationAndUsersEfContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Organizations)
                .WithMany(e => e.Users).UsingEntity<UserOrganization>(
                j => j.HasOne(o => o.Organization).WithMany().HasForeignKey(uc => uc.OrganizationId),
                j => j.HasOne(u => u.User).WithMany().HasForeignKey(uc => uc.UserId),
                j =>
                {
                    j.HasKey(uc => new { uc.UserId, uc.OrganizationId });
                    j.ToTable("UserOrganizations"); // Nombre de la tabla para la entidad de unión
                    j.HasData(
                        new { UserId = 1, OrganizationId = 1 },
                        new { UserId = 1, OrganizationId = 2 },
                        new { UserId = 2, OrganizationId = 1 }
                    );
                });

            // Datos iniciales para User
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "John Doe", Email = "john@example.com", Password = "password" },
                new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com", Password = "password" }
            );

            // Datos iniciales para Organization
            modelBuilder.Entity<Organization>().HasData(
                new Organization { Id = 1, Name = "Org1", SlugTenant = "org1" },
                new Organization { Id = 2, Name = "Org2", SlugTenant = "org2" }
            );
        }
    }
}
