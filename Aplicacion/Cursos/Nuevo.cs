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

            //[Required(ErrorMessage = "Por favor ingrese el titulo")]
            public string Titulo { get; set; } // Título del curso
            public string Descripcion { get; set; } // Descripción del curso
            public DateTime? FechaPublicacion { get; set; } // Fecha de publicación del curso
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
            // llama toda la informacion que esta en la base datos
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //crea un nuevo curso con los valores que llegan a ejecuta que son los request
                var curso = new Curso
                {
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion,
                };
                //agrega el nuevo curso en el _context
                _context.Curso.Add(curso);
                //guarda el context en la base de datos con el nuevo curso
                var valor = await _context.SaveChangesAsync();
                //si devuelve un mayor a cero indica que se realizo un guardado
                if (valor > 0)
                {
                    return Unit.Value;
                }
                //si devuelve cero indica que no se guardo nada
                throw new Exception("No se pudo insertar el curso");
            }
        }
    }
}

