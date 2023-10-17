using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Aplicacion.ManejadorError
{
    public class ManejadorException : Exception
    {
        // Propiedad que almacena el código de estado HTTP asociado a la excepción.
        public HttpStatusCode Codigo { get; }

        // Propiedad que permite proporcionar detalles específicos sobre el error.
        public object Errores { get; }

        // Constructor de la clase.
        // Recibe un código de estado HTTP y un objeto de errores (opcional).
        public ManejadorException(HttpStatusCode codigo, object errores = null)
        {
            // Asigna el código de estado HTTP a la propiedad "Codigo".
            Codigo = codigo;

            // Asigna el objeto de errores (si se proporciona) a la propiedad "Errores".
            Errores = errores;
        }
    }
}
