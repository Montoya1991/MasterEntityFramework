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

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        // Definición de la solicitud para obtener una lista de cursos.
        public class ListaCursos : IRequest<List<Curso>>
        {

        }

        // Manejador que procesa la solicitud para obtener una lista de cursos.
        public class Manejador : IRequestHandler<ListaCursos, List<Curso>>
        {
            private readonly CursosOnlineContext _context;

            // Constructor que recibe una instancia de CursosOnlineContext para acceder a la base de datos.
            // El parametro context trae la informacion de todas las tablas de la base de datos
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }

            // Método para manejar la solicitud y obtener una lista de cursos.
            public async Task<List<Curso>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                // Se consulta la base de datos para obtener la lista de cursos.
                var cursos = await _context.Curso.ToListAsync();
                return cursos;
            }
        }
    }
}
