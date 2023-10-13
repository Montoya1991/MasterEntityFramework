using System;
using System.Collections.Generic;

namespace Dominio
{
    public class Instructor
    {
        public Guid InstructorId { get; set; }
        // Identificador único del instructor. (llave primaria)

        public string Nombre { get; set; }
        // El nombre del instructor.

        public string Apellidos { get; set; }
        // Los apellidos del instructor.

        public string Grado { get; set; }
        // El grado o título del instructor.

        public byte[] FotoPerfil { get; set; }
        // Una representación de la foto de perfil del instructor en forma de bytes.

        public ICollection<CursoInstructor> InstructorsLink { get; set; }
        // Una colección de enlaces entre cursos e instructores.
        // Esto puede representar una relación muchos a muchos entre cursos e instructores.
    }
}
