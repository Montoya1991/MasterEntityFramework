using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Dominio
{
    public class Usuario : IdentityUser
    {
        // Hereda de IdentityUser proporcionado por ASP.NET Identity para la gestión de usuarios.

        public string NombreCompleto { get; set; }
        // Propiedad adicional para almacenar el nombre completo del usuario en la aplicación.
    }
}
