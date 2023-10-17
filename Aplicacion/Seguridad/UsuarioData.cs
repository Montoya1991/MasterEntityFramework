using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Seguridad
{
    public class UsuarioData
    {
        // Propiedades para almacenar información sobre un usuario.

        public string NombreCompleto { get; set; }
        // Nombre completo del usuario.

        public string Token { get; set; }
        // Token de seguridad que puede utilizarse para autenticación.

        public string Email { get; set; }
        // Dirección de correo electrónico del usuario.

        public string Username { get; set; }
        // Nombre de usuario del usuario.

        public string Imagen { get; set; }
        // Enlace a la imagen de perfil del usuario (puede ser nulo).
    }
}
