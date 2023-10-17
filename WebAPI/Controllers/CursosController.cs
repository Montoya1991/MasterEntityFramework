using Aplicacion.Cursos; // Se importa el espacio de nombres que contiene las clases relacionadas con cursos.
using Dominio; // Se importa el espacio de nombres que contiene las clases de dominio.
using MediatR; // Se importa el espacio de nombres de MediatR para la gestión de solicitudes y manejadores.
using Microsoft.AspNetCore.Authorization; // Se importa el espacio de nombres para la autorización.
using Microsoft.AspNetCore.Mvc; // Se importa el espacio de nombres de ASP.NET Core MVC.
using System; // Se importa el espacio de nombres para tipos de datos básicos.
using System.Collections.Generic; // Se importa el espacio de nombres para listas y colecciones.
using System.Threading.Tasks; // Se importa el espacio de nombres para tareas asincrónicas.

namespace WebAPI.Controllers
{
    [ApiController] // Atributo que indica que esta clase es un controlador de API.
    [Route("api/[controller]")] // Ruta base de la API para este controlador, como "api/Cursos".
    public class CursosController : MicontrollerBase
    {
        // Acción HTTP GET para obtener una lista de cursos. https://localhost:44378/api/Cursos/
        [HttpGet]
        public async Task<ActionResult<List<CursoDto>>> Get()
        {
            // Se envía una solicitud Consulta.ListaCursos al manejador y se retorna el resultado como una lista de cursos.
            return await Mediator.Send(new Consulta.ListaCursos());
        }

        // Acción HTTP GET para obtener los detalles de un curso por su identificador. https://localhost:44378/api/Cursos/1
        // El número al final del enlace equivale al ID del curso que se desea obtener.
        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDto>> Detalle(Guid id)
        {
            // Se envía una solicitud ConsultaId.CursoUnico al manejador con el identificador y se retorna el resultado como un curso.
            return await Mediator.Send(new ConsultaId.CursoUnico { Id = id });
        }

        // Acción HTTP POST para crear un nuevo curso. https://localhost:44378/api/Cursos
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            // Se envía una solicitud Nuevo.Ejecuta al manejador con los datos del nuevo curso y se retorna el resultado como Unit (indicando que la operación se realizó con éxito).
            return await Mediator.Send(data);
        }

        // Acción HTTP PUT para editar un curso por su identificador. https://localhost:44378/api/Cursos/6
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Editar(Guid id, Editar.Ejecuta data)
        {
            data.CursoId = id;
            // Se envía una solicitud Editar.Ejecuta al manejador con el identificador y los datos de edición del curso, y se retorna el resultado como Unit.
            return await Mediator.Send(data);
        }

        // Acción HTTP DELETE para eliminar un curso por su identificador. https://localhost:44378/api/Cursos/8
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id)
        {
            // Se envía una solicitud Eliminar.Ejecuta al manejador con el identificador y se retorna el resultado como Unit.
            return await Mediator.Send(new Eliminar.Ejecuta { Id = id });
        }
    }
}
