using System;
using System.ComponentModel.DataAnnotations.Schema;

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
