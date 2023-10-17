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
        // Una representación de la foto de portada del curso en forma de bytes.

        public ICollection<InstructorDto> Instructores { get; set; }
        // Una colección de objetos `InstructorDto` que representan los instructores asociados al curso.

        public PrecioDto Precio { get; set; }
        // Un objeto `PrecioDto` que contiene información sobre el precio del curso.

        public ICollection<ComentarioDto> Comentarios { get; set; }
        // Una colección de objetos `ComentarioDto` que representan los comentarios asociados al curso.
    }
}
