using Dominio;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    public class DataPrueba
    {
        public static async Task InsertarData(CursosOnlineContext context, UserManager<Usuario> usuarioManager)
        {
            if (!usuarioManager.Users.Any())
            {
                var usuario = new Usuario {NombreCompleto = "David Montoya", UserName="Montoya", Email="jdmontoya0491@hotmail.com"};
                usuarioManager.CreateAsync(usuario, "Dante1991+");
            }
        }
    }
}
