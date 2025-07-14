using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

//--------------------------------------------
// Servicios
//--------------------------------------------

// 1️⃣ CORS – permite *cualquier* origen, método y encabezado (solo DEV)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p => p
        .AllowAnyOrigin()      // <— cualquier host
        .AllowAnyMethod()      // GET, POST, PUT, DELETE…
        .AllowAnyHeader());    // Content-Type, Authorization…
});

builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//--------------------------------------------
// Middleware
//--------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 2️⃣ Activar la política CORS antes de authorization
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();