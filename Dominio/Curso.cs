using System;
using System.Collections.Generic;

namespace Dominio
{
    public class Curso
    {
        public Guid CursoId { get; set; }
        // Identificador único del curso (llave primaria)

        public string Titulo { get; set; }
        // El título del curso.

        public string Descripcion { get; set; }
        // Una breve descripción del curso.

        public DateTime? FechaPublicacion { get; set; }
        // La fecha en que el curso fue publicado. El signo de interrogación (?) indica que puede ser nulo.

        public Precio PrecioPromocion { get; set; }
        // Una referencia a un objeto Precio que representa el precio de promoción del curso.
        // Esto establece una relación uno a uno con la clase Precio.

        public ICollection<Comentario> ComentarioLista { get; set; }
        // Una colección de comentarios asociados a este curso.
        // Esto establece una relación uno a muchos con la clase Comentario, ya que un curso puede tener varios comentarios.

        public ICollection<CursoInstructor> InstructorLink { get; set; }
        // Una colección de enlaces entre cursos e instructores.
        // Esto puede representar una relación muchos a muchos entre cursos e instructores.
    }
}
