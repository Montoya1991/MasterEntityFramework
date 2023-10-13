using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class CursoInstructor
    {
        public Guid InstructorId { get; set; }
        // Identificador único del instructor asociado al curso.

        public Guid CursoId { get; set; }
        // Identificador único del curso asociado al instructor.

        public Curso Curso { get; set; }
        // Una referencia a la clase Curso, estableciendo una relación uno a uno con el curso asociado.

        public Instructor Instructor { get; set; }
        // Una referencia a la clase Instructor, estableciendo una relación uno a uno con el instructor asociado.


        // Las propiedades InstructorId y CursoId se utilizan para establecer relaciones entre instructores y cursos.
        // Cada instancia de esta clase representa una asociación entre un instructor y un curso específico.
        // Por ejemplo, si un curso tiene múltiples instructores o si un instructor está asociado con varios cursos,
        // se crearán múltiples instancias de CursoInstructor para registrar esas relaciones en la base de datos.
    }
}
