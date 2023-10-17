using Aplicacion.Contratos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Seguridad.TokenSeguridad
{
    public class UsuarioSesion : IUsuarioSesion
    {
        // Inyección de dependencia del IHttpContextAccessor para acceder al contexto HTTP actual.
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioSesion(IHttpContextAccessor httpContextAccessor)
        {
            // El constructor recibe el IHttpContextAccessor como una dependencia.
            _httpContextAccessor = httpContextAccessor;
        }

        public string OptenerUsuarioSesion()
        {
            // El método OptenerUsuarioSesion se utiliza para obtener el nombre de usuario de la sesión actual.

            // Se obtiene el nombre de usuario desde las reclamaciones del contexto HTTP actual.
            var userName = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            // Se retorna el nombre de usuario.
            return userName;
        }
    }
}
