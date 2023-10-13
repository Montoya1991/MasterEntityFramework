using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore.Internal;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<Curso>
        {
            public int Id { get; set; }
        }



        public class Manejador : IRequestHandler<CursoUnico, Curso>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }

            public async Task<Curso> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso.FindAsync(request.Id);
                if (curso == null)
                {
                    // Si el curso no existe, se lanza una excepción.
                    //throw new Exception("El curso no se puede eliminar");

                    // Si el curso no se encuentra en la base de datos, se lanza una excepción específica.
                    throw new ManejadorException(HttpStatusCode.NotFound, new { mensaje = "No se encontro el curso" });
                }

                return curso;
            }
        }
    }
}
