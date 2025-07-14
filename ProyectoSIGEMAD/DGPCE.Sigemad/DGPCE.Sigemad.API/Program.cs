using DGPCE.Sigemad.API.Middleware;
using DGPCE.Sigemad.Application;
using DGPCE.Sigemad.Identity;
using DGPCE.Sigemad.Identity.Services;
using DGPCE.Sigemad.Identity.Services.Interfaces;
using DGPCE.Sigemad.Infrastructure;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.IO.Converters;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()


    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Converters.Add(new GeometryConverter());
    });



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.ConfigureIdentityServices(builder.Configuration);


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    );
});

// Configuracion para multilenguaje
builder.Services.AddLocalization(options => options.ResourcesPath = "");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "es" };
    options.SetDefaultCulture("es");
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});

/*
// Configura la política predeterminada para permitir a todos
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequiereAutorizacion", policy =>
        policy.RequireAuthenticatedUser()); // Requiere que el usuario esté autenticado

    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAssertion(_ => true) // Permitir a todos
        .Build();
});
*/

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.AddScoped<IExcelExportService, ExcelExportService>();

var app = builder.Build();

// Habilitar multilenguaje
app.UseRequestLocalization();


// Configure the HTTP request pipeline.
bool IsSwaggerEnabled = builder.Configuration.GetValue<bool>("Swagger:Enabled");
if (app.Environment.IsDevelopment() || IsSwaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();


app.UseCors("CorsPolicy"); //1. Siempre primero va uso de CORS
app.UseAuthorization(); // 2. Se hace uso de reglas de authorization
app.UseMiddleware<AuthenticatedUserMiddleware>();

app.UseSerilogRequestLogging();

app.MapControllers();


app.Run();
