using Dominio; // Importa el espacio de nombres Dominio.
using Microsoft.AspNetCore.Identity; // Importa el espacio de nombres Identity.
using Microsoft.EntityFrameworkCore.Internal; // Importa el espacio de nombres EntityFrameworkCore.Internal.
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    public class DataPrueba
    {
        // Método estático que permite insertar datos de prueba en la base de datos.
        public static async Task InsertarData(CursosOnlineContext context, UserManager<Usuario> usuarioManager)
        {
            // "CursosOnlineContext context" es el contexto de la base de datos que permite realizar operaciones en ella.
            // "UserManager<Usuario> usuarioManager" es el administrador de usuarios de Identity que se utiliza para gestionar usuarios.

            // Verificar si no hay usuarios en la base de datos
            if (!usuarioManager.Users.Any())
            {
                // Crear un nuevo usuario de prueba
                var usuario = new Usuario
                {
                    NombreCompleto = "David Montoya",
                    UserName = "Montoya",
                    Email = "jdmontoya0491@hotmail.com"
                };

                // Crear el usuario asincrónicamente con una contraseña
                // Utilizamos el método "CreateAsync" del administrador de usuarios para crear el usuario en la base de datos.
                // Esto se hace de forma asincrónica, lo que significa que no bloqueará la ejecución del programa mientras se completa la operación.
                await usuarioManager.CreateAsync(usuario, "Dante1991+");
            }
        }

    }
}
