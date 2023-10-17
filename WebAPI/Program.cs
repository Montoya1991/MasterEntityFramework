using Dominio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistencia;
using System;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Se crea y configura el host de la aplicaci�n.
            var hostserver = CreateHostBuilder(args).Build();

            // Se crea un �mbito de servicios para realizar algunas operaciones de inicializaci�n.
            using (var ambiente = hostserver.Services.CreateScope())
            {
                var services = ambiente.ServiceProvider;
                try
                {
                    // Se obtiene el UserManager para administrar usuarios y roles.
                    var userManager = services.GetRequiredService<UserManager<Usuario>>();

                    // Se obtiene el contexto de la base de datos.
                    var context = services.GetRequiredService<CursosOnlineContext>();

                    // Se aplica migraci�n para actualizar la base de datos a la �ltima versi�n.
                    context.Database.Migrate();

                    // Se insertan datos de prueba en la base de datos.
                    DataPrueba.InsertarData(context, userManager).Wait();
                }
                catch (Exception e)
                {
                    // En caso de errores, se registra el error en el registro (logging).
                    var logging = services.GetRequiredService<ILogger<Program>>();
                    logging.LogError(e, "Ocurri� un error en la migraci�n");
                }

                // Se inicia la ejecuci�n del host de la aplicaci�n.
                hostserver.Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
