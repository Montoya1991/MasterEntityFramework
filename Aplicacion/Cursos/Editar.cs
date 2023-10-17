using Aplicacion.ManejadorError;
using Dominio;
//using FluentValidation;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        // Definición de la solicitud para editar un curso.
        public class Ejecuta : IRequest
        {
            public Guid CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }

            public List<Guid> ListaInstructor { get; set; }
            public decimal? precio { get; set; }
            public decimal? Promocion { get; set; }
        }

        // Validación de la solicitud de edición utilizando FluentValidation.
        public class EjecutarValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutarValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
            }
        }

        // Manejador que procesa la solicitud para editar un curso.
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;

            // Constructor que recibe una instancia de CursosOnlineContext para acceder a la base de datos.
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }

            // Método para manejar la solicitud y editar un curso.
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // Se busca el curso en la base de datos por su identificador.
                var curso = await _context.Curso.FindAsync(request.CursoId);
                if (curso == null)
                {
                    //    // Si el curso no se encuentra en la base de datos, se lanza una excepción específica.
                    throw new ManejadorException(HttpStatusCode.NotFound, new { mensaje = "No se encontro el curso" });
                }

                // Se actualizan los datos del curso con los valores proporcionados en la solicitud.
                //El operador ?? se utiliza para asignar el valor de request.Titulo a curso.Titulo
                //si request.Titulo no es nulo (es decir, se proporcionó un nuevo título en la solicitud).
                //Si request.Titulo es nulo, se mantiene el valor actual de curso.Titulo.
                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

                 //Actualizar precio del curso
                 var precioEntidad = _context.Precio.Where(x=> x.CursoId == curso.CursoId).FirstOrDefault();
                if (precioEntidad != null)
                {
                    precioEntidad.Promoción = request.Promocion ?? precioEntidad.Promoción;
                    precioEntidad.PrecioActual = request.precio ?? precioEntidad.PrecioActual;
                }
                else
                {
                    precioEntidad = new Precio
                    {
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.precio ?? 0,
                        Promoción = request.Promocion ?? 0,
                        CursoId = curso.CursoId,
                    };
                    await _context.Precio.AddAsync(precioEntidad);
                }

                if (request.ListaInstructor != null)
                {
                    if(request.ListaInstructor.Count > 0)
                    {
                        var instructoresBD = _context.CursoInstructor.Where(x => x.CursoId == request.CursoId).ToList();
                        foreach (var instructorEliminarid in instructoresBD)
                        {
                            _context.CursoInstructor.Remove(instructorEliminarid);
                        }
                        foreach ( var id in request.ListaInstructor)
                        {
                            var nuevoInstructor = new CursoInstructor
                            {
                                CursoId = request.CursoId,
                                InstructorId = id
                            };
                            _context.CursoInstructor.Add(nuevoInstructor);
                        }
                    }
                }
                // Se guarda la información actualizada en la base de datos.
                var resultado = await _context.SaveChangesAsync();
                if (resultado > 0)
                {
                    // Si la operación de guardado tiene éxito, se retorna Unit.Value.
                    return Unit.Value;
                }

                // Si no se guardan los cambios, se lanza una excepción.
                throw new Exception("No se guardaron los cambios");
            }
        }
    }
}
