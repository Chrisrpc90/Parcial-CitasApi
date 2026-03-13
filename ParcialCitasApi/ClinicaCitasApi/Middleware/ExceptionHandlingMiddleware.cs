using System.Net;
using System.Text.Json;

namespace ClinicaCitasApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await Handle(context, ex);
            }
        }

        private static Task Handle(HttpContext context, Exception ex)
        {
            var (status, title) = ex switch
            {
                KeyNotFoundException => (HttpStatusCode.NotFound, "Recurso no encontrado"),
                ArgumentException => (HttpStatusCode.BadRequest, "Datos inválidos"),
                InvalidOperationException => (HttpStatusCode.Conflict, "Operación no permitida"),
                _ => (HttpStatusCode.InternalServerError, "Error interno")
            };

            var problem = new
            {
                type = "about:blank",
                title,
                status = (int)status,
                detail = ex.Message,
                traceId = context.TraceIdentifier
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
