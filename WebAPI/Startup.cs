using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Aplicacion.Cursos;
using FluentValidation.AspNetCore;
using WebAPI.Middleware;
using Dominio;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuraci�n de servicios

            // Se agrega la configuraci�n de Entity Framework Core para la conexi�n a la base de datos.
            services.AddDbContext<CursosOnlineContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            // Se agregan los servicios de MediatR para el manejo de comandos y consultas.
            services.AddMediatR(typeof(Consulta.Manejador).Assembly);
            // Se configuran los controladores con FluentValidation para la validaci�n de modelos.
            services.AddControllers().AddFluentValidation(cgf => cgf.RegisterValidatorsFromAssemblyContaining<Nuevo>());

            // Configuraci�n de autenticaci�n y autorizaci�n
            var builder = services.AddIdentityCore<Usuario>();
            // Crear un IdentityBuilder para personalizar la configuraci�n de autenticaci�n
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            // Agregar el almacenamiento de Entity Framework para administrar usuarios y roles
            identityBuilder.AddEntityFrameworkStores<CursosOnlineContext>();
            // Agregar el administrador de inicio de sesi�n personalizado para el modelo de usuario "Usuario"
            identityBuilder.AddSignInManager<SignInManager<Usuario>>();
            
            services.TryAddSingleton<ISystemClock, SystemClock>();


            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ManejadorErrorMiddleware>();

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
