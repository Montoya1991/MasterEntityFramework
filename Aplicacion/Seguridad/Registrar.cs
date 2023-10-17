using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class Registrar
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            // Esta clase define un comando para registrar un nuevo usuario.
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>
        {
            public EjecutaValidador()
            {
                // Esta clase define reglas de validación para el comando de registro.
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            // Este manejador procesa el registro de un nuevo usuario.

            private readonly CursosOnlineContext _context;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly UserManager<Usuario> _userManager;

            public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador)
            {
                // El constructor recibe el contexto de la base de datos, el administrador de usuarios y el generador de tokens JWT.
                _userManager = userManager;
                _context = context;
                _jwtGenerador = jwtGenerador;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // Este método maneja la lógica de registro del nuevo usuario.

                // Comprueba si el email ya está registrado en la base de datos.
                var existe = await _context.Users.Where(x => x.Email == request.Email).AnyAsync();
                if (existe)
                {
                    // Si el email ya existe, se lanza una excepción de BadRequest con un mensaje.
                    throw new ManejadorException(HttpStatusCode.BadRequest, new { mensaje = "El email ya está registrado" });
                }

                // Comprueba si el nombre de usuario ya está registrado en la base de datos.
                var existeUsername = await _context.Users.Where(x => x.UserName == request.Username).AnyAsync();
                if (existeUsername)
                {
                    // Si el nombre de usuario ya existe, se lanza una excepción de BadRequest con un mensaje.
                    throw new ManejadorException(HttpStatusCode.BadRequest, new { mensaje = "El usuario ya está registrado" });
                }

                // Si el email y el nombre de usuario no existen en la base de datos, se crea un nuevo objeto Usuario.
                var usuario = new Usuario
                {
                    NombreCompleto = request.Nombre + " " + request.Apellidos,
                    Email = request.Email,
                    UserName = request.Username,
                };

                // Se intenta crear el usuario utilizando el administrador de usuarios.
                var resultado = await _userManager.CreateAsync(usuario, request.Password);
                if (resultado.Succeeded)
                {
                    // Si se crea con éxito, se genera un token JWT y se devuelve la información del usuario.
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = _jwtGenerador.CrearToken(usuario),
                        Username = usuario.UserName,
                        Email = usuario.Email
                    };
                }

                // Si no se puede crear el usuario, se lanza una excepción genérica.
                throw new Exception("No se pudo agregar un nuevo usuario");
            }
        }
    }
}
