using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var con1 = builder.Configuration.GetConnectionString("PsgqlOrgAndUsersDbConnection");
var con2 = builder.Configuration.GetConnectionString("PsgqlOrgAndProdsDbConnection");

builder.Services.AddDbContext<OrganizationAndUsersEfContext>(options => options.UseNpgsql(con1));
builder.Services.AddDbContext<OrganizationProductsEfContext>(options => options.UseNpgsql(con2));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
