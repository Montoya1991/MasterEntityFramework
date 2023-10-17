using Aplicacion.ManejadorError;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebAPI.Middleware
{
    public class ManejadorErrorMiddleware
    {
        // Constructor de la clase.
        private readonly RequestDelegate _next;
        // "RequestDelegate" representa la próxima función middleware en la cadena.
        // "_next" es una referencia a esa función, que será invocada después de este middleware.

        private readonly ILogger<ManejadorErrorMiddleware> _logger;
        // "ILogger<ManejadorErrorMiddleware>" es un objeto de registro que permite registrar eventos y errores.
        // "_logger" es una referencia a ese objeto de registro.


        // Constructor de la clase.
        public ManejadorErrorMiddleware(RequestDelegate next, ILogger<ManejadorErrorMiddleware> logger)
        {
            // El constructor recibe dos parámetros:
            // - "next": Un delegado que representa la próxima función middleware en la cadena de middleware.
            // - "logger": Un objeto de registro que permite registrar información de errores y eventos.

            _next = next;     // Asigna el delegado "next" a la variable privada "_next".
            _logger = logger; // Asigna el objeto de registro "logger" a la variable privada "_logger".
        }

        // Método "Invoke" que maneja las excepciones durante el procesamiento de las solicitudes HTTP.
        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Intenta ejecutar la próxima función middleware en la cadena de middleware.
                await _next(context);
            }
            catch (Exception ex)
            {
                // En caso de que ocurra una excepción, llama al método "ManejadorExcepcionAsincrono" para manejarla.
                await ManejadorExcepcionAsincrono(context, ex, _logger);
            }
        }

        // Método que maneja excepciones y genera respuestas de error personalizadas.
        private async Task ManejadorExcepcionAsincrono(HttpContext context, Exception ex, ILogger<ManejadorErrorMiddleware> logger)
        {
            // Inicializa la variable "errores" como nula.
            object errores = null;

            // Se manejan diferentes tipos de excepciones.
            switch (ex)
            {
                case ManejadorException me:
                    // En caso de una excepción personalizada "ManejadorException", registra un error en el registro ("logger").
                    logger.LogError(ex, "Manejador error");

                    // Obtiene los detalles del error personalizado "ManejadorException".
                    errores = me.Errores;

                    // Establece el código de estado de la respuesta HTTP con el código de estado de la excepción personalizada.
                    context.Response.StatusCode = (int)me.Codigo;
                    break;
                case Exception e:
                    // En caso de una excepción genérica, registra un error de servidor en el registro.
                    logger.LogError(ex, "Error de servidor");

                    // Obtiene un mensaje de error genérico, o "Error" si el mensaje es nulo o en blanco.
                    errores = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;

                    // Establece el código de estado de la respuesta HTTP en "InternalServerError" (500).
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            // Establece el tipo de contenido de la respuesta HTTP como JSON.
            context.Response.ContentType = "application/json";

            // Si hay detalles de error, los serializa como JSON y los envía en la respuesta.
            if (errores != null)
            {
                // Serializa los errores en un objeto JSON anidado.
                var resultados = JsonConvert.SerializeObject(new { errores });

                // Escribe el objeto JSON en la respuesta HTTP.
                await context.Response.WriteAsync(resultados);
            }
        }
    }
}
