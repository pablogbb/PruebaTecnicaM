using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

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

        public async Task Invoke(HttpContext context, TenantEfContext tenantEfContext)
        {
            var slugTenant = context.Request.RouteValues["slugTenant"] as string;

            if (!string.IsNullOrEmpty(slugTenant))
            {
                var connectionString = _configuration.GetConnectionString("PsgqlTenantDbConnection") ?? "";
                var tenantConnectionString = connectionString.Replace("{DatabaseName}", slugTenant);

                tenantEfContext.ChangeConnectionString(tenantConnectionString);

                // validacion con el token
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Authorization token is missing.");
                    return;
                }
                try
                {
                    var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                    var tenantIds = jwtToken.Claims.Where(c => c.Type == "SlugTenant").Select(c => c.Value).ToList();

                    if (!tenantIds.Contains(slugTenant))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Unauthorized: Invalid tenant.");
                        return;
                    }
                }
                catch
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid token.");
                    return;
                }
            }

            await _next(context);
        }
    }
}
