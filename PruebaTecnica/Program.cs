using Application.Services;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var conn = builder.Configuration.GetConnectionString("PsgqlOrgAndUsersDbConnection");
var conn1 = builder.Configuration.GetConnectionString("PsgqlTenantDbConnection");

builder.Services.AddDbContext<OrganizationAndUsersEfContext>(options => options.UseNpgsql(conn, x=> x.MigrationsAssembly("Infrastructure")));
builder.Services.AddDbContext<TenantEfContext>(options => options.UseNpgsql(conn1, x => x.MigrationsAssembly("Infrastructure")));

builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IOrganizationRepository,OrganizationRepository>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrganizationService>();

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

app.UseAuthorization();

app.UseTenantMiddleware();

app.MapControllers();

app.Run();
