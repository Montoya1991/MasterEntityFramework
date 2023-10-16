using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Cursos
{
    public class CursoDto
    {
        public Guid CursoId { get; set; }
        // Identificador único del curso (llave primaria)

        public string Titulo { get; set; }
        // El título del curso.

        public string Descripcion { get; set; }
        // Una breve descripción del curso.

        public DateTime? FechaPublicacion { get; set; }
        // La fecha en que el curso fue publicado. El signo de interrogación (?) indica que puede ser nulo.

        public byte[] FotoPortada { get; set; }

        public ICollection<InstructorDto> Instructores { get; set; }

        public PrecioDto Precio { get; set; }
        public ICollection<ComentarioDto> Comentarios { get; set; }

    }
}
