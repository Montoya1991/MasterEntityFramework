using Aplicacion.Contratos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class UsuarioActual
    {
        public class Ejecutar : IRequest<UsuarioData>
        {
            // Esta clase define un comando para obtener la información del usuario actual.
        }

        public class Manejador : IRequestHandler<Ejecutar, UsuarioData>
        {
            // Este manejador se encarga de obtener la información del usuario actual.

            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly IUsuarioSesion _usuarioSesion;

            public Manejador(UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IUsuarioSesion usuarioSesion)
            {
                // El constructor recibe el administrador de usuarios, el generador de tokens JWT y el servicio de usuario en sesión.
                _jwtGenerador = jwtGenerador;
                _userManager = userManager;
                _usuarioSesion = usuarioSesion;
            }

            public async Task<UsuarioData> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                // Este método maneja la lógica para obtener la información del usuario actual.

                // Se obtiene el nombre de usuario actual a través del servicio de usuario en sesión.
                var usuario = await _userManager.FindByNameAsync(_usuarioSesion.OptenerUsuarioSesion());

                // Se crea un objeto de UsuarioData con la información del usuario actual.
                return new UsuarioData
                {
                    NombreCompleto = usuario.NombreCompleto,
                    Username = usuario.UserName,
                    Token = _jwtGenerador.CrearToken(usuario),
                    Email = usuario.Email,
                    Imagen = null
                };
            }
        }
    }
}
