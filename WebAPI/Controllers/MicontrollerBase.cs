using MediatR; // Se importa el espacio de nombres de MediatR.
using Microsoft.AspNetCore.Mvc; // Se importa el espacio de nombres de ASP.NET Core MVC.
using Microsoft.Extensions.DependencyInjection; // Se importa el espacio de nombres de inyección de dependencias.

namespace WebAPI.Controllers
{
    [Route("api/[controller]")] // Ruta base para las acciones del controlador.
    [ApiController] // Atributo que indica que esta clase es un controlador de API.
    public class MicontrollerBase : ControllerBase
    {
        private IMediator _mediator; // Variable privada para almacenar el servicio IMediator.

        // Propiedad protegida que proporciona acceso al servicio IMediator.
        // Permite acceder al servicio a través del servicio de inyección de dependencias de ASP.NET Core.
        protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());
    }
}
