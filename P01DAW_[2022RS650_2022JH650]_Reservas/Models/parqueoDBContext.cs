using Microsoft.EntityFrameworkCore;

namespace P01DAW__2022RS650_2022JH650__Reservas.Models
{
    public class parqueoDBContext : DbContext
    {

        public parqueoDBContext(DbContextOptions<parqueoDBContext> options) : base(options)
        {
        }
        public DbSet<parqueo> parqueo { get; set; }
        public DbSet<reserva> reserva { get; set; }
        public DbSet<usuarios> usuarios { get; set; }
        public DbSet<sucursal_parqueo> sucursal_parqueo { get; set; }
    }

    }
