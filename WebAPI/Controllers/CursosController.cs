using Aplicacion.Cursos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : MicontrollerBase
    {
        //Se reemplazan las siguientes lineas de codigo al heredar de micontrollerbase
        //private readonly IMediator _mediator;

        //// Constructor del controlador que recibe una instancia de IMediator a través de inyección de dependencia.
        //public CursosController(IMediator Mediator)
        //{
        //    _mediator = Mediator;
        //}

        // Acción HTTP GET para obtener una lista de cursos. https://localhost:44378/api/Cursos/
        [HttpGet]
        public async Task<ActionResult<List<Curso>>> Get()
        {
            // Se envía una solicitud Consulta.ListaCursos al manejador y se retorna el resultado como una lista de cursos.
            return await Mediator.Send(new Consulta.ListaCursos());
        }

        // Acción HTTP GET para obtener los detalles de un curso por su identificador. https://localhost:44378/api/Cursos/1
        // el numero al final del link equivale al id del curso que se desea eliminar
        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> Detalle(int id)
        {
            // Se envía una solicitud ConsultaId.CursoUnico al manejador con el identificador y se retorna el resultado como un curso.
            return await Mediator.Send(new ConsultaId.CursoUnico { Id = id });
        }

        // Acción HTTP POST para crear un nuevo curso. https://localhost:44378/api/Cursos
        // mas los parametros del curso que se desea ingresar
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            // Se envía una solicitud Nuevo.Ejecuta al manejador con los datos del nuevo curso
            // y se retorna el resultado como Unit (indicando que la operación se realizó con éxito).
            return await Mediator.Send(data);
        }

        // Acción HTTP PUT para editar un curso por su identificador. https://localhost:44378/api/Cursos/6
        // mas los o el parametro del curso que se desea editar
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Editar(int id, Editar.Ejecuta data)
        {
            data.CursoId = id;
            // Se envía una solicitud Editar.Ejecuta al manejador con el identificador y los datos de edición del curso,
            // y se retorna el resultado como Unit.
            return await Mediator.Send(data);
        }

        // Acción HTTP DELETE para eliminar un curso por su identificador. https://localhost:44378/api/Cursos/8
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(int id)
        {
            // Se envía una solicitud Eliminar.Ejecuta al manejador con el identificador y se retorna el resultado como Unit.
            return await Mediator.Send(new Eliminar.Ejecuta { Id = id });
        }

    }
}
