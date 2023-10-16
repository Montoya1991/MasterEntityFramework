using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Cursos
{
    public class CursoInstructorDto
    {
        public Guid InstructorId { get; set; }
        // Identificador único del instructor asociado al curso.
        public Guid CursoId { get; set; }
        // Identificador único del curso asociado al instructor.
    }
}
