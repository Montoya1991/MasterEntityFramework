using Dominio;
using Microsoft.AspNetCore.Mvc;
using Persistencia;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController1 : ControllerBase
    {
        private readonly CursosOnlineContext context1;

        public HomeController1(CursosOnlineContext context)
        {
            this.context1 = context;
            // Constructor que recibe el contexto de la base de datos a través de la inyección de dependencias
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            string[] nombres = new[] { "Fabian", "Rolando", "Maria" };
            return nombres;
        }
        [HttpGet]
        public IEnumerable<Curso> GetCursos()
        {
            return context1.Curso.ToList();
        }
    }
}
