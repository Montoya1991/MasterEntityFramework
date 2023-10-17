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
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnlineContext _context;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly UserManager<Usuario> _userManager;

            public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador) 
            {
                _userManager = userManager;
                _context = context;
                _jwtGenerador = jwtGenerador;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var existe =await _context.Users.Where(x => x.Email == request.Email).AnyAsync();
                if (existe)
                {
                    throw new ManejadorException(HttpStatusCode.BadRequest, new {mensaje = "El email ya esta registrado"});
                }
                var existeUsername = await _context.Users.Where(x => x.UserName == request.Username).AnyAsync();
                if (existeUsername)
                {
                    throw new ManejadorException(HttpStatusCode.BadRequest, new { mensaje = "El Usuario ya esta registrado" });
                }
                var usuario = new Usuario
                {
                    NombreCompleto = request.Nombre + " " + request.Apellidos,
                    Email = request.Email,
                    UserName = request.Username,

                };
                var resultado = await _userManager.CreateAsync(usuario, request.Password);
                if(resultado.Succeeded)
                {
                    return new UsuarioData
                    {
                        NombreCompleto = usuario.NombreCompleto,
                        Token = _jwtGenerador.CrearToken(usuario),
                        Username = usuario.UserName,
                        Email = usuario.Email
                    };
                }
                throw new Exception("No se pudo agregar nuevo usuario");

            }
        }

    }
}
