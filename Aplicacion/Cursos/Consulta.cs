using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Dominio;
using System.Threading.Tasks;
using System.Threading;
using Persistencia;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        // Definición de la solicitud para obtener una lista de cursos.
        public class ListaCursos : IRequest<List<CursoDto>>
        {

        }

        // Manejador que procesa la solicitud para obtener una lista de cursos.
        public class Manejador : IRequestHandler<ListaCursos, List<CursoDto>>
        {
            // Declaración de un campo privado que almacena el contexto de la base de datos.
            private readonly CursosOnlineContext _context;

            // Declaración de un campo privado que almacena una instancia de IMapper.
            private readonly IMapper _mapper;

            // Constructor de la clase.
            public Manejador(CursosOnlineContext context, IMapper mapper)
            {
                // Asigna el contexto de la base de datos inyectado al campo "_context".
                _context = context;

                // Asigna la instancia de IMapper inyectada al campo "_mapper".
                _mapper = mapper;
            }

            // Método para manejar la solicitud y obtener una lista de cursos.
            public async Task<List<CursoDto>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                // Se consulta la base de datos para obtener la lista de cursos.
                var cursos = await _context.Curso
                    // Se incluyen los comentarios relacionados con cada curso.
                    .Include(x => x.ComentarioLista)
                    // Se incluye el precio de promoción del curso, si está disponible.
                    .Include(x => x.PrecioPromocion)
                    // Se incluye la relación con los instructores, y se obtiene información sobre cada instructor.
                    .Include(x => x.InstructorLink).ThenInclude(x => x.Instructor)
                    // Se carga la lista de cursos en una colección, marcando el final de la consulta.
                    .ToListAsync();

                // Se mapean los objetos de tipo "Curso" a objetos de tipo "CursoDto" utilizando el mapeador (_mapper).
                var cursoDto = _mapper.Map<List<Curso>, List<CursoDto>>(cursos);

                // Se devuelve la lista de cursos en el formato de objetos "CursoDto".
                return cursoDto;
            }

        }
    }
}
