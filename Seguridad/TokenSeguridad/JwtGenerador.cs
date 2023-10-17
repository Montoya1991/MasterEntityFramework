using Aplicacion.Contratos;
using Dominio;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Seguridad.TokenSeguridad
{
    public class JwtGenerador : IJwtGenerador
    {
        public string CrearToken(Usuario usuario)
        {
            // Paso 1: Crear una lista de reclamaciones (claims) para el token
            var claims = new List<Claim>
            {
                // En este caso, se agrega una reclamación "NameId" con el nombre de usuario del usuario.
                new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName)
            };

            // Paso 2: Crear una clave de seguridad para firmar el token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));

            // Paso 3: Crear credenciales para la firma del token
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Paso 4: Configurar la descripción del token, incluyendo su duración y las credenciales
            var tokenDescripcion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30), // El token expira en 30 días
                SigningCredentials = credenciales,
            };

            // Paso 5: Crear un manejador de tokens JWT
            var tokenManejador = new JwtSecurityTokenHandler();

            // Paso 6: Crear el token utilizando la descripción
            var token = tokenManejador.CreateToken(tokenDescripcion);

            // Paso 7: Escribir el token como una cadena y retornarlo
            return tokenManejador.WriteToken(token);
        }
    }
}
