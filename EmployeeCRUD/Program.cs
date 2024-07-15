using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EmployeeCRUD.Data;
using EmployeeCRUD.Service;
using Microsoft.Build.Framework;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


// Register the encoding provider
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

builder.Services.AddDbContext<EmployeeCRUDContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeCRUDContext") ?? throw new InvalidOperationException("Connection string 'EmployeeCRUDContext' not found.")));

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {

            // for home
            //builder.WithOrigins("http://192.168.0.104:8080")
            //       .AllowAnyHeader()
            //       .AllowAnyMethod()
            //       .AllowCredentials(); // If using credentials

            //for office

            builder.WithOrigins("http://192.168.15.102:8081")
                 .AllowAnyHeader()
                 .AllowAnyMethod()
                 .AllowCredentials(); // If using credentials



        });
});

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.Run();
