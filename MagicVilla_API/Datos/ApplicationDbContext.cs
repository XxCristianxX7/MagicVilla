using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicVilla_API.Datos
{
    public class ApplicationDbContext :DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
            
        }
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                    new Villa()
                    {
                        Id = 1,
                        Nombre = "Villa Real",
                        Detalle = "Detalle de la villa... ",
                        ImagenUrl = "",
                        Ocupantes = 5,
                        MetrosCuadrados = 50,
                        Tarifa = 200,
                        Amenidad ="",
                        FechaCreacion= DateTime.Now,
                        FechaActualizaion = DateTime.Now

                    },
                    new Villa()
                    {
                        Id = 2,
                        Nombre = "Premium Villa",
                        Detalle = "Vista a la piscina",
                        ImagenUrl = "",
                        Ocupantes = 4,
                        MetrosCuadrados = 40,
                        Tarifa = 220,
                        Amenidad = "",
                        FechaCreacion = DateTime.Now,
                        FechaActualizaion = DateTime.Now

                    }
                    );
        }
    }
}
