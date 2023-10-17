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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsuarioSesion(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string OptenerUsuarioSesion()
        {
            var userName = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return userName;
        }
    }
}
