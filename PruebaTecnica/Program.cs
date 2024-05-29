using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var con1 = builder.Configuration.GetConnectionString("PsgqlOrgAndUsersDbConnection");
var con2 = builder.Configuration.GetConnectionString("PsgqlOrgAndProdsDbConnection");

builder.Services.AddDbContext<OrganizationAndUsersEfContext>(options => options.UseNpgsql(con1));
builder.Services.AddDbContext<TenantEfContext>();


var app = builder.Build();


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
