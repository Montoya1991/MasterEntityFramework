using System;
using System.ComponentModel.DataAnnotations.Schema;
//Se utiliza using System.ComponentModel.DataAnnotations.Schema;
//para aplicar anotaciones de atributos, como [Column],
//Al especificar el tipo de columna, puedes controlar cómo se almacenan los valores en la base de datos,
//lo que es especialmente útil cuando necesitas una precisión específica para los valores decimales
//en la base de datos. En este caso, se ha configurado para que se utilice un tipo de columna decimal
//con una precisión de 18 dígitos y 4 decimales.

namespace Dominio
{
    public class Precio
    {
        public Guid PrecioId { get; set; }
        // Identificador único del precio. (llave primaria)
        [Column(TypeName = "decimal(18,4)")]
        public decimal PrecioActual { get; set; }
        // El precio actual del curso.
        [Column(TypeName = "decimal(18,4)")]
        public decimal Promoción { get; set; }
        // El precio de promoción del curso, si hay alguna promoción.

        public Guid CursoId { get; set; }
        // El identificador del curso al que está asociado este precio.

        public Curso Curso { get; set; }
        // Una referencia a la clase Curso, estableciendo una relación uno a uno con un curso.
        // El CursoId se utiliza como clave foránea para definir esta relación.
    }
}
