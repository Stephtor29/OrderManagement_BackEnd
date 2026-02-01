using Microsoft.EntityFrameworkCore;
using OrderManagement.DataAccess.DBContext;
using OrderManagement.API.Extensions;
using OrderManagement.BusinessLogic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var connectionString = builder.Configuration.GetConnectionString("OrderManagementConn");

builder.Services.AddDbContext<OrderManagementDBContext>(option =>
    option.UseSqlServer(connectionString));

builder.Services.DataAccess(connectionString);
builder.Services.BusinessLogic();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(typeof(MappingProfileExtensions));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();