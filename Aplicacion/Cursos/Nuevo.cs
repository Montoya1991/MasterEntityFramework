using Dominio;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Titulo { get; set; } // Título del curso
            public string Descripcion { get; set; } // Descripción del curso
            public DateTime? FechaPublicacion { get; set; } // Fecha de publicación del curso
            public List<Guid> ListaInstructor { get; set; } // Lista de instructores asociados al curso
            public decimal Precio { get; set; } // Precio actual del curso
            public decimal Promocion { get; set; } // Precio de promoción del curso
        }

        public class EjecutarValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutarValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;

            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                Guid _cursoId = Guid.NewGuid();

                // Creación de un nuevo curso con los valores proporcionados en la solicitud.
                var curso = new Curso
                {
                    CursoId = _cursoId,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion,
                };

                // Agregar el nuevo curso al contexto (_context).
                _context.Curso.Add(curso);

                // Lógica para asociar instructores al curso.
                if (request.ListaInstructor != null)
                {
                    foreach (var id in request.ListaInstructor)
                    {
                        var cursoInstructor = new CursoInstructor
                        {
                            CursoId = _cursoId,
                            InstructorId = id,
                        };
                        _context.CursoInstructor.Add(cursoInstructor);
                    }
                }

                // Lógica para gestionar el precio del curso.
                var precioEntidad = new Precio
                {
                    CursoId = _cursoId,
                    PrecioActual = request.Precio,
                    Promoción = request.Promocion,
                    PrecioId = Guid.NewGuid(),
                };
                _context.Precio.Add(precioEntidad);

                // Guardar los cambios en la base de datos.
                var valor = await _context.SaveChangesAsync();

                // Si el valor devuelto es mayor que cero, se realizó la inserción exitosamente.
                if (valor > 0)
                {
                    return Unit.Value;
                }

                // Si no se pueden guardar los cambios, se lanza una excepción.
                throw new Exception("No se pudo insertar el curso");
            }
        }
    }
}
