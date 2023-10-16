using Aplicacion.Cursos;
using AutoMapper;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aplicacion
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Curso, CursoDto>()
            .ForMember(x => x.Instructores, y => y.MapFrom(z => z.InstructorLink.Select(a => a.Instructor).ToList()))
            .ForMember(x => x.Comentarios, y => y.MapFrom(z => z.ComentarioLista))
            .ForMember(x=> x.Precio , y => y.MapFrom(z=> z.PrecioPromocion));
            CreateMap<CursoInstructor, CursoInstructorDto>();
            CreateMap<Instructor, InstructorDto>();
            CreateMap<Precio, PrecioDto>();
            CreateMap<Comentario, ComentarioDto>();
        }
    }
}
