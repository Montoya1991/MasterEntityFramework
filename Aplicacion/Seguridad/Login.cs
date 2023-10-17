using Aplicacion.ManejadorError;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using FluentValidation;
using Aplicacion.Contratos;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData>
        {
            // La clase "Ejecuta" se utiliza para representar los datos de inicio de sesión.

            // El campo "Email" representa la dirección de correo electrónico del usuario que intenta iniciar sesión.
            public string Email { get; set; }

            // El campo "Password" representa la contraseña del usuario que intenta iniciar sesión.
            public string Password { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            // La clase "EjecutaValidacion" se utiliza para definir reglas de validación para los datos de inicio de sesión.

            // El constructor de la clase, que se ejecuta al crear una instancia de "EjecutaValidacion".
            public EjecutaValidacion()
            {
                // Se definen las reglas de validación para los campos de la clase "Ejecuta".

                // La siguiente línea establece una regla que indica que el campo "Email" no debe estar vacío.
                RuleFor(x => x.Email).NotEmpty();

                // La siguiente línea establece una regla que indica que el campo "Password" no debe estar vacío.
                RuleFor(x => x.Password).NotEmpty();
            }
        }



        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            // La clase "Manejador" implementa "IRequestHandler" para manejar la solicitud de inicio de sesión.

            // Inyectamos el servicio UserManager para administrar usuarios.
            private readonly UserManager<Usuario> _userManager;

            // Inyectamos el servicio SignInManager para administrar el proceso de inicio de sesión.
            private readonly SignInManager<Usuario> _signInManager;

            // Inyectamos el servicio IJwtGenerador para generar tokens de autenticación.
            private readonly IJwtGenerador _jwtGenerador;

            public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador)
            {
                // Constructor de la clase "Manejador" que recibe las dependencias necesarias para el proceso de inicio de sesión.

                // Asignamos el servicio SignInManager inyectado a la variable privada "_signInManager".
                _signInManager = signInManager;

                // Asignamos el servicio UserManager inyectado a la variable privada "_userManager".
                _userManager = userManager;

                // Asignamos el servicio IJwtGenerador inyectado a la variable privada "_jwtGenerador".
                _jwtGenerador = jwtGenerador;

            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                // Método "Handle" para manejar el proceso de inicio de sesión.

                // Se busca al usuario por su dirección de correo electrónico.
                var usuario = await _userManager.FindByEmailAsync(request.Email);

                if (usuario == null)
                {
                    // Si no se encuentra un usuario con el correo electrónico proporcionado, se lanza una excepción de "Unauthorized".
                    throw new ManejadorException(HttpStatusCode.Unauthorized);
                }

                // Se verifica la contraseña proporcionada en la solicitud.
                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

                if (resultado.Succeeded)
                {
                    // Si la verificación de contraseña es exitosa, se devuelve un objeto "UsuarioData" con los detalles del usuario y un token de autenticación.
                    // Creamos un objeto de tipo UsuarioData y asignamos valores a sus propiedades.
                    return new UsuarioData
                    {
                        // NombreCompleto: El nombre completo del usuario obtenido de la propiedad "NombreCompleto" del objeto "usuario".
                        NombreCompleto = usuario.NombreCompleto,
                        // Token: Generamos un token de autenticación utilizando el servicio "_jwtGenerador" y el objeto "usuario".
                        Token = _jwtGenerador.CrearToken(usuario),
                        // Username: El nombre de usuario del usuario obtenido de la propiedad "UserName" del objeto "usuario".
                        Username = usuario.UserName,
                        // Email: El correo electrónico del usuario obtenido de la propiedad "Email" del objeto "usuario".
                        Email = usuario.Email,
                        // Imagen: Establecemos como nulo (null) para la imagen, ya que no se proporciona aquí.
                        Imagen = null
                    };
                }

                // Si la verificación de contraseña no es exitosa, se lanza una excepción de "Unauthorized".
                throw new ManejadorException(HttpStatusCode.Unauthorized);
            }
        }

    }
}
