using Application.Products.Add;
using Application.Services;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PruebaTecnica.Middlewares;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(configuracion => {
    configuracion.RegisterServicesFromAssemblies(typeof(AddProductCommand).Assembly);
});


var conn = builder.Configuration.GetConnectionString("PsgqlOrgAndUsersDbConnection");
var conn1 = builder.Configuration.GetConnectionString("PsgqlTenantDbConnection");

builder.Services.AddDbContext<OrganizationAndUsersEfContext>(options => options.UseNpgsql(conn, x=> x.MigrationsAssembly("Infrastructure")));
builder.Services.AddDbContext<TenantEfContext>(options => options.UseNpgsql(conn1, x => x.MigrationsAssembly("Infrastructure")));

builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IOrganizationRepository,OrganizationRepository>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrganizationService>();


//authentification
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
                    ClockSkew = TimeSpan.Zero
                });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    OrganizationAndUsersEfContext context = scope.ServiceProvider.GetRequiredService<OrganizationAndUsersEfContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRouting();
app.UseAuthorization();
//tenant Middleware
app.UseTenantMiddleware();
app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{slugTenant}/{controller=Home}/{action=Index}/{id?}");
});



app.Run();
