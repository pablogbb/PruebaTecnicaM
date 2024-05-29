using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace PruebaTecnica.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public TenantMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            // Obtener el slugTenant de la URL
            var slugTenant = context.Request.RouteValues["slugTenant"] as string;

            if (!string.IsNullOrEmpty(slugTenant))
            {
                // Crear el DbContext basado en el tenantId
                var dbContext = CreateDbContext(context, slugTenant);
                context.Items["DbContext"] = dbContext;
            }

            await _next(context);
        }

        private TenantEfContext CreateDbContext(HttpContext context, string slugTenant)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TenantEfContext>();

            var connectionString = _configuration.GetConnectionString("PsgqlTenantDbConnection");
            var tenantConnectionString = connectionString??"".Replace("{DatabaseName}", slugTenant);

            optionsBuilder.UseNpgsql(tenantConnectionString);

            return new TenantEfContext(optionsBuilder.Options);
        }
    }
}
