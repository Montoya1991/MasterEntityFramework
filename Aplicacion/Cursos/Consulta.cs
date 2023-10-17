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
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;
            // Constructor que recibe una instancia de CursosOnlineContext para acceder a la base de datos.
            // El parametro context trae la informacion de todas las tablas de la base de datos
            public Manejador(CursosOnlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            // Método para manejar la solicitud y obtener una lista de cursos.
            public async Task<List<CursoDto>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                // Se consulta la base de datos para obtener la lista de cursos.
                var cursos = await _context.Curso
                .Include(x => x.ComentarioLista)
                .Include(x=> x.PrecioPromocion)
                .Include(x => x.InstructorLink).ThenInclude(x => x.Instructor).ToListAsync();
                var cursoDto = _mapper.Map<List<Curso>, List<CursoDto>>(cursos);
                return cursoDto;
            }
        }
    }
}
