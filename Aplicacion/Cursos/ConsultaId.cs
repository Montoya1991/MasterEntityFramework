﻿using Aplicacion.ManejadorError;
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
        public class CursoUnico : IRequest<CursoDto>
        {
            public Guid Id { get; set; }
        }



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
                var curso = await _context.Curso
                    .Include(x => x.ComentarioLista)
                    .Include(x => x.PrecioPromocion)
                    .Include(x => x.InstructorLink).ThenInclude(y => y.Instructor).FirstOrDefaultAsync(a => a.CursoId == request.Id);
                if (curso == null)
                {
                    // Si el curso no existe, se lanza una excepción.
                    //throw new Exception("El curso no se puede eliminar");

                    // Si el curso no se encuentra en la base de datos, se lanza una excepción específica.
                    throw new ManejadorException(HttpStatusCode.NotFound, new { mensaje = "No se encontro el curso" });
                }
                var cursoDto = _mapper.Map<Curso, CursoDto>(curso);
                return cursoDto;
            }
        }
    }
}
