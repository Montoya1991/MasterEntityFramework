using Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.Contratos
{
    public interface IJwtGenerador
    {
        // Método que genera un token JWT basado en la información del usuario.
        string CrearToken(Usuario usuario);
    }
}

