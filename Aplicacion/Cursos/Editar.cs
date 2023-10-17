using Aplicacion.ManejadorError;
using Dominio;
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
            // Declara un campo privado llamado "_context" de tipo "CursosOnlineContext".
            // Este campo se utiliza para acceder a la base de datos.
            private readonly CursosOnlineContext _context;

            // Constructor de la clase "Manejador" que recibe una instancia de "CursosOnlineContext".
            // Este constructor se utiliza para inyectar la dependencia de la base de datos en el manejador.
            public Manejador(CursosOnlineContext context)
            {
                // Asigna la instancia de "CursosOnlineContext" proporcionada como argumento al campo privado "_context".
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
                // Si se encuentra un precio existente en la base de datos para el curso:
                if (precioEntidad != null)
                {
                    // - Se actualiza la propiedad "Promoción" con el valor de "request.Promocion" si no es nulo, de lo contrario, se mantiene el valor actual.
                    // - Se actualiza la propiedad "PrecioActual" con el valor de "request.precio" si no es nulo, de lo contrario, se mantiene el valor actual.
                    precioEntidad.Promoción = request.Promocion ?? precioEntidad.Promoción;
                    precioEntidad.PrecioActual = request.precio ?? precioEntidad.PrecioActual;
                }
                else
                {
                    // Si no se encuentra un precio existente en la base de datos para el curso:
                    // - Se crea una nueva entidad "Precio" con los valores proporcionados.
                    precioEntidad = new Precio
                    {
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.precio ?? 0,
                        Promoción = request.Promocion ?? 0,
                        CursoId = curso.CursoId,
                    };
                    // Se agrega la nueva entidad "Precio" al contexto de la base de datos para su posterior guardado.
                    await _context.Precio.AddAsync(precioEntidad);
                }

                if (request.ListaInstructor != null)
                {
                    if (request.ListaInstructor.Count > 0)
                    {
                        // Si se proporciona una lista de instructores en la solicitud:
                        // - Se obtiene la lista de instructores asociados al curso desde la base de datos.
                        var instructoresBD = _context.CursoInstructor.Where(x => x.CursoId == request.CursoId).ToList();

                        // - Se eliminan los registros existentes de la tabla "CursoInstructor" asociados al curso.
                        foreach (var instructorEliminarid in instructoresBD)
                        {
                            _context.CursoInstructor.Remove(instructorEliminarid);
                        }

                        // - Se recorre la lista de instructores proporcionada en la solicitud y se crean nuevos registros en "CursoInstructor" para asociarlos al curso.
                        // Se recorre la lista de instructores proporcionada en la solicitud.
                        foreach (var id in request.ListaInstructor)
                        {
                            // Se crea un nuevo registro CursoInstructor para asociar un instructor a este curso.
                            var nuevoInstructor = new CursoInstructor
                            {
                                CursoId = request.CursoId, // Se asigna el ID del curso de la solicitud.
                                InstructorId = id // Se asigna el ID del instructor actual en el bucle.
                            };

                            // Se agrega el nuevo registro CursoInstructor al contexto de la base de datos.
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
