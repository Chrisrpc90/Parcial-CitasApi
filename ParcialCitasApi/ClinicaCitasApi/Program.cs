using ClinicaCitasApi.Middleware;
using ClinicaCitasApi.Services;
using ClinicaCitasApi.Services.Interfaces;
using ClinicaCitasApi.Storage;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Personaliza errores de validación (DTOs)
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        return new BadRequestObjectResult(new
        {
            title = "Validación fallida",
            status = 400,
            errors,
            traceId = context.HttpContext.TraceIdentifier
        });
    };
});

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Persistencia en memoria
builder.Services.AddSingleton<InMemoryDatabase>();

// Servicios
builder.Services.AddScoped<IPacientesService, PacientesService>();
builder.Services.AddScoped<IMedicosService, MedicosService>();
builder.Services.AddScoped<ICitasService, CitasService>();
builder.Services.AddScoped<IEspecialidadesService, EspecialidadesService>();

var app = builder.Build();

// Manejo centralizado de errores
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swagger + UI
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ClinicaCitasApi v1");
    options.RoutePrefix = "swagger";
});

// Habilitar wwwroot (archivos estáticos)
app.UseStaticFiles();

// Home -> dashboard.html
app.MapGet("/", () => Results.Redirect("/dashboard.html"));

// (Recomendado quitar para evitar warning en HTTP)
// app.UseHttpsRedirection();

app.MapControllers();

// IMPRIME LINKS EN CONSOLA
app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine("\n==============================");
    Console.WriteLine("✅ API corriendo en:");
    foreach (var u in app.Urls) Console.WriteLine(u);
    Console.WriteLine("📘 Swagger:");
    foreach (var u in app.Urls) Console.WriteLine($"{u}/swagger");
    Console.WriteLine("🧭 Dashboard:");
    foreach (var u in app.Urls) Console.WriteLine($"{u}/dashboard.html");
    Console.WriteLine("==============================\n");
});

app.Run();