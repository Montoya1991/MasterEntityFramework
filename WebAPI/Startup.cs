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
using Aplicacion.Contratos;
using Seguridad.TokenSeguridad;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AutoMapper;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Este método se llama durante la configuración de servicios.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuración de servicios

            // Se configura la conexión a la base de datos a través de Entity Framework Core.
            services.AddDbContext<CursosOnlineContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            // Se agregan los servicios de MediatR para el manejo de comandos y consultas.
            services.AddMediatR(typeof(Consulta.Manejador).Assembly);

            // Se configuran los controladores con FluentValidation para la validación de modelos.
            services.AddControllers(opt =>
            {
                // Se establece una política de autorización para requerir usuarios autenticados.
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            }).AddFluentValidation(cgf => cgf.RegisterValidatorsFromAssemblyContaining<Nuevo>());

            // Configuración de autenticación y autorización

            // Se configura la autenticación de usuarios con IdentityCore y Entity Framework.
            services.AddIdentityCore<Usuario>().AddEntityFrameworkStores<CursosOnlineContext>()
                .AddSignInManager<SignInManager<Usuario>>();

            // Se agrega la autenticación basada en tokens JWT.
            // Se crea una clave simétrica a partir de una cadena (secreta) codificada en UTF-8.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));

            // Se agrega la autenticación con el esquema JwtBearer (JWT) al servicio de autenticación.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                // Configuración de parámetros de validación del token JWT.
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    // Se especifica que se debe validar la firma del token con la clave creada anteriormente.
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,

                    // No se valida la audiencia (quién debe recibir el token).
                    ValidateAudience = false,

                    // No se valida el emisor (quién emitió el token).
                    ValidateIssuer = false
                };
            });

            // Se registran servicios personalizados, como IJwtGenerador e IUsuarioSesion.
            services.AddScoped<IJwtGenerador, JwtGenerador>();
            services.AddScoped<IUsuarioSesion, UsuarioSesion>();

            // Se configura AutoMapper para mapeo de objetos.
            services.AddAutoMapper(typeof(Consulta.Manejador));
        }

        // Este método se llama durante la configuración del pipeline de solicitud HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Se utiliza un middleware personalizado (ManejadorErrorMiddleware) para manejar errores.

            if (env.IsDevelopment())
            {
                // En entorno de desarrollo, se puede habilitar una página de errores detallados.
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                // En entornos de producción, se puede redirigir a una página de error personalizada.
                //app.UseExceptionHandler("/Home/Error");
            }

            // Se habilita la autenticación.
            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseRouting();

            // Se habilita la autorización.
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
