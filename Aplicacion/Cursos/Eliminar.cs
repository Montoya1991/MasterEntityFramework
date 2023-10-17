using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using System.Net;
using System.Linq;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        // Definición de la solicitud para eliminar un curso por su identificador.
        public class Ejecuta : IRequest
        {
            public Guid Id { get; set; }
            // Propiedad que almacena el identificador del curso que se desea eliminar.
        }

        // Manejador que procesa la solicitud para eliminar un curso.
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;

            // Constructor que recibe una instancia de CursosOnlineContext para acceder a la base de datos.
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }

            // Método para manejar la solicitud y eliminar un curso por su identificador.
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var instructoresBD = _context.CursoInstructor.Where(x => x.CursoId == request.Id);
                foreach(var instructor in instructoresBD)
                {
                    _context.CursoInstructor.Remove(instructor);
                }
                var comentariosDB = _context.Comentario.Where(x => x.CursoId == request.Id);
                foreach (var comentario in comentariosDB)
                {
                    _context.Comentario.Remove(comentario);
                }

                var precioDB = _context.Precio.Where(x=> x.CursoId == request.Id).FirstOrDefault();
                if(precioDB!=null)
                {
                    _context.Precio.Remove(precioDB);
                }
                // Se busca el curso en la base de datos por su identificador.
                var curso = await _context.Curso.FindAsync(request.Id);
                if (curso == null)
                {
                    // Si el curso no existe, se lanza una excepción.
                    //throw new Exception("El curso no se puede eliminar");

                    // Si el curso no se encuentra en la base de datos, se lanza una excepción específica.
                    throw new ManejadorException(HttpStatusCode.NotFound, new { mensaje = "No se encontro el curso" }); 
                }

                // Se utiliza el método Remove para marcar el curso como eliminado en el contexto.
                _context.Remove(curso);

                // Se guarda la información actualizada en la base de datos.
                var resultado = await _context.SaveChangesAsync();
                if (resultado > 0)
                {
                    // Si la operación de eliminación tiene éxito, se retorna Unit.Value.
                    //Unit.Value se utiliza para representar que la eliminación del curso
                    //se ha llevado a cabo correctamente,
                    return Unit.Value;
                }

                // Si no se pueden guardar los cambios, se lanza una excepción.
                throw new Exception("No se pudieron guardar los cambios");
            }
        }
    }
}
