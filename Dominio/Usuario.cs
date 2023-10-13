using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Usuario : IdentityUser //esta clase contiene todas las propiedades que puede tener un usuario
    {
        public string NombreCompleto { get; set; }
    }
}
