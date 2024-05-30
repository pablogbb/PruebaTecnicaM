using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IOrganizationRepository: IGenericRepository<Organization>
    {
        Task CreateDatabaseAndMigrateAsync(String SlugTenant, String TenantConnectionString);
    }
    public class OrganizationRepository : GenericRepository<Organization, OrganizationAndUsersEfContext>, IOrganizationRepository
    {
        private TenantEfContext tenantEfContext;
        public OrganizationRepository(OrganizationAndUsersEfContext dbContext) : base(dbContext)
        {
        }

        public async Task CreateDatabaseAndMigrateAsync(String SlugTenant, String TenantConnectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TenantEfContext>();

            //var connectionString = _configuration.GetConnectionString("PsgqlTenantDbConnection");
            var tenantConnectionString = TenantConnectionString.Replace("{DatabaseName}", SlugTenant);

            optionsBuilder.UseNpgsql(tenantConnectionString);
                       

            using (TenantEfContext tenantEfContext = new TenantEfContext(optionsBuilder.Options))
            {
                //await tenantEfContext.Database.EnsureCreatedAsync();
                await tenantEfContext.Database.MigrateAsync();
            }
                
        }

    }
}
