namespace DGPCE.Sigemad.API.Middleware;

public class RequestLoggerMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Aquí tienes el método y el path del endpoint que se está llamando
        var method = context.Request.Method;
        var path = context.Request.Path;

        // Puedes poner un breakpoint aquí para atrapar TODAS las llamadas al backend
        //System.Diagnostics.Debugger.Break();

        // O imprimir el log para ver en Output o en la consola de logs
        Console.WriteLine($"Request: {method} {path}");

        await _next(context); // Sigue con el pipeline
    }
}
