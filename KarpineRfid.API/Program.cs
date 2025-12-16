using KarpineRfid.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();








if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();              // Generates openapi.json
    app.UseSwaggerUI(options =>    // Enables Swagger UI
    {
        options.SwaggerEndpoint("/openapi/v1.json", "RFID API v1");
    });

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
