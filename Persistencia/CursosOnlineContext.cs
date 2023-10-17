using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Dominio;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistencia
{
    // Se instala librería Entity Framework Core y Entity Framework Core SQL Server.
    // Ambas librerías deben tener la misma versión.
    public class CursosOnlineContext : IdentityDbContext<Usuario>
    {
        // Constructor de la clase que toma DbContextOptions como argumento, se utiliza para configurar la conexión a la base de datos.
        public CursosOnlineContext(DbContextOptions options) : base(options)
        {
            // Constructor que llama a la clase base (IdentityDbContext) con las opciones proporcionadas.
        }

        // Método que se llama durante la configuración del modelo de la base de datos.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Llamar al método base para realizar la configuración predeterminada.
            base.OnModelCreating(modelBuilder);

            // Se configura una clave primaria compuesta para la entidad CursoInstructor utilizando los campos CursoId e InstructorId.
            modelBuilder.Entity<CursoInstructor>().HasKey(ci => new { ci.CursoId, ci.InstructorId });
            // Define una clave primaria compuesta para la entidad CursoInstructor utilizando CursoId e InstructorId.

            // Aquí se pueden agregar más configuraciones de modelos y relaciones según las necesidades del proyecto.
        }

        // Propiedades que representan las tablas en la base de datos.
        public DbSet<Curso> Curso { get; set; }
        public DbSet<Precio> Precio { get; set; }
        public DbSet<Comentario> Comentario { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<CursoInstructor> CursoInstructor { get; set; }
    }
}
