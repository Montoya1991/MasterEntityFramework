using System;

namespace Dominio
{
    public class Comentario
    {
        public Guid ComentarioId { get; set; } //Tipo de dato para llaves primarias
        // Identificador único del comentario.(llave primaria)

        public string Alumno { get; set; }
        // El nombre del alumno que hizo el comentario.

        public int Puntaje { get; set; }
        // El puntaje asignado al curso en este comentario.

        public string ComentarioTexto { get; set; }
        // El texto del comentario proporcionado por el alumno.

        public Guid CursoId { get; set; }
        // El identificador del curso al que se refiere este comentario.

        public Curso Curso { get; set; }
        // Una referencia a la clase Curso, estableciendo una relación uno a uno con el curso comentado.
        // El CursoId se utiliza como clave foránea para definir esta relación.
    }
}
