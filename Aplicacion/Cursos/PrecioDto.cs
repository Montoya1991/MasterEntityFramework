using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Aplicacion.Cursos
{
    public class PrecioDto
    {
        public Guid PrecioId { get; set; }
        // Identificador único del precio. (llave primaria)
        public decimal PrecioActual { get; set; }
        // El precio actual del curso.
        public decimal Promoción { get; set; }
        // El precio de promoción del curso, si hay alguna promoción.
        public Guid CursoId { get; set; }
        // El identificador del curso al que está asociado este precio.
    }
}
