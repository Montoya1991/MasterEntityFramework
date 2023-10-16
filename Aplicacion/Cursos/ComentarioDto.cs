using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Cursos
{
    public class ComentarioDto
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
    }
}
