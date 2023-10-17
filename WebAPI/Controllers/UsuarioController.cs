using Aplicacion.Seguridad; // Se importa el espacio de nombres que contiene las clases de seguridad de la aplicación.
using Dominio; // Se importa el espacio de nombres que contiene las clases de dominio.
using Microsoft.AspNetCore.Authorization; // Se importa el espacio de nombres para la autorización.
using Microsoft.AspNetCore.Mvc; // Se importa el espacio de nombres de ASP.NET Core MVC.
using System.Threading.Tasks; // Se importa el espacio de nombres para tareas asincrónicas.

namespace WebAPI.Controllers
{
    [AllowAnonymous] // Atributo que permite el acceso a estas acciones sin autorización.
    public class UsuarioController : MicontrollerBase
    {
        // Acción HTTP POST para autenticar un usuario.
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>> Login(Login.Ejecuta parametros)
        {
            // Se envía una solicitud de autenticación al manejador y se retorna el resultado.
            return await Mediator.Send(parametros);
        }

        // Acción HTTP POST para registrar un nuevo usuario.
        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioData>> Registra(Registrar.Ejecuta parametros)
        {
            // Se envía una solicitud de registro al manejador y se retorna el resultado.
            return await Mediator.Send(parametros);
        }

        // Acción HTTP GET para devolver información de usuario actual.
        [HttpGet]
        public async Task<ActionResult<UsuarioData>> DevolverUsuario()
        {
            // Se envía una solicitud para obtener la información del usuario actual y se retorna el resultado.
            return await Mediator.Send(new UsuarioActual.Ejecutar());
        }
    }
}
