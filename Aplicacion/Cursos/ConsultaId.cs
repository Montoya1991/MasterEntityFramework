using Aplicacion.ManejadorError;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        // Clase de solicitud para obtener un curso específico.
        public class CursoUnico : IRequest<CursoDto>
        {
            public Guid Id { get; set; }
            // Propiedad que representa el ID del curso que se desea obtener.
        }

        // Manejador que procesa la solicitud para obtener un curso específico.
        public class Manejador : IRequestHandler<CursoUnico, CursoDto>
        {
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;

            public Manejador(CursosOnlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CursoDto> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                // Se realiza la consulta en la base de datos para obtener el curso con el ID proporcionado.
                var curso = await _context.Curso
                    .Include(x => x.ComentarioLista)        // Incluye la colección de comentarios.
                    .Include(x => x.PrecioPromocion)        // Incluye el precio de promoción.
                    .Include(x => x.InstructorLink)         // Incluye la relación con instructores.
                    .ThenInclude(y => y.Instructor)     // Incluye el instructor asociado al curso.
                    .FirstOrDefaultAsync(a => a.CursoId == request.Id);  // Realiza la consulta y filtra por el ID proporcionado.


                // Se verifica si el curso no se encuentra en la base de datos.
                if (curso == null)
                {
                    // En caso de que el curso no exista, se lanza una excepción específica con un código de estado "NotFound" (404).
                    throw new ManejadorException(HttpStatusCode.NotFound, new { mensaje = "No se encontró el curso" });
                }

                // Se mapea el objeto "Curso" al formato "CursoDto" utilizando el mapeador "_mapper".
                var cursoDto = _mapper.Map<Curso, CursoDto>(curso);

                // Se devuelve el curso en el formato de "CursoDto".
                return cursoDto;
            }
        }
    }
}
